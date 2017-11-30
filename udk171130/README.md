samples and lines
--------------------

supercollider
==

first we need to find a few soundfiles to use. the examples below expects a mono drumloop sample that is four beats long as well as five single drum sound samples (preferably from a 808 drum machine).
find your own or download free ones from [here](http://www.users.globalnet.co.uk/%7Espufus/drums.htm) and [here](http://trashaudio.com/2010/01/roland-tr-808-sample-pack).

NOTE: can not be `.mp3`. in supercollider it is recommended to use `.aiff` or `.wav`

```supercollider
s.boot;
s.meter;
s.scope;

// http://www.users.globalnet.co.uk/~spufus/drums/brushmuff1.wav
(
~loop.free;
~loop= Buffer.readChannel(s, "/Users/asdf/Desktop/loops/brushmuff1.wav", channels:[0]);  //load a 4 beat long drumloop
)

~loop.play;  //this should play the loop once in left channel only
//if no sound make sure you gave the correct path between the "" above - see post window for any errors


//--load a sampler (a synth definition for playing buffers)
(
SynthDef(\avsamp, {|out= 0, buf, rate= 1, offset= 0, atk= 0.005, rel= 0.01, cur= -4, amp= 0.1, pan= 0, gate= 1, loop= 1|
    var env= EnvGen.ar(Env.asr(atk, 1, rel, cur), gate, doneAction:2);
    var src= PlayBuf.ar(1, buf, rate*BufRateScale.ir(buf), 1, offset*BufFrames.ir(buf), loop);
    OffsetOut.ar(out, Pan2.ar(src*env*amp, pan));
}).add;
)

~loop.duration;  //buffer duration in seconds. should return 2.4 for brushmuff1.wav
4/~loop.duration*60;  //calculate bpm from duration and number of beats in file

TempoClock.tempo= 100/60;  //set default tempo to match soundfile
TempoClock.tempo= 4/~loop.duration;  //or same thing but simpler. number of beats / duration in seconds

(
Pdef(\loop).quant= 1;
Pdef(\loop, PmonoArtic(\avsamp,
    \buf, ~loop,
    \dur, 4,  //note the four beats as duration here
    \legato, 0.95,  //legato is default 0.8. we want to play almost whole file. try different values
    \amp, 0.75,
)).play;
)

(
Pdef(\loop).quant= 1;
Pdef(\loop, PmonoArtic(\avsamp,
    \buf, ~loop,
    \dur, 4,
    \legato, Pseq([0.95, 0.8], inf),  //shorter every other time - this subltre stop makes a huge difference
    \amp, 0.75,
)).play;
)

(
Pdef(\loop).quant= 1;
Pdef(\loop, PmonoArtic(\avsamp,
    \buf, ~loop,
    \dur, Pseq([2, 2, 4], inf),  //variation - only play half loop duration the first 2 times
    \legato, 0.95,
    \amp, 0.75,
)).play;
)

(
Pdef(\loop).quant= 1;
Pdef(\loop, PmonoArtic(\avsamp,
    \buf, ~loop,
    \dur, Pseq([0.5, 1.5, 2, 4], inf),  //more complex rhythm with duration pattern
    \legato, 0.95,
    \amp, 0.75,
)).play;
)

(
Pdef(\loop).quant= 1;
Pdef(\loop, PmonoArtic(\avsamp,
    \buf, ~loop,
    \dur, 2,
    \offset, Pseq([0, 0, 0.75, 0.5], inf),  //offset set starting position in sample (in percent)
    \legato, 0.95,
    \amp, 0.75,
)).play;
)

(
Pdef(\loop).quant= 1;
Pdef(\loop, PmonoArtic(\avsamp,
    \buf, ~loop,
    \dur, 1,
    \offset, Pseq([0, 0.25, 0.5, 0.75, 0.75, 0.5, 0.125, 0.125], inf),  //first play straight through, then 'stutter backwards'
    \legato, 0.95,
    \amp, 0.75,
)).play;
)

//nested patterns
(
Pdef(\loop).quant= 1;
Pdef(\loop, PmonoArtic(\avsamp,
    \buf, ~loop,
    \dur, Pseq([1, 1, 1, Pseq([1/8], 8)], inf),
    \offset, Pseq([0, 0.25, 0.5, Pseq([0.375], 8)], inf),  //stuttering in the end
    \legato, 0.95,
    \amp, 0.75,
)).play;
)

(
Pdef(\loop).quant= 1;
Pdef(\loop, PmonoArtic(\avsamp,
    \buf, ~loop,
    \dur, Pseq([4, 1, 1, 1, Pseq([1/8], 8)], inf),
    \offset, Pseq([0, 0, 0.25, 0.5, Pseq([0.375], 8)], inf),
    \rate, Pseq([1, 1, 1, 1, Pseq([1, 1.5, 2, 2.5, 3, 3.5, 4, 4.5], 1)], inf),  //playback rate
    \legato, 0.95,
    \amp, 0.75,
)).play;
)

(
Pdef(\loop).quant= 1;
Pdef(\loop, PmonoArtic(\avsamp,
    \buf, ~loop,
    \dur, Pseq([4, 1, 1, 1, Pseq([1/16], 16)], inf),
    \offset, Pseq([0, 0, 0.25, 0.5, Pseq([0.375], 16)], inf),
    \rate, Pseq([1, 1, 1, 1, Pseq([1, 1.5, 2, 2.5, 3, 3.5, 4, 4.5, 5, 5.5, 6, 6.5, 7, 7.5, 8, 8.5], 1)], inf),
    \legato, 0.95,
    \amp, 0.75,
    \pan, Pseq([0, 0, 0, 0, Pseq([-0.5, 0.5], 8)], inf),
)).play;
)

//exactly the same thing but simpler using Pseries
(
Pdef(\loop).quant= 1;
Pdef(\loop, PmonoArtic(\avsamp,
    \buf, ~loop,
    \dur, Pseq([4, 1, 1, 1, Pseq([1/16], 16)], inf),
    \offset, Pseq([0, 0, 0.25, 0.5, Pseq([0.375], 16)], inf),
    \rate, Pseq([1, 1, 1, 1, Pseries(1, 0.5, 16)], inf),
    \legato, 0.95,
    \amp, 0.75,
    \pan, Pseq([0, 0, 0, 0, Pseq([-0.5, 0.5], 8)], inf),
)).play;
)

//or exponential growth instead of linear
(
Pdef(\loop).quant= 1;
Pdef(\loop, PmonoArtic(\avsamp,
    \buf, ~loop,
    \dur, Pseq([4, 1, 1, 1, Pseq([1/16], 16)], inf),
    \offset, Pseq([0, 0, 0.25, 0.5, Pseq([0.375], 16)], inf),
    \rate, Pseq([1, 1, 1, 1, Pgeom(1, 1.2, 16)], inf),
    \legato, 0.95,
    \amp, 0.75,
    \pan, Pseq([0, 0, 0, 0, Pseq([-0.5, 0.5], 8)], inf),
)).play;
)

Pseries(1, 0.5, 16).asStream.all;
Pgeom(1, 1.2, 16).asStream.all;


//--set another tempo
TempoClock.tempo= 119/60;  //bpm / 60 = beats per second

//might get more interesting results with the sequencer (pdef) playing at a tempo that does not match the soundfile
(
Pdef(\loop).quant= 4;
Pdef(\loop, PmonoArtic(\avsamp,
    \buf, ~loop,
    \dur, ~loop.duration,
    \legato, 0.8,
    \amp, 0.75,
)).play;
)

(
Pdef(\loop).quant= 4;
Pdef(\loop, PmonoArtic(\avsamp,
    \buf, ~loop,
    \dur, 1,
    \offset, Pseq([0, 0.25, 0, 0.5], inf),
    \legato, 0.8,
    \amp, 0.75,
)).play;
)

(
Pdef(\loop).quant= 4;
Pdef(\loop, PmonoArtic(\avsamp,
    \buf, ~loop,
    \dur, 0.5,
    \offset, Pseq([0, 0.25, 0, 0.5], inf),
    \legato, Pseg(Pseq([0.6, 0.9], inf), 16),  //subtle variation in length
    \amp, 0.75,
)).play;
)

(
Pdef(\loop).quant= 4;
Pdef(\loop, PmonoArtic(\avsamp,
    \buf, ~loop,
    \dur, 0.5,
    \offset, Pseq([0, 0.25, 0, 0.5], inf)+Pseg(Pseq([0, 0.25], inf), 100),  //slowly shift offsetposition - strange effect
    \legato, 0.8,
    \amp, 0.75,
)).play;
)

(
Pdef(\loop).quant= 4;
Pdef(\loop, PmonoArtic(\avsamp,
    \buf, ~loop,
    \dur, Pseq([1, 1, 1, 1 ,1, 1, 1, 0.5, 0.5], inf),
    \offset, Pseq([0, 0.25, 0, 0.5, 0, 0.25, 0, 0.5, 0.75], inf),  //sort of cut to pieces and reconstructed
    \legato, 0.5,
    \amp, 0.75,
)).play;
)


//--tempo + rate

//here transpose and keep the soundfile tempo by speeding up the playback (rate)
(
TempoClock.tempo= 4/~loop.duration*1.2;  //1.2 should match 1.2 for rate below
Pdef(\loop).quant= 1;
Pdef(\loop, PmonoArtic(\avsamp,
    \buf, ~loop,
    \dur, 4,
    \rate, 1.2,
    \legato, 0.95,
    \amp, 0.75,
)).play;
)


//multiple sequencers using the same soundfile loop

(
TempoClock.tempo= 4/~loop.duration*1.75;
Pdef(\loop).quant= 4;
Pdef(\loop, PmonoArtic(\avsamp,
    \buf, ~loop,
    \dur, Pseq([4, 4, 2, 2, 1, 1, 1, 1], inf),
    \rate, Pseq([1, 1, 1, 1, 1, 1, 1, -1]*1.75, inf),  //last backwards
    \legato, 0.6,
    \amp, 0.75,
)).play;
)

//second
(
Pdef(\loop2).quant= 4;
Pdef(\loop2, PmonoArtic(\avsamp,
    \buf, ~loop,
    \dur, 2,
    \rate, 1.8/2,  //soundfile played octave down
    \legato, 0.75,
    \offset, 0.25,
    \amp, 0.75,
)).play;
)

//third
(
Pdef(\loop3).quant= 4;
Pdef(\loop3, PmonoArtic(\avsamp,
    \buf, ~loop,
    \dur, 4,  //full duration
    \rate, 1.8,
    \legato, 0.5,  //don't play full legato
    \offset, 0.125,  //start at 1/8 into the file
    \amp, 0.75,
)).play;
)

//fourth
(
Pdef(\loop4).quant= 4;
Pdef(\loop4, PmonoArtic(\avsamp,
    \buf, ~loop,
    \dur, Pseq([1, 1, 1, 1, 1, 1, 1, 0.75, 0.25], inf),
    \rate, 1.8/1.5,
    \legato, 0.2,
    \offset, 0.0175,
    \amp, 0.75,
    \pan, Pseq([0.3, -0.3], inf),
)).play;
)

Pdef(\loop).stop;
Pdef(\loop2).stop;
Pdef(\loop3).stop;
Pdef(\loop4).stop;

Pdef(\loop).play;
Pdef(\loop2).play;
Pdef(\loop3).play;
Pdef(\loop4).play;

CmdPeriod.hardRun;  //stop all

//now try loading different soundfiles into the ~loop buffer
//adjust number of beats in your pattern/tempo code if necessary

//or try recording from internal microphone like last week...
(
SynthDef(\avrec, {|buf|
    RecordBuf.ar(Limiter.ar(SoundIn.ar), buf, loop:0, doneAction:2);
}).add;
)

Synth(\avrec, [\buf, ~loop]);  //record into ~loop buffer (only overwrite what is in memory - not on disk)

Pdef(\loop).play;  //and try all the different patterns above

Pdef(\loop).stop;



//--

//drum kit with single buffers
(
~kick.free;
~kick= Buffer.read(s, "/Users/asdf/Desktop/Roland TR-808 Normalised/BassDrum/KickDrum0004.aif");
~snare.free;
~snare= Buffer.read(s, "/Users/asdf/Desktop/Roland TR-808 Normalised/SnareDrum/SnareDrum0004.aif");
~hihat.free;
~hihat= Buffer.read(s, "/Users/asdf/Desktop/Roland TR-808 Normalised/Cls'd Hihat/Closed Hihat0004.aif");
~hihatopen.free;
~hihatopen= Buffer.read(s, "/Users/asdf/Desktop/Roland TR-808 Normalised/Open Hihat/Open Hihat0004.aif");
~clap.free;
~clap= Buffer.read(s, "/Users/asdf/Desktop/Roland TR-808 Normalised/Misc/Clap.aif");
)

//--set yet another tempo
TempoClock.tempo= 160/60;  //bpm / 60 = beats per second

(
Pdef(\kick).quant= 4;
Pdef(\kick, PmonoArtic(\avsamp,
    \buf, ~kick,
    \amp, 0.5,
)).play;
)

(
Pdef(\snare).quant= 4;
Pdef(\snare, PmonoArtic(\avsamp,
    \buf, ~snare,
    \dur, 1,
    \amp, Pseq([0, 1], inf)*0.5,
)).play;
)

(
Pdef(\hihat).quant= 4;
Pdef(\hihat, PmonoArtic(\avsamp,
    \buf, ~hihat,
    \dur, 0.5,
    \amp, Pseq([0.125, 0.75], inf)*0.5,
)).play;
)

(
Pdef(\clap).quant= 4;
Pdef(\clap, PmonoArtic(\avsamp,
    \buf, ~clap,
    \dur, Pseq([1, 7], inf),
    \amp, Pseq([0.2, 0.4], inf),
)).play;
)

//now make it better


//here my attempt - fixing up the clap
(
Pdef(\clap).quant= 4;
Pdef(\clap, PmonoArtic(\avsamp,
    \buf, ~clap,
    \dur, 0.25,
    \rate, Pseq([1, 1, 1, 1.25], inf),
    \offset, Pseq([0, 0.01], inf),
    \amp, Pbjorklund(7, 16)*0.2,
)).play;
)

(
Pdef(\hihat).quant= 4;
Pdef(\hihat, PmonoArtic(\avsamp,
    \buf, Pseq([Pseq([~hihat], 7), ~hihatopen], inf),
    \dur, Pseq([0.52, 0.48], inf),  //'swing' the hihat a bit
    \rate, 0.9,
    \amp, Pbjorklund(9, 16)*Pseq([0.25, 0.75], inf)*0.5,
    \pan, Pseq([0, -0.3, 0.3, 0], inf),
)).play;
)

(
Pdef(\snare).quant= 4;
Pdef(\snare, PmonoArtic(\avsamp,
    \buf, ~snare,
    \dur, Pseq([Pseq([1], 14), 0.8, 1.2], inf),
    \rate, 0.86,
    \legato, Pseg(Pseq([0.15, 0.3], inf), 64),
    \amp, Pseq([Pseq([0, 1], 6), 0.25, 1], inf)*0.5,
)).play;
)

(
Pdef(\kick).quant= 4;
Pdef(\kick, PmonoArtic(\avsamp,
    \buf, ~kick,
    \amp, Pseq([Pseq([0.5], 32-5), Pseq([0], 5+32)], inf),
    \atk, 0,
)).play;
)


//--load two effects
(
SynthDef(\avecho, {|in= 0, out= 0, del= 0.1, dec= 1|
    var snd= In.ar(in, 2);
    var efx= CombN.ar(snd, 1, del.clip(0, 1), dec, 0.5);
    Out.ar(out, efx);
}).add;
SynthDef(\avdist, {|in= 0, out= 0, dist= 20|
    var snd= In.ar(in, 2);
    snd= (snd*(dist+1)).sin*(1/(dist+1));
    Out.ar(out, snd);
}).add;
)

//--start the effects
a.free; a= Synth(\avdist, [\in, 0, \dist, 0]);
b.free; b= Synth(\avecho, [\in, 0, \dec, 1]);

b.set(\del, 0.05);
b.set(\del, 0.04);
b.set(\del, 0.1);
b.set(\dec, 3);
b.set(\dec, 1);
a.set(\dist, 10);
a.set(\dist, 0);
b.free;  //stop the echo
a.free;  //stop the dist

Pdef(\kick).stop;
Pdef(\hihat).stop;
Pdef(\snare).stop;
Pdef(\clap).stop;
```

- - -

unity3d
==

* start unity and create a new 3D project. give it a name (here 'lines').
* select Assets / Create / Material
* give the material a name (can be anything - here 'LineMat') by typing under the icon
* at the top of the inspector for the material select Shader / Particles / Additive
* also click on the Particle Texture icon and select 'Default-Particle'
* your scene should now look like this...

![00material](00material.png?raw=true "00material")

* select GameObject / Create Empty
* select Component / Effect / Line Renderer
* in the GameObject's inspector find Materials and flip down / expand it
* click on the little circle next to Element 0 and select LineMat by double clicking

![01linematerial](01linematerial.png?raw=true "01linematerial")

press play and start exploring. change the Positions / Size to for example 5 and then the individual x, y, z, positions below. try the Color and set start and end to different colours. etc.

* create a new script by selecting Assets / Create / C# Script
* give the script the name 'Lines' by typing under the white icon
* double click the white C# script icon to open it in MonoDevelop
* copy and paste in the code here below replacing what was there originally

```cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lines : MonoBehaviour {
    int num= 10;
    LineRenderer line;
    void Start () {
        line = GetComponent<LineRenderer> ();
        line.positionCount = num;
        line.loop = true;  //optional wrap back to beginning
    }
    void Update () {
        line.startWidth = 0.1F;
        line.endWidth = 0.1F;
        for (int i = 0; i < num; i++) {
            line.SetPosition (i, new Vector3 ((i % 5) * 2 - 1, (i % 6) * 2 - 1, (i % 7) * 2 - 1));
        }
    }
}
```

* save and switch back to unity
* attach the script to the empty GameObject by drag&drop, or by menu Component / Scripts / Lines, or by clicking Add Component in inspector (3 ways to do the same thing)
* press play and you should see this...

![02lines](02lines.png?raw=true "02lines")

experiment with the code. add public variables. make it animate.

```cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lines : MonoBehaviour {
    public float startwidthspeed= 0.01F;  //added these
    public float startwidthamount= 4.0F;
    public float endwidthspeed= 0.011F;
    public float endwidthamount= 4.0F;
    int num= 10;
    LineRenderer line;
    void Start () {
        line = GetComponent<LineRenderer> ();
        line.positionCount = num;
        line.loop = true;
        line.numCornerVertices = 90;  //added this
    }
    void Update () {
        line.startWidth = Mathf.Sin(Time.frameCount*startwidthspeed)*startwidthamount;  //simple animation
        line.endWidth = Mathf.Sin(Time.frameCount*endwidthspeed)*endwidthamount;
        for (int i = 0; i < num; i++) {
            line.SetPosition (i, new Vector3 ((i % 5) * 2 - 1, (i % 6) * 2 - 1, (i % 7) * 2 - 1));
        }
    }
}
```

* select the material we made (LineMat)
* in the inspector click 'Select' in the Particle Texture icon and select 'Default-Particle'

![03material](03material.png?raw=true "03material")

press play and play around more with the settings.

specially try changing Texture Mode under GameObject / Line Renderer inspector. set it to 'Tile' and you should see something like this...

![04texturemode](04texturemode.png?raw=true "04texturemode")

* select Main Camera and set the clear flags to 'Depth only'
* change in Material to Shader / Particle / Alpha Blended
* set the material's Tint Color to full white
* now try this code...

```cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lines : MonoBehaviour {
    public float startwidthspeed= 0.01F;
    public float startwidthamount= 4.0F;
    public float endwidthspeed= 0.011F;
    public float endwidthamount= 4.0F;
    public Vector3 rotationspeeds= new Vector3(0.4F, 0.5F, 0.6F);  //new public variable
    public Color startcolor = new Color (0.1F, 0.2F, 0.3F, 0.01F);
    public Color endcolor = new Color (0.9F, 0.8F, 0.7F, 0.01F);
    int num= 500;
    LineRenderer line;
    void Start () {
        line = GetComponent<LineRenderer> ();
        line.positionCount = num;
        line.loop = true;
        line.numCornerVertices = 50;
        line.useWorldSpace= false;  //need to be off for rotation to work
    }
    void Update () {
        transform.localEulerAngles = rotationspeeds * (1.0F*Time.frameCount);  //added this
        line.startWidth = Mathf.Sin(Time.frameCount*startwidthspeed)*startwidthamount;
        line.endWidth = Mathf.Sin(Time.frameCount*endwidthspeed)*endwidthamount;
        line.startColor = startcolor;
        line.endColor = endcolor;
        for (int i = 0; i < num; i++) {
            line.SetPosition (i, new Vector3 ((i % 15) * 2 - 1, (i % 16) * 2 - 1, (i % 17) * 2 - 1));
        }
    }
}

```

should give you something like...

![05fivehundred](05fivehundred.png?raw=true "05fivehundred")

now try more materials and adding them to the line renderer (set Materials / Size to for example 4 and then set your new materials under elements 0-3)

links
==

Unity 5.5 Line Renderer - https://www.youtube.com/watch?v=nzgJ3JkClx4
