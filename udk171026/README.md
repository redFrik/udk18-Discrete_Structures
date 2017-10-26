pattern phasing
--------------------

this time we will experiment with a very (the most?) simple way of combining patterns... shifting one of them in time or in space so that they play out of phase.

supercollider
==

first a brief overview...

- `Pseq` - seq stands for sequence and P for pattern
- `Pseq([1, 2, 3, 4, 5], inf);` - numbers in a sequence. `inf` is the number of repetitions (inf= infinite)
- `Pseq([1, 2, 3, 4, 5], 3);` - this will repeate only 3 times and then the pattern is finished
- `Pseq([0.1, 4, 0.3, -3], inf);` - can contain any numbers (and also symbols or other patterns)
 - `Pseq([0, 1, Pseq([5, 4, 3], 2), -1], inf);` - nested patterns. result: `0, 1, 5, 4, 3, 5, 4, 3, -1, 0, 1, 5, 4 3, 5, 4, 3, -1, 0, 1` etc infinite times

 - `Pdef` - def stands for definition. here used as a kind of global variable / placeholder
 - `Pdef(\somename).play;`  - start the Pdef named \somename
 - `Pdef(\somename).stop;` - stop \somename. i.e. reference the Pdef by name
 - `Pdef(\other).stop;` - refer to another Pdef called \other and stop it

 - `PmonoArtic`  - is our monophonic sequencer. it has a lot of built-in functionallity but we only need to give it a few arguments - the rest defaults
 - `a= PmonoArtic(\mysound).play;` - first argument is synthdef name (here causes error because we did not load any sound called \mysound yet)
 - `a.stop;`- stop the [broken] sequencer
 
 - `a= PmonoArtic(\mysound, \freq, 400, \amp, 0.2).play;` - additional arguments after synthdef name are then all key, value pairs
 - `a.stop;`
 - `a= PmonoArtic(\mysound, \freq, Pseq([400, 500, 600, 700], inf), \amp, 0.2).play;` - or key, pattern pairs
 - `a.stop;`

these key, value paris can either be controlling the PmonoArtic or the SynthDef. it is difficult in the beginning to discern between which keys belong to which. below `legato` and `dur` are for PmonoArtic while the rest will control the synth.

```supercollider
s.boot;
s.meter;
s.scope;


//--load a sound
(
SynthDef(\avping, {|out= 0, freq= 400, atk= 0.01, rel= 0.1, cur= -4, amp= 0.1, gate= 1, pan= 0, mod= 1|
    var env= EnvGen.ar(Env.asr(atk, 1, rel, cur), gate, doneAction:2);
    var snd= SinOsc.ar(freq, SinOsc.ar(freq*mod, env*pi, mod));
    Out.ar(out, Pan2.ar(snd*env, pan, amp));
}).add;
)

//--start playing
(
Pdef(\ping1, PmonoArtic(\avping,
    \freq, Pseq([300, 400, 600, 533], inf),  //change these
    \dur, 2,
    \legato, 0.1,  //this key for example is for the PmonoArtic sequencer
    \atk, 0.01,    //while this key belongs to the \avping SynthDef
    \rel, 5,
    \amp, 0.5,
)).play;
)

(
Pdef(\ping1, PmonoArtic(\avping,
    \freq, Pseq([300, 400, 600, 533], inf),
    \dur, Pseq([2, 1, 1, 4], inf),  //new durations (instead of the static 2 seconds above)
    \legato, 0.1,
    \atk, 0.01,
    \rel, 5,
    \amp, 0.5,
)).play;
)

Pdef(\ping1).stop;  //stop the sequencer that we stored with the name \ping1 in a Pdef

//two sequences - starts in sync but then drifts apart
//because the second sequencer have a pattern with 5 frequencies while the first only 4
(
Pdef(\ping1, PmonoArtic(\avping,  //same as above
    \freq, Pseq([300, 400, 600, 533], inf),
    \dur, Pseq([2, 1, 1, 4], inf),
    \legato, 0.1,
    \atk, 0.01,
    \rel, 5,
    \amp, 0.5,
    \pan, -1,  //pan to the left speaker
)).play;
Pdef(\ping2, PmonoArtic(\avping,  //this second one is the lower frequency
    \freq, Pseq([300, 400, 600, 533, 400], inf)*0.5,  //note: here added one frequency (400) and octave down (*0.5)
    \dur, Pseq([2, 1, 1, 4], inf),
    \legato, 0.1,
    \atk, 0.01,
    \rel, 5,
    \amp, 0.5,
    \pan, 1,  //pan to the right speaker
)).play;
)

//adding a third sequencer - this one 6 frequencies in its pattern - also plays with double duration
(
Pdef(\ping3, PmonoArtic(\avping,
    \freq, Pseq([300, 400, 600, 533, 400, 300], inf)*2,  //added one frequency (300) and octave up(*2)
    \dur, Pseq([2, 1, 1, 4], inf)*2,  //double durations
    \legato, 0.1,
    \atk, 1.5,  //much longer attack time
    \rel, 5,
    \amp, 0.3,  //slightly lower amplitude
    \pan, 0,  //pan to the middle
)).play;
)

Pdef(\ping1).stop;
Pdef(\ping2).stop;
Pdef(\ping3).stop;

//so with such simple patterns of different length you get a very long repetition cycle


//--pattern basics
(
Pdef(\ping1, PmonoArtic(\avping,
    \freq, Pseq([300, 400, 600, 533], inf),  //change these
    \dur, 0.125,
    \legato, 0.2,
    \amp, 1,
)).play;
)

(
Pdef(\ping1, PmonoArtic(\avping,
    \freq, Pseq([300, 400, 600, 533], inf),
    \dur, 0.125,
    \legato, 0.2,
    \amp, Pseq([1, 1, 0.25, 0], inf),  //pattern (instead of static 1 like above)
)).play;
)

(
Pdef(\ping1, PmonoArtic(\avping,
    \freq, Pseq([300, 400, 600, 533], inf),
    \dur, 0.125,
    \legato, 0.2,
    \amp, Pseq([1, 1, 0.25, 0,  1, 0, 0.5, 0], inf),  //4+4 length - try shifting these numbers around
)).play;
)

(
Pdef(\ping1, PmonoArtic(\avping,
    \freq, Pseq([300, 400, 600, 533], inf),
    \dur, 0.125,
    \legato, 0.2,
    \amp, Pseq([1, 1, 0.25, 0,  1, 0, 0.5, 0,  1, 1, 0.25, 0,  1, 0, 0.5, 0.5], inf),  //4+4+4+4 length
)).play;
)

//now patterns with different lengths (isorhythms)
(
Pdef(\ping1, PmonoArtic(\avping,
    \freq, Pseq([300, 400, 600, 533], inf),
    \dur, 0.125,
    \legato, 0.2,
    \amp, Pseq([1, 1, 0.25, 0, 0.5], inf),  //5 in total - gives five amplitudes against four frequencies
)).play;
)

//because there is 4 frequencies and 5 amplitudes, the total cycle will start over and repeate every 20th beat (4.lcm(5))

(
Pdef(\ping1, PmonoArtic(\avping,
    \freq, Pseq([300, 400, 600, 533, 800, 700], inf),  //now six frequencies here
    \dur, 0.125,
    \legato, 0.2,
    \amp, Pseq([1, 1, 0.25, 0, 0.5], inf),  //still 5 - gives five against six
)).play;
)

//for this the total cycle length is 30th beats (5.lcm(6))

//six against five against four (repeats every 60th beat ([4, 5, 6].reduce('lcm')))
(
Pdef(\ping1, PmonoArtic(\avping,
    \freq, Pseq([300, 400, 600, 533, 800, 700], inf),
    \dur, 0.125,
    \legato, Pseq([1, 0.2, 0.1, 0.05], inf),  //legato can also be a pattern - here four
    \amp, Pseq([1, 1, 0.25, 0, 0.5], inf),
)).play;
)

//six against five against four against three against two (also repeats after 60 beats because 4 and 6 divides evenly with 2 and 3)
(
Pdef(\ping1, PmonoArtic(\avping,
    \freq, Pseq([300, 400, 600, 533, 800, 700], inf),
    \dur, 0.125,
    \legato, Pseq([1, 0.2, 0.1, 0.05], inf),
    \amp, Pseq([1, 1, 0.25, 0, 0.5], inf),
    \rel, Pseq([1, 0.5, 0.1], inf),  //also release pattern - here three
    \pan, Pseq([-0.5, 0.5], inf),  //also panning pattern - here two
)).play;
)

//but quickly more complex with 7 instead of 6 frequencies (now repeate every 420th beat [4, 5, 7, 3, 2].reduce('lcm'))
(
Pdef(\ping1, PmonoArtic(\avping,
    \freq, Pseq([300, 400, 600, 533, 800, 700, 500], inf),
    \dur, 0.125,
    \legato, Pseq([1, 0.2, 0.1, 0.05], inf),
    \amp, Pseq([1, 1, 0.25, 0, 0.5], inf),
    \rel, Pseq([1, 0.5, 0.1], inf),  //this is three so better with seven instead of six frequencies
    \pan, Pseq([-0.5, 0.5], inf),
)).play;
)

//lots of parameters and patterns of different length (repeats every 13860 beat)
(
Pdef(\ping1, PmonoArtic(\avping,
    \freq, Pseq([300, 400, 600, 533, 800, 700, 500], inf),
    \dur, Pseq([0.125, 0.125, 0.125, 0.125, 0.125, 0.125, 0.125, 0.125, 0.333, 0.333, 0.333], inf),
    \legato, Pseq([1, 0.2, 0.1, 0.05], inf),
    \amp, Pseq([1, 1, 0.25, 0, 0.5], inf),
    \atk, Pseq([1, 0.001, 0.001, 0.001, 0.001, 0.001, 0.001, 0.001, 0.001], inf),
    \rel, Pseq([1, 3, 0.5, 0.1], inf),
    \mod, Pseq([0, 0, 1, 0.5], inf),
    \pan, Pseq([-0.5, 0.5], inf),
)).play;
)

Pdef(\ping1).stop;


//--math operations on patterns
(
Pdef(\ping1, PmonoArtic(\avping,
    \freq, Pseq([300, 400, 600, 533], inf)*2,  //multiply with 2 will result in frequencies one octave above
    \dur, 0.125,
    \legato, 0.2,
    \amp, 1,
)).play;
)

(
Pdef(\ping1, PmonoArtic(\avping,
    \freq, Pseq([300, 400, 600, 533], inf)*Pseq([1, 1, 1, 2, 0.5], inf),  //multiplying two patterns
    \dur, 0.125,
    \legato, 0.2,
    \amp, 1,
)).play;
)

(
Pdef(\ping1, PmonoArtic(\avping,
    \freq, Pseq([300, 400, 600, 533], inf)/2,  //divide by 2 will result in frequencies one octave below (same as *0.5)
    \dur, 0.125,
    \legato, 0.2,
    \amp, 1,
)).play;
)

(
Pdef(\ping1, PmonoArtic(\avping,
    \freq, Pseq([300, 400, 600, 533], inf)/Pseq([1, 1, 2, 1, 1.5], inf),  //dividing two patterns
    \dur, 0.125,
    \legato, 0.2,
    \amp, 1,
)).play;
)

(
Pdef(\ping1, PmonoArtic(\avping,
    \freq, Pseq([300, 400, 600, 533], inf)+Pseq([0, 0, 0, 200, -100], inf),  //adding two patterns
    \dur, 0.125,
    \legato, 0.2,
    \amp, 1,
)).play;
)

Pdef(\ping1).stop;


//--nested patterns
(
Pdef(\ping1, PmonoArtic(\avping,
    \freq, Pseq([Pseq([200, 800], 4), 300, 400, 600, 533], inf),  //200, 800 four times, then continue
    \dur, 0.25,
    \legato, 0.2,
    \amp, 1,
)).play;
)

(
Pdef(\ping1, PmonoArtic(\avping,
    \freq, Pseq([Pseq([200, 800, Pseq([500, 400], 2)], 3), 300, 400, 600, 533], inf),  //nested nested patterns
    \dur, 0.25,
    \legato, 0.2,
    \amp, 1,
)).play;
)

(
Pdef(\ping1, PmonoArtic(\avping,
    \freq, Pseq([Pseq([200, 800, Pseq([500, 400], 3)], 3), 300, 400, 600, 533, 800, 900, 1000, 1200], inf),  //more notes in the end - 1+1+(2*3)*3+8= 32 total
    \dur, 0.125,  //faster
    \legato, 0.2,
    \amp, 1,
)).play;
)

(
Pdef(\ping1, PmonoArtic(\avping,
    \freq, Pseq([Pseq([200, 800, Pseq([500, 400], 3)], 3), 300, 400, 600, 533, 800, 900, 1000, 1200], inf),  //32 frequences in this pattern
    \dur, Pseq([Pseq([0.125], 24), Pseq([0.0625], 8)], inf),  //8+24 adds up to 32 events
    \legato, 0.2,
    \amp, 1,
)).play;
)

//this is the same as above but written out manually without the nested patterns and repetitions
(
Pdef(\ping1, PmonoArtic(\avping,
    \freq, Pseq([
        200, 800, 500, 400, 500, 400, 500, 400,
        200, 800, 500, 400, 500, 400, 500, 400,
        200, 800, 500, 400, 500, 400, 500, 400,
        300, 400, 600, 533, 800, 900, 1000, 1200], inf),  //32 in total
    \dur, Pseq([
        0.125, 0.125, 0.125, 0.125, 0.125, 0.125, 0.125, 0.125,
        0.125, 0.125, 0.125, 0.125, 0.125, 0.125, 0.125, 0.125,
        0.125, 0.125, 0.125, 0.125, 0.125, 0.125, 0.125, 0.125,
        0.0625, 0.0625, 0.0625, 0.0625, 0.0625, 0.0625, 0.0625, 0.0625], inf),
    \legato, 0.2,
    \amp, 1,
)).play;
)

Pdef(\ping1).stop;


//--finite length
(
Pdef(\ping1, PmonoArtic(\avping,
    \freq, Pseq([Pseq([200, 800, Pseq([500, 400], 3)], 3), 300, 400, 600, 533, 800, 900, 1000, 1200], inf),
    \dur, Pseq([Pseq([0.125], 24), Pseq([0.0625], 8)], inf),
    \legato, 0.2,
    \amp, Pseq([1], 32),  //only play 32 events and then stop the whole sequencer - compare with inf
)).play;
)


//--phasing - one sequencer running slightly slower
//event duration is ever so little longer for ping2
//so everything is gradually phasing out of sync
(
Pdef(\ping1, PmonoArtic(\avping,
    \freq, Pseq([300, 400, 600, 533], inf),  //change these
    \dur, 0.25,
    \legato, 0.2,
    \amp, 0.5,
    \pan, -0.5,  //pan left
)).play;
Pdef(\ping2, PmonoArtic(\avping,
    \freq, Pseq([300, 400, 600, 700], inf),  //change these too
    \dur, 0.252,  //how fast drift apart - compare with 0.25 in dur for ping1
    \legato, 0.2,
    \amp, 0.5,
    \pan, 0.5,  //pan right
)).play;
)

//try to extend and add many more pings - all with different dur. e.g. 0.25, 0.252, 0.254, 0.256 etc.


//--Steve Reich - Clapping Music
//notic how the pattern is 12 beats long and is to be repeated 8 times
//then each 8x the second pattern adds one single beat (a pause) to the end
//so the two patterns are phasing out of sync in discrete steps

(
Pdef(\ping1).stop;
Pdef(\ping2).stop;
Pdef(\counter).stop;
Pdef(\ping1, PmonoArtic(\avping,
    \freq, 400,
    \dur, 0.125,
    \legato, 0.3,
    \amp, Pseq([1, 1, 1, 0, 1, 1, 0, 1, 0, 1, 1, 0], inf)*0.5,
    \atk, 0.002,
    \pan, -0.5,
)).play;
Pdef(\ping2, PmonoArtic(\avping,
    \freq, 300,
    \dur, 0.125,
    \legato, 0.3,
    \amp, Pseq([Pseq([1, 1, 1, 0, 1, 1, 0, 1, 0, 1, 1, 0], 8), 0], inf)*0.5,  //notice 8x and then the extra 0
    \atk, 0.002,
    \pan, 0.5,
)).play;
Pdef(\counter, PmonoArtic(\avping,
    \dur, 0.125*12,
    \index, Pseq([1, 2, 3, 4, 5, 6, 7, 8], 12).trace(prefix: "counter "),
    \amp, 0,  //silent - only for posting
)).play;
)


//--Steve Reich - Piano Phase
//simplified version - same phrase for both sequencers - only difference is that the second one play slightly slower
(
Pdef(\ping1).stop;
Pdef(\ping2).stop;
Pdef(\ping1, PmonoArtic(\avping,
    \freq, Pseq([64, 66, 71, 73, 74, 66, 64, 73, 71, 66, 74, 73], inf).midicps,  //midicps convert from midi to Hz (cycles per second)
    \dur, 0.125,
    \legato, 0.4,
    \amp, 0.5,
    \atk, 0.002,
    \rel, 0.15,
    \pan, -0.5,
)).play;
Pdef(\ping2, PmonoArtic(\avping,
    \freq, Pseq([64, 66, 71, 73, 74, 66, 64, 73, 71, 66, 74, 73], inf).midicps,
    \dur, Pseq([0.1254], inf),  //notice the difference here - try different values
    \legato, 0.4,
    \amp, 0.5,
    \atk, 0.002,
    \rel, 0.15,
    \pan, 0.5,
)).play;
)

//--more and different synths
//while any of the above sequencers are playing we can change the synth sound
(
SynthDef(\avping, {|out= 0, freq= 400, atk= 0.01, rel= 0.1, cur= -4, amp= 0.1, gate= 1, pan= 0, mod= 1|
    var env= EnvGen.ar(Env.asr(atk, 1, rel, cur), gate, doneAction:2);
    var snd= Mix(VarSaw.ar(freq*[1, 2, 3], 0, env.lag(mod/8)/[2, 3, 4], 1/[2, 4, 8]));
    Out.ar(out, Pan2.ar(snd*env, pan, amp));
}).add;
)

(
SynthDef(\avping, {|out= 0, freq= 400, atk= 0.01, rel= 0.1, cur= -4, amp= 0.1, gate= 1, pan= 0, mod= 1|
    var env= EnvGen.ar(Env.asr(atk, 1, rel, cur), gate, doneAction:2);
    var snd= Mix(Pulse.ar(freq*[1, 2, 8], 0.4+LFTri.ar([1, 2, 3], 0, mod*0.4)))/3;
    Out.ar(out, Pan2.ar(snd*env, pan, amp));
}).add;
)

(
SynthDef(\avping, {|out= 0, freq= 400, atk= 0.01, rel= 0.1, cur= -4, amp= 0.1, gate= 1, pan= 0, mod= 1|
    var env= EnvGen.ar(Env.asr(atk, 1, rel, cur), gate, doneAction:2);
    var snd= SinOsc.ar(freq, env*mod);
    Out.ar(out, Pan2.ar(snd*env, pan, amp));
}).add;
)

//back to what we had at the top of this example
(
SynthDef(\avping, {|out= 0, freq= 400, atk= 0.01, rel= 0.1, cur= -4, amp= 0.1, gate= 1, pan= 0, mod= 1|
    var env= EnvGen.ar(Env.asr(atk, 1, rel, cur), gate, doneAction:2);
    var snd= SinOsc.ar(freq, SinOsc.ar(freq*mod, env*pi, mod));
    Out.ar(out, Pan2.ar(snd*env, pan, amp));
}).add;
)
```

- - -

unity3d
==

these examples are very similar to [last week](https://github.com/redFrik/udk18-Discrete_Structures/tree/master/udk171019#unity3d). see there for screenshots of menus etc.
and just like last week's example we will let the script create a lot of objects from a single prefab. the difference now is that we will rotate each object separately and not just rotate the main camera.

* start unity and create a new **2D** project. give it a name (here phasing2d)
* create a new script by selecting Assets / Create / C# Script
* give the script the name 'Phasing2D' by typing under the white icon
* double click the white C# script icon to open it in MonoDevelop
* copy and paste in the code here below replacing what was there originally

```cs
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Phasing2D : MonoBehaviour {
    public Transform prefab;    //holds our initial gameobject
    List<Transform> clones = new List<Transform>();
    int dim = 10;    //10x10= 100 objects in total
    public Vector2 rotationSpeed;  //rotation in x y
    public float spread= 0.0F;  //phasing offset
    void Start() {  //do once when starting
        float spacing= 1.25F;    //space between gameobjects
        float scale = 0.25F;     //size of gameobject
        rotationSpeed.x= 1.0F;
        rotationSpeed.y= 1.0F;
        prefab.transform.localScale = new Vector3(scale, scale, scale);
        for (int x = 0; x < dim; x++) {  //the order in which clones are created matter
            for (int y = 0; y < dim; y++) {  //try swapping the x and y loops here
                clones.Add(Instantiate(prefab, new Vector2(x-(dim/2), y-(dim/2))*spacing, Quaternion.identity));
            }
        }
        transform.position= new Vector3(0, 0, 0);  //default camera position
    }
    void Update() {  //do every frame - many times per second
        int i= 0;
        foreach(Transform ct in clones) {
            ct.localEulerAngles = rotationSpeed*(Time.frameCount+(i*spread));
            i++;
        }
    }
}
```

* save and switch back to unity
* in the upper left hierachy window, click to select the 'Main Camera'
* attach the script to the camera by selecting Component / Scripts / Phasing2D
* create a new plane by selecting GameObject / 3D Object / Plane. this game object will become our prefab from which all clones will be made
* again select the main camera and in the inspector click on the little circle next to prefab. select the Plane by doubleclicking in the dialog that pops up
* create a light by selecting GameObject / Light / Directional Light
* your scene should now look like this...

![00scene](00scene.png?raw=true "00scene")

* click play (or press cmd+p)
* change the numbers in various number boxes. try rotationSpeed and spread values - also go negative
* click stop (or press cmd+p again)

remember: unity3d does not store your settings when you are in play mode.

* stop and select Assets / Create / Material
* give the material a name (here 'PhasingMat') by typing under the icon
* in the upper left hierachy window, click to select the 'Plane'
* in the inspector find Materials and then under there click on the little circle next to Element 0. select the PhasingMat by doubleclicking in the dialog that pops up
* you should now see something like this...

![01material](01material.png?raw=true "01material")

* play around some more and try to come up with something strange
* things to try:
  * change the colour, metallic, smoothness of the material
  * change the colour, intensity, position etc of the light
  * change camera clear flag, projection, field of view etc
  * don't forget to mess with the rotationSpeed and spread values
  * also try to change the code in the script. add more objects, change scaling and spacing
  * etc

![02strange](02strange.png?raw=true "02strange")

let us try to arrange the objects in a circle instead of in a square. so in the code we will use sin and cos math functions instead of x (rows) and y (columns).

* open the C# code and find line 19. replace what is there with this...

```cs
clones.Add(Instantiate(prefab, new Vector2(Mathf.Sin((x*dim+y)/Mathf.PI*2*dim*dim), Mathf.Cos((x*dim+y)/Mathf.PI*2*dim*dim))*3.3F*spacing, Quaternion.identity));
```
* save and switch back to unity
* again play around with the values - specially the x, y rotation speeds and the spread variable (also go negative).

![03circle](03circle.png?raw=true "03circle")

3d
--

again we now repeate the same thing but with cubes and in 3D...

* restart unity and create a new **3D** project. give it a name (here phasing3d)
* create a new script by selecting Assets / Create / C# Script
* give the script the name 'Phasing3D' by typing under the white icon
* double click the white C# script icon to open it in MonoDevelop
* copy and paste in the code here below replacing what was there originally

```cs
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Phasing3D : MonoBehaviour {
    public Transform prefab;    //holds our initial gameobject
    List<Transform> clones = new List<Transform>();
    int dim = 10;    //10x10*10= 1000 objects in total
    public Vector3 rotationSpeed;  //rotation in x y z
    public float spread= 0.0F;     //phasing offset
    public Vector3 scaling;        //scaling in x y z
    void Start() {  //do once when starting
        float spacing= 1.25F;    //space between gameobjects
        float scale = 0.5F;     //size of gameobject
        rotationSpeed.x= 1.0F;
        rotationSpeed.y= 1.0F;
        rotationSpeed.z= 1.0F;
        scaling.x= 1.0F;
        scaling.y= 1.0F;
        scaling.z= 1.0F;
        prefab.transform.localScale = new Vector3(scale, scale, scale);
        for (int x = 0; x < dim; x++) {
            for (int y = 0; y < dim; y++) {
                for (int z = 0; z < dim; z++) {
                    clones.Add(Instantiate (prefab, new Vector3(x-(dim/2), y-(dim/2), z-(dim/2))*spacing, Quaternion.identity));
                }
            }
        }
        transform.position= new Vector3(0, 0, -10);  //default camera position
    }
    void Update() {  //do every frame - many times per second
        int i= 0;
        foreach(Transform ct in clones) {
            ct.localEulerAngles = rotationSpeed*(Time.frameCount+(i*spread));
            ct.localScale = scaling*((i+1)/(1.0F*dim*dim*dim));
            i++;
        }
    }
}
```
note how this script is very similar to the 2d version above.

* save and switch back to unity
* attach the script to the main camera by selecting Component / Scripts / Phasing3D
* create a new cube by selecting GameObject / 3D Object / Cube
* in the main camera inspector click on the little circle next to prefab and select the Cube by doubleclicking
* click play (or press cmd+p)
* by default your scene should now look like this...

![04cubes](04cubes.png?raw=true "04cubes")

* now change the numbers in various number boxes. try rotationSpeed and spread values - also go negative
* things to try:
  * change the colour, intensity, type of light
  * change camera position, scale, clear flag, projection, field of view
  * also try to change the code in the script. add more objects, change scaling and spacing
  * add more lights in different colours
  * etc

* stop and select Assets / Create / Material
* give the material a name (here 'PhasingMat') by typing under the icon
* in the upper left hierachy window, click to select the 'Cube'
* in the inspector find Materials and then under there click on the little circle next to Element 0. select the PhasingMat by doubleclicking in the dialog that pops up
* click play (or press cmd+p) and start chaning things - also the material

![05cubeofcubes](05cubeofcubes.png?raw=true "05cubeofcubes")

last we arrange the cubes in a sphere...

* open the C# code and find line 25 (the inside of the z for loop - the line that starts with `clones.Add`). replace that line with these lines...

```cs
float xx = Mathf.Sin ((x * dim + y) / Mathf.PI * 2 * dim * dim);
float yy = Mathf.Cos ((x * dim + y) / Mathf.PI * 2 * dim * dim);
float zz = Mathf.Sin (z / (1.0F * dim)*Mathf.PI);
clones.Add(Instantiate(prefab, new Vector3(xx*zz, yy*zz, z/(1.0F*dim)*2-1)*4.5F*spacing, Quaternion.identity));
```

* save and switch back to unity
* again play around with the values - specially the spread and scaling variables (also go negative).

![06sphereofcubes](06sphereofcubes.png?raw=true "06sphereofcubes")

- - -

links
==

Steve Reich - Clapping Music https://www.youtube.com/watch?v=QNZQzpWCTlA and https://www.youtube.com/watch?v=lzkOFJMI5i8

Steve Reich - Piano Phase https://www.youtube.com/watch?v=7P_9hDzG1i0

poi https://www.youtube.com/watch?v=PHOO01O_pAQ
