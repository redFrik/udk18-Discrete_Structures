granular synthesis and particle systems
--------------------

supercollider
==

again we need to find some soundfiles to work with. like last week something like speech will work great.

download this one... https://raw.githubusercontent.com/redFrik/udk18-Discrete_Structures/master/udk171214/granular.aiff (on osx you can alt+click the link to download the file directly)

or record your own using [Audacity](http://www.audacityteam.org).

NOTE: can not be `.mp3`. in supercollider it is recommended to use `.aiff` or `.wav`

```supercollider
//mac osx only... will write a soundfile to your desktop
"say -o ~/Desktop/granular.aiff --data-format=BEI16@44100 granular".unixCmd;
```

```supercollider
s.boot;
s.meter;
s.scope;

(
~source.free;
~source= Buffer.readChannel(s, "/Users/asdf/Desktop/granular.aiff", channels:[0]);  //edit this line with the path to your own soundfile
)

//--load a sampler (a synth definition for playing a short grain of a buffer)
(
SynthDef(\avgrain, {|out= 0, buf, rate= 1, offset= 0, time= 0.1, amp= 0.1, pan= 0|
    var env= EnvGen.ar(Env.sine(time), doneAction:2);
    var src= PlayBuf.ar(1, buf, rate*BufRateScale.ir(buf), 1, offset*BufFrames.ir(buf), 1);
    OffsetOut.ar(out, Pan2.ar(src*env*amp, pan));
}).add;
)

Env.sine(0.1).plot;  //100ms fixed duration envelope. goes smoothly from 0 to 1 and back
Env.sine(0.1).test;

//--technical note:
//below we will use Pbind and not PmonoArtic like before. this is because
//Pbind does not need to send a 'note-off' (gate= 0) osc message for each
//synth. the synths will play for a fixed duration (here called 'time').
//so we can save 50% of the network traffic by only sending a 'note-on'
//and this is a huge saving when we are generating thousands of synths.
//therefore Pbind instead of PmonoArtic.

(
Pdef(\gran, Pbind(\instrument, \avgrain,
    \buf, ~source,
    \dur, 0.5,   //half a second delay between each grain/event
    \time, 0.5,  //half a second envelope
    \offset, 0,  //starting at the beginning - try change (range 0-1)
    \amp, 0.75
)).play;
)

(
Pdef(\gran, Pbind(\instrument, \avgrain,
    \buf, ~source,
    \dur, 0.25,   //faster - quarter of a second
    \time, 0.25,  //and shorter
    \offset, 0,
    \amp, 0.75,
)).play;
)

(
Pdef(\gran, Pbind(\instrument, \avgrain,
    \buf, ~source,
    \dur, 0.125,   //even faster
    \time, 0.125,  //and also shorter
    \offset, 0,
    \amp, 0.75,
)).play;
)

//--offset
//we can control where in the file our segment should be taken from
(
Pdef(\gran, Pbind(\instrument, \avgrain,
    \buf, ~source,
    \dur, 0.125,
    \time, 0.125,
    \offset, Pseq([0, 0.5], inf),  //now we alternate offset (starting) position - try different ones (0-1)
    \amp, 0.75,
)).play;
)

(
Pdef(\gran, Pbind(\instrument, \avgrain,
    \buf, ~source,
    \dur, 0.125,
    \time, 0.125,
    \offset, Pseq([0, 0.125, 0.25, 0.375], inf),  //stepping from beginning in 0.125 increments - observe what happens - why we start hearing the original file?
    \amp, 0.75,
)).play;
)

(
Pdef(\gran, Pbind(\instrument, \avgrain,
    \buf, ~source,
    \dur, 0.125,
    \time, 0.125,
    \offset, Pseg(Pseq([0, 1], inf), Pseq([~source.duration, 0], inf)),  //this is a way to step through from 0 to 1 respecting the duration of the soundfile
    \amp, 0.75,
)).play;
)

//--overlaps
//to get smoother results we can keep the speed (dur) and let offsets step 0-1
//but then increase the envelope length (time) so that each segment overlap
(
Pdef(\gran, Pbind(\instrument, \avgrain,
    \buf, ~source,
    \dur, 0.125,
    \time, 0.125*2,  //note - sounds much better with x2 overlap
    \offset, Pseg(Pseq([0, 1], inf), Pseq([~source.duration, 0], inf)),
    \amp, 0.75,
)).play;
)

//--time-stretch
//using overlaps in combination with stepping through offsets faster or slower,
//we can play through the file and time-stretch without pitch-shift.
//(compare with pitch-shift examples below)
(
Pdef(\gran, Pbind(\instrument, \avgrain,
    \buf, ~source,
    \dur, 0.125,
    \time, 0.125*2,
    \offset, Pseg(Pseq([0, 1], inf), Pseq([~source.duration*0.8, 0], inf)),  //80% of original duration
    \amp, 0.75,
)).play;
)

(
Pdef(\gran, Pbind(\instrument, \avgrain,
    \buf, ~source,
    \dur, 0.125,
    \time, 0.125*2,
    \offset, Pseg(Pseq([0, 1], inf), Pseq([~source.duration*1.5, 0], inf)),  //slower (you will hear some artefacts)
    \amp, 0.75,
)).play;
)

(
Pdef(\gran, Pbind(\instrument, \avgrain,
    \buf, ~source,
    \dur, 0.125/2,  //play double speed and thereby more segments
    \time, Pkey(\dur)*2,  //a smart way to write x2 overlap using Pkey
    \offset, Pseg(Pseq([0, 1], inf), Pseq([~source.duration*1.5, 0], inf)),  //sounds better
    \amp, 0.75,
)).play;
)

(
Pdef(\gran, Pbind(\instrument, \avgrain,
    \buf, ~source,
    \dur, 0.125/8,  //play much faster
    \time, Pkey(\dur)*2,  //x2 overlap
    \offset, Pseg(Pseq([0, 1], inf), Pseq([~source.duration*4, 0], inf)),  //more extreme time stretch (lots of artefacts)
    \amp, 0.75,
)).play;
)

(
Pdef(\gran, Pbind(\instrument, \avgrain,
    \buf, ~source,
    \dur, 0.125/8,
    \time, Pkey(\dur)*10,  //much more overlap can give interesting results
    \offset, Pseg(Pseq([0, 1], inf), Pseq([~source.duration*4, 0], inf)),
    \amp, 0.75,
)).play;
)

//--randomness
//one common way to lessen the artefacts is to add a little bit of randomisation
//here we randomise the offset (starting position) adding small values with gaussian distribution around 0
(
Pdef(\gran, Pbind(\instrument, \avgrain,
    \buf, ~source,
    \dur, 0.125/16,  //very fast
    \time, Pkey(\dur)*10,  //lots of overlap
    \offset, Pseg(Pseq([0, 1], inf), Pseq([~source.duration*4, 0], inf))+Pgauss(0, 0.002),  //added noise
    \amp, 0.75,
)).play;
)

(
Pdef(\gran, Pbind(\instrument, \avgrain,
    \buf, ~source,
    \dur, 0.125/24,  //super fast
    \time, Pkey(\dur)*15,  //x15 overlap
    \offset, Pseg(Pseq([0, 1], inf), Pseq([~source.duration*8, 0], inf))+Pgauss(0, 0.002),  //now even longer (x8). still sounds ok
    \amp, 0.5,  //note a bit lower amp to not clip
)).play;
)

//--pitch-shift
//back to original duration (no stretch - stepping through offsets 0-1 at soundfile duration).
//but if we play each grain with a different playback rate we can transpose up/down without
//affecting pitch
(
Pdef(\gran, Pbind(\instrument, \avgrain,
    \buf, ~source,
    \dur, 0.125/8,
    \time, Pkey(\dur)*4,
    \rate, 0.75,  //note each grain is played back at a slower rate
    \offset, Pseg(Pseq([0, 1], inf), Pseq([~source.duration, 0], inf))+Pgauss(0, 0.002),
    \amp, 0.75,
)).play;
)

(
Pdef(\gran, Pbind(\instrument, \avgrain,
    \buf, ~source,
    \dur, 0.125/8,
    \time, Pkey(\dur)*4,
    \rate, 0.5,  //much lower (artefacts)
    \offset, Pseg(Pseq([0, 1], inf), Pseq([~source.duration, 0], inf))+Pgauss(0, 0.002),
    \amp, 0.75,
)).play;
)

(
Pdef(\gran, Pbind(\instrument, \avgrain,
    \buf, ~source,
    \dur, 0.125/8,
    \time, Pkey(\dur)*12,  //with much more overlap some artefacts go away but you loose clarity in sound
    \rate, 0.5,
    \offset, Pseg(Pseq([0, 1], inf), Pseq([~source.duration, 0], inf))+Pgauss(0, 0.002),
    \amp, 0.5,
)).play;
)

//--combine pitch-shift and time-stretch
(
Pdef(\gran, Pbind(\instrument, \avgrain,
    \buf, ~source,
    \dur, 0.125/16,
    \time, Pkey(\dur)*12,
    \rate, 1.2,
    \offset, Pseg(Pseq([0, 1], inf), Pseq([~source.duration*4, 0], inf))+Pgauss(0, 0.002),
    \amp, 0.75,
)).play;
)

(
Pdef(\gran, Pbind(\instrument, \avgrain,
    \buf, ~source,
    \dur, 0.125/32,
    \time, Pkey(\dur)*12,
    \rate, 1.5,
    \offset, Pseg(Pseq([0, 1], inf), Pseq([~source.duration*6, 0], inf))+Pgauss(0, 0.002),
    \amp, 0.75,
)).play;
)

//--experiments
(
Pdef(\gran, Pbind(\instrument, \avgrain,
    \buf, ~source,
    \dur, 0.125/16+Pgauss(0, 0.0001),
    \time, Pkey(\dur)*20,
    \rate, Pseg(Pseq([1.1, 0.75], inf), 16),  //pattern
    \offset, Pseg(Pseq([0, 1], inf), Pseq([~source.duration*14, 0], inf))+Pgauss(0, 0.002),
    \amp, Pgauss(0.5, 0.1),
    \pan, Pgauss(0, 0.5),
)).play;
)

(
Pdef(\gran, Pbind(\instrument, \avgrain,
    \buf, ~source,
    \dur, 0.125/32+Pgauss(0, 0.0001),  //duration with added randomness
    \time, Pgauss(0.2, 0.01),  //grain length is random
    \rate, Pseg(Pseq([1.1, 0.75], inf), 16)+Pgauss(0.25, 0.1),  //pitch variation
    \offset, Pseg(Pseq([0, 1], inf), Pseq([~source.duration*14, 0], inf))+Pgauss(0, 0.01),  //randomness here = spread
    \amp, Pgauss(0.25, 0.1),  //vary amplitude too
    \pan, Pgauss(0, 0.5),  //and panning
)).play;
)

(
Pdef(\gran, Pbind(\instrument, \avgrain,
    \buf, ~source,
    \dur, 0.125/32+Pgauss(0, 0.0001),
    \time, Pgauss(0.2, 0.01),
    \rate, Pseg(Pseq([1.1, 0.75], inf), 16)*Pseq([1, 1.25, 1.5, 1.75], inf),  //rate slowly going up and down
    \offset, Pseg(Pseq([0, 1], inf), Pseq([~source.duration*14, 0], inf))+Pgauss(0, 0.01),
    \amp, Pgauss(0.25, 0.1)*Pwrand([1, 0], [0.9, 0.1], inf),
    \pan, Pgauss(0, 0.5),
)).play;
)

//no randomness - just deterministic patterns - sounding more machine like
(
Pdef(\gran, Pbind(\instrument, \avgrain,
    \buf, ~source,
    \dur, 0.01,
    \time, Pkey(\dur)*15,
    \rate, Pseq([0, 3, 5, 7].midiratio, inf),
    \offset, Pseg(Pseq([0, 1], inf), Pseq([~source.duration*24, 0], inf)),
    \amp, Pseq([1, 1, 1, 1, 0], inf)*0.5,
    \pan, Pseq([-0.5, 0, 0.5, 0], inf),
)).play;
)

//slowly scanning through the soundfile
(
Pdef(\gran, Pbind(\instrument, \avgrain,
    \buf, ~source,
    \dur, 0.01,
    \time, Pkey(\dur)*25,
    \rate, Pseq([0, 4, 7, 11].midiratio, inf),
    \offset, Pseg(Pseq([0, 1], inf), Pseq([~source.duration*24, 1], inf)),
    \amp, Pseq([1, 1, 1, 1, 1, 0], inf)*0.5,
    \pan, Pseq([-0.5, 0, 0, 0.5, 0], inf),
)).play;
)

//scanning through extremely slow - ambient piece?
(
Pdef(\gran, Pbind(\instrument, \avgrain,
    \buf, ~source,
    \dur, 0.005*Pseq([1, 1.01, 1.02, 1.03], inf),
    \time, Pkey(\dur)*20,
    \rate, Pseq([0, 4, 7, 11].midiratio, inf),
    \offset, Pseg(Pseq([0, 1], inf), Pseq([~source.duration*100, 0], inf)),
    \amp, Pseq([1, 1, 1, 1, 1, 0, 1, 1, 1, 0, 1, 1, 1], inf)*0.5,
    \pan, Pseq([-0.5, 0, 0, 0.5, 0], inf),
)).play;
)

//experiment!
//note: make sure duration is never 0. watch your cpu usage and save often. also keep an eye on level meters. scale down your amplitude if you are clipping
```

ideas
--

* use the built-in microphone and record new sounds for the granulator while it is running.

```supercollider
(
SynthDef(\avrec, {|buf|
    RecordBuf.ar(Limiter.ar(SoundIn.ar), buf, loop:0, doneAction:2);
}).add;
)

Synth(\avrec, [\buf, ~source]);  //record into ~source buffer (only overwrite what is in memory - not on disk)
```

* edit the synthdef and try with different envelopes - not only the smooth sine. note: make sure to add the `time` argument to scale durations.

```supercollider
Env.triangle(0.1).plot;
Env.perc(0.01, 0.1).plot;
Env.linen(0.01, 0.01, 0.1, 1, [4, 0, -5]).plot;
```

* load more buffers and try blending grains from different buffers i.e. pick randomly or in a sequence - crossfade by adjusting the weights

```supercollider
\buf, Pwrand([~source, ~source2, ~source3], [0.8, 0.1, 0.1], inf),  //weighted randomness
```

* recommended reading: Curtis Roads - Microsound (2001)

- - -

granulator
--

try the code in [granulator.scd](https://github.com/redFrik/udk18-Discrete_Structures/blob/master/udk171214/granulator.scd?raw=true). this example loads a whole directory of soundfiles and play back fragments from all or some of them. it also includes a gui.

unity3d
==

* start unity and create a new 3D project. give it a name (here 'part').
* select Assets / Create / Material
* give the material a name (can be anything - here 'PartMat') by typing under the icon
* at the top of the inspector for the material select Shader / Particles / Additive
* optionally set the Tint Color to something (here green)
* also click on the Particle Texture icon and select 'Default-Particle'
* select GameObject / Create Empty
* select Component / Effects / Particle System
* drag and drop your PartMat material onto the GameObject
* move around the GameObject - observe how the particle system follow along

![00setup](00setup.png?raw=true "00setup")

* find the 'Simulation Space' option in the Particle System inspector and change from 'Local' to 'World'
* again move around the GameObject - observe the difference

![01simulation](01simulation.png?raw=true "01simulation")

* here we want to use 'World' so keep that setting
* set the following under 'Emission' and 'Shape'...

![02settings](02settings.png?raw=true "02settings")

* select the Main Camera and set 'Clear Flags' to 'Solid Color'
* select a background
* change 'Field of View' (here 120) and you should see something like this...

![03green](03green.png?raw=true "03green")

now experiment with the particles. things to try:

* start speed and start size
* gravity modifier
* different shapes (e.g. Donut)
* activate 'Color over Lifetime' and set start and end colours
* min and max particle size (under Renderer)
* etc

when you are happy you can add the following script to move the particle system around...

* create a new script by selecting Assets / Create / C# Script
* give the script the name 'PartMove' by typing under the white icon
* double click the white C# script icon to open it in MonoDevelop
* copy and paste in the code here below replacing what was there originally

```cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartMove : MonoBehaviour {
    public float xspeed= 2.0F;
    public float yspeed= 3.0F;
    public float zspeed= 4.0F;
    public float scale = 5.0F;
    public float xrot = 1.0F;
    public float yrot = 0.0F;
    public float zrot = 0.0F;
    void Start () {
    }
    void Update () {
        transform.localPosition = new Vector3 (Mathf.Sin (Time.time * xspeed) * scale, Mathf.Cos (Time.time * yspeed) * scale, 0);
        //transform.localPosition = new Vector3 (Mathf.Sin (Time.time * xspeed) * scale, Mathf.Cos (Time.time * yspeed) * scale, Mathf.Sin (Time.time * zspeed) * scale);
        transform.localEulerAngles = new Vector3 (Time.time*xrot, Time.time*yrot, Time.time*zrot);
    }
}
```

* attach the script to the GameObject by drag and drop.
* play around with the script variables
* try the different variants by commenting out lines

technical note: since last week i've discovered `Time.time` which should give ever so slightly smoother animation compared to `Time.frameCount`

multiple
--

the following example creates many particle systems

* start unity and create a new 3D project. give it a name (here partmult)
* create a new script by selecting Assets / Create / C# Script
* give the script the name 'PartMult' by typing under the white icon
* double click the white C# script icon to open it in MonoDevelop
* copy and paste in the code here below replacing what was there originally

```cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartMult : MonoBehaviour {
    public GameObject prefab;
    public float spread= 1.0F;
    public float scale = 5.0F;
    int num= 10;  //number of objects (and particle systems)
    List<GameObject> list= new List<GameObject>();
    void Start() {
        for (int i = 0; i < num; i++) {
            list.Add (Instantiate (prefab, new Vector3 (0, 0, 0), Quaternion.identity));
        }
        prefab.gameObject.SetActive (false);  //hide prefab object
    }
    void Update () {
        float i = 0;
        foreach (GameObject go in list) {
            i++;
            float theta = (Time.time * spread) + ((i / num)*Mathf.PI*2);
            ParticleSystem ps = go.GetComponent<ParticleSystem> ();
            ps.startColor = new Color(1-(i/num), 0.1F, i/num, 1.0F);
            var em= ps.emission; em.rateOverTime= i*10;
            //var sh = ps.shape; sh.arc = i*10;

            go.transform.localPosition = new Vector3 (
                Mathf.Sin(theta)*scale,
                Mathf.Cos(theta)*scale,
                1
            );
        }
    }
}
```

* create a new material by selecting Assets / Create / Material
* give the material a name (can be anything - here 'PartMat') by typing under the icon
* at the top of the inspector for the material select Shader / Particle / Additive (or some other shader also possible)
* also click on the Particle Texture icon and select 'Default-Particle' (or import your own photo and add it here)
* select GameObject / Create Empty (can also be a sphere or some other object if you want to try)
* select Component / Effects / Particle System
* drag and drop the PartMat material onto the GameObject
* drag and drop the PartMult script onto the Main Camera
* drag the GameObject onto Prefab variable in Main Camera's inspector
* set Main Camera's 'Clear Flags' to 'Solid Color' and select a black background
* play

![05multiple](05multiple.png?raw=true "05multiple")

modify the code and play with the particle system in the prefab (stop first before trying to make changes). also try setting 'Simulation Space' to 'World' (though 'Local' works better here i think). change the prefab's material etc etc.

links
==

https://en.wikipedia.org/wiki/Granular_synthesis

[Introduction To Unity: Particle Systems](https://www.raywenderlich.com/113049/introduction-unity-particle-systems)
