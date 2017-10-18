introduction and overview
--------------------

* links to previous semesters... <http://redfrik.github.io/udk00-Audiovisual_Programming/>
* and dates + times for this course... <https://github.com/redFrik/udk18-Discrete_Structures> <-save this page

unity3d
==

* install [unity](http://unity3d.com)

* start unity and create a new **2D** project. give it a name (here grid)
* create a new script by selecting Assets / Create / C# Script

![00script](00script.png?raw=true "00script")

* give the script a name by typing under the white icon (here Grid)
* doublie click the white C# script icon to open it in MonoDevelop (the first time you might be asked to install extra stuff)

![01icon](01icon.png?raw=true "01icon")

* copy and paste in the code here below replacing all what was in the script originally

```cs
using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour {
    public Transform prefab;    //holds our initial sphere
    public Vector2 camrot;  //camera rotation x y
    void Start() {
        int dim = 20;    //20x20= 400
        camrot.x = 0.3F;    //default x rotation speed
        camrot.y = 0.2F;    //default y rotation speed
        float spacing= 1.5F;    //space between spheres
        float scale = 0.5F;        //size of spheres
        prefab.transform.localScale = new Vector3(scale, scale, scale);
        for (int x = 0; x < dim; x++) {
            for (int y = 0; y < dim; y++) {
                Instantiate(prefab, new Vector2(x-(dim/2), y-(dim/2))*spacing, Quaternion.identity);
            }
        }
        transform.position= new Vector3(0, 0, 0);  //default camera position
    }
    void Update() {
        transform.localEulerAngles= camrot*Time.frameCount;    //do the rotation each frame
    }
}
```

* save and switch back to unity

* in the upper left hierachy window, click to select the 'Main Camera'

![02maincamera](02maincamera.png?raw=true "02maincamera")

* to attach the script to the camera now select Component / Scripts / Grid (or whatever you called it)

![03addscript](03addscript.png?raw=true "03addscript")

* you should see the following in the inspector window...

![04inspector](04inspector.png?raw=true "04inspector")

* create a new sphere by selecting GameObject / 3D Object / Sphere.  this game object will become our prefab from which all the clones will be made

![05sphere](05sphere.png?raw=true "05sphere")

* again select the main camera and in the inspector click on the little circle next to prefab and select the Sphere by doubleclicking in the dialog that pops up
* your scene should now look like this...

![06scene](06scene.png?raw=true "06scene")

* click play (or press cmd+p)
* change the numbers in various number boxes. try Camrot values - also go negative
* click stop (or press cmd+p again)
* select GameObject / Light / Directional Light

![07light](07light.png?raw=true "07light")

* again press play and start changing things
* things to try:
  * change the colour, intensity, type of the light (first select light in the hierarchy window)
  * change camera position, scale, clear flag, projection, field of view (select camera in the hierarchy window)
  * don't forget to mess with the Camrot values for x and y rotation speed
  * also try to change the code in the script

3d
--

now we try the same thing but with cubes and in 3D...

* restart unity and create a new **3D** project. give it a name (here matrix)
* create a new script by selecting Assets / Create / C# Script
* give the script a name (here Matrix)
* doublie click the icon to open it in MonoDevelop
* copy and paste in the code here below replacing all what was in the script originally

```cs
using UnityEngine;
using System.Collections;

public class Matrix : MonoBehaviour {
    public Transform prefab;
    public Vector3 camrot;  //camera rotation x y z
    void Start() {
        int dim = 10;  //10x10x10= 1000. careful when increasing this - not too much
        camrot.x = 0.3F;
        camrot.y = 0.2F;
        camrot.z = 0.1F;
        float spacing= 5.0F;    //try changing this - around 2.0 is good
        float scale = 1.0F;        //and this - 0.1 is a good value
        prefab.transform.localScale = new Vector3 (scale, scale, scale);
        for (int x = 0; x < dim; x++) {
            for (int y = 0; y < dim; y++) {
                for (int z = 0; z < dim; z++) {    //notice the third dimension
                    Instantiate(prefab, new Vector3(x-(dim/2), y-(dim/2), z-(dim/2))*spacing, Quaternion.identity);
                }
            }
        }
        transform.position= new Vector3(0, 0, 0);
    }
    void Update() {
        transform.localEulerAngles= camrot*Time.frameCount;
    }
}
```

note how this script is very similar to the 2d version above.

* save and switch back to unity
* attach the script to the main camera by selecting Component / Scripts / Matrix (or whatever you called it)
* select GameObject / 3D Object / Cube
* in the main camera inspector click on the little circle next to prefab and select the Cube by doubleclicking
* your scene should now look like this...

![09cubescene](09cubescene.png?raw=true "09cubescene")

* click play (or press cmd+p)
* change the numbers in various number boxes. try Camrot values - also go negative
* things to try:
  * change the colour, intensity, type of the light
  * change camera position, scale, clear flag, projection, field of view
  * also try to change the code in the script
  * add more lights
  * etc

![10cubes](10cubes.png?raw=true "10cubes")

![11cubes2](11cubes2.png?raw=true "11cubes2")

![12cubes3](12cubes3.png?raw=true "12cubes3")

the screenshot above is with: `float spacing= 3.0F;` and `float scale = 0.9F;` in the script and a point light.

- - -
 
supercollider
==

* install [supercollider](http://supercollider.github.io/download)

* copy all the code below here into a new document
* evaluate line by line, block by block

```supercollider
//--boot the server
s.boot;
s.meter;
s.scope;

//--load sounds and effects
(
SynthDef(\avsynth, {|out= 0, freq= 400, atk= 0.01, rel= 0.1, cur= -4, amp= 0.1, gate= 1, pan= 0, mod= 1, filtAmt= 200, filtQ= 0.1, filtLag= 0|
    var env= EnvGen.ar(Env.asr(atk, 1, rel, cur), gate, doneAction:2);
    var snd= VarSaw.ar(freq, 0, LFDNoise3.ar(env*mod, 0.5, 0.5));
    snd= BLowPass4.ar(snd, env.lag(filtLag)*filtAmt+(filtAmt*4), 1-env+filtQ.clip(0.01, 30));
    Out.ar(out, Pan2.ar(snd*env, pan, amp));
}).add;
SynthDef(\avverb, {|in= 0, out= 0, room= 1, drylevel= 0.1|
    var snd= In.ar(in, 2);
    var efx= Mix({AllpassN.ar(HPF.ar(snd, {50.0.rrand(100)}!2), 0.1, {0.005.rrand(0.1)}!2, 3*room, 0.15)}!8);
    efx= efx+Mix({AllpassN.ar(HPF.ar(efx, {100.0.rrand(300)}!2), 0.25, {0.05.exprand(0.25)}!2, 4*room, 0.1)}!4);
    Out.ar(out, snd*drylevel+efx);
}).add;
SynthDef(\avdist, {|in= 0, out= 0, dist= 20|
    var snd= In.ar(in, 2);
    snd= (snd*(dist+1)).sin*(1/(dist+1));
    Out.ar(out, snd);
}).add;
)

//--start playing
(
Pdef(\pat1, PmonoArtic(\avsynth,
    \freq, Pseq([300, 400, 600, 700], inf),  //change these
    \dur, 0.25,
    \legato, 0.2,  //change this from 0 to 1
)).play;
)

(
Pdef(\pat1, PmonoArtic(\avsynth,
    \freq, Pseq([200, 400, 600, 700], inf),  //first tone is lower
    \dur, 0.25,
    \legato, 0.5,  //all tones longer
)).play;
)

(
Pdef(\pat1, PmonoArtic(\avsynth,
    \freq, Pseq([200, 400, 600, 700, 800, 500, 400, 300], inf),  //more
    \dur, 0.25,
    \legato, 0.05,  //shorter
)).play;
)

(
Pdef(\pat1, PmonoArtic(\avsynth,
    \freq, Pseq([200, 400, 600, 700, 800, 500, 400, 300], inf),
    \dur, 0.125,  //everything faster
    \legato, 0.05,
)).play;
)

(
Pdef(\pat1, PmonoArtic(\avsynth,
    \freq, Pseq([200, Pseq([300, 1200], 4), 600, 700, 800, 500, 400, 300], inf),  //nested patterns
    \dur, 0.125,
    \legato, 0.05,
)).play;
)

(
Pdef(\pat1, PmonoArtic(\avsynth,
    \freq, Pseq([Pseq([Pseq([100, 300, 400], 2), 1200], 3), 600, 700, 800, 500, 400, 300], inf),  //nested nested patterns
    \dur, 0.125,
    \legato, Pseq([Pseq([0.1], 8), 0.5, 0.6, 0.7, 0.8, 0.9, 1], inf),  //8 repeasts instead of inf
)).play;
)

(
Pdef(\pat1, PmonoArtic(\avsynth,
    \freq, Pseq([Pseq([Pseq([250, 300, 350], 2), 1200], 3), 600, 700, 800, 500, 400, 300], inf)*0.5,  //scale (transpose down 1 octave)
    \dur, 0.125,
    \legato, Pseq([Pseq([0.1], 8), 0.5, 0.6, 0.7, 0.8, 0.9, 1], inf)*2,  //scale (double duration)
)).play;
)

//back to simple
(
Pdef(\pat1, PmonoArtic(\avsynth,
    \freq, Pseq([300, 500, 600, 700], inf),
    \dur, 0.125,
    \legato, 0.1,
)).play;
)

(
Pdef(\pat1, PmonoArtic(\avsynth,
    \freq, Pstutter(2, Pseq([300, 500, 600, 700], inf)),  //stutter twice
    \dur, 0.125,
    \legato, 0.1,
)).play;
)

(
Pdef(\pat1, PmonoArtic(\avsynth,
    \freq, Pstutter(2, Pseq([300, 500, 600, 700, Pstutter(4, Pseq([800], 1))], inf)),  //nested stutter
    \dur, 0.125,
    \legato, 0.1,
)).play;
)

(
Pdef(\pat1, PmonoArtic(\avsynth,
    \freq, Pstutter(2, Pseq([300, 500, 600, 700, Pstutter(4, Pseq([800], 1))], inf)),
    \dur, 0.125,
    \legato, 0.1,
    \atk, 0,  //change attack
    \rel, 0.3,  //change release
)).play;
)

(
Pdef(\pat1, PmonoArtic(\avsynth,
    \freq, Pstutter(2, Pseq([300, 500, 600, 700, Pstutter(4, Pseq([800], 1))], inf)),
    \detune, Pseq([0, 0, -10, 100, 0, 0, 0], inf),  //also added detune
    \dur, 0.125,
    \legato, 0.1,
    \atk, Pseq([0, 0, 0.1], inf),  //can also be pattern
    \rel, Pstutter(8, Pseq([0.5, 0.1], inf)),  //can also be pattern
)).play;
)

//now a second sequencer. note the new name pat2
(
Pdef(\pat2, PmonoArtic(\avsynth,
    \freq, Pseq([100, 200, 300, 300], inf),
    \dur, 0.125,
    \legato, 0.1,
    \atk, 0.01,
    \rel, 0.5,
)).play;
)

(
Pdef(\pat2, PmonoArtic(\avsynth,
    \freq, Pseq([100, 200, 300, 300], inf),
    \dur, 0.125,
    \legato, 0.1,
    \atk, 0.01,
    \rel, 0.5,
    \pan, Pseq([-0.5, 0, 0.5], inf),  //panning left-right
)).play;
)

(
Pdef(\pat2, PmonoArtic(\avsynth,
    \freq, Pseq([100, 200, 300, 300], inf)*2,  //transpose up one octave
    \dur, 0.125,
    \legato, 0.1,
    \atk, 0.01,
    \rel, 0.5,
    \pan, Pseq([-0.5, 0, 0.5], inf),
)).play;
)

(
Pdef(\pat2, PmonoArtic(\avsynth,
    \freq, Pseq([100, 200, 300, 300], inf)*Pstutter(8, Pseq([1, 1, 1, 1.25, 0.75, 0.75, 1.5, 0.5], inf)),  //scaling can also be a pattern
    \dur, 0.125,
    \legato, 0.1,
    \atk, 0.01,
    \rel, 0.5,
    \pan, Pseq([-0.5, 0, 0.5], inf),
)).play;
)

//a third sequencer (kickdrum)
(
Pdef(\pat3, PmonoArtic(\avsynth,
    \freq, Pseq([50, 60], inf),
    \dur, 0.25,
    \legato, 0.01,
    \atk, 0,
    \rel, 0.1,
    \filtAmt, 100,
    \filtQ, 1,
    \amp, Pseq([0.4, 0.1], inf),
)).play;
)

(
Pdef(\pat3, PmonoArtic(\avsynth,
    \freq, Pseq([50, 60], inf),
    \dur, 0.25,
    \legato, 0.01,
    \atk, 0,
    \rel, 0.1,
    \filtAmt, Pseq([Pseq([1], 17), 2, 3, 4, 5, 6, 7, 8, 9, 8, 7, 6, 5, 4, 3, 2]*100, inf),
    \filtQ, 1,
    \amp, Pseq([0.4, 0.1], inf),
)).play;
)

Pdef(\pat1).stop
Pdef(\pat2).stop
Pdef(\pat3).stop
Pdef(\pat3).play
Pdef(\pat2).play
Pdef(\pat1).play

//back to editing pat1
(
Pdef(\pat1, PmonoArtic(\avsynth,
    \freq, Pstutter(2, Pseq([300, 500, 600, 700, Pstutter(4, Pseq([800], 1))], inf)),
    \detune, Pseq([0, 0, -10, 100, 0, 0, 0], inf),
    \dur, 0.125,
    \legato, Pseq([1, 0.1, 0.1], inf),  //added pattern for legato
    \atk, Pseq([0, 0, 0.1], inf),
    \rel, Pstutter(8, Pseq([0.5, 0.1], inf)),
)).play;
)

(
Pdef(\pat1, PmonoArtic(\avsynth,
    \freq, Pstutter(2, Pseq([300, 500, 600, 700, Pstutter(4, Pseq([800], 1))], inf)),
    \detune, Pseq([0, 0, -10, 100, 0, 0, 0], inf),
    \dur, 0.125,
    \legato, Pseq([1, 0.1, 0.1], inf),
    \atk, Pseq([0, 0, 0.1], inf),
    \rel, Pstutter(8, Pseq([0.5, 0.1], inf)),
    \mod, 10,  //added modulator scaling
    \filtAmt, Pseq([Pseq([200], 6), 1000], inf),  //added filter parameter
)).play;
)

(
Pdef(\pat1, PmonoArtic(\avsynth,
    \freq, Pstutter(2, Pseq([300, 500, 600, 700, Pstutter(4, Pseq([800], 1))], inf)),
    \detune, Pseq([0, 0, -10, 100, 0, 0, 0], inf),
    \dur, 0.125,
    \cur, 4,  //curvature for envelope
    \legato, Pseq([1, 0.1, 0.1], inf),
    \atk, Pseq([0, 0, 0.1], inf),
    \rel, Pstutter(8, Pseq([0.5, 0.1], inf)),
    \mod, 10,  //added modulator scaling
    \filtAmt, Pseq([Pseq([200], 6), 1000], inf),  //added filter parameter
)).play;
)

//--effects
a.free; a= Synth(\avdist, [\in, 50, \dist, 40]);  //distorion on bus 50
b.free; b= Synth(\avverb, [\in, 60, \room, 10]);  //reverb on bus 60

(
Pdef(\pat2, PmonoArtic(\avsynth,
    \freq, Pseq([100, 200, 300, 300], inf)*Pstutter(8, Pseq([1, 1, 1, 1.25, 0.75, 0.75, 1.5, 0.5], inf)),  //scaling can also be a pattern
    \dur, 0.125,
    \legato, 0.1,
    \atk, 0.01,
    \rel, 0.5,
    \pan, Pseq([-0.5, 0, 0.5], inf),
    \out, 50,  //distort everything by sending to bus 50
)).play;
)

(
Pdef(\pat1, PmonoArtic(\avsynth,
    \freq, Pstutter(2, Pseq([300, 500, 600, 700, Pstutter(4, Pseq([800], 1))], inf)),
    \detune, Pseq([0, 0, -10, 100, 0, 0, 0], inf),
    \dur, 0.125,
    \cur, 4,  //curvature for envelope
    \legato, Pseq([1, 0.1, 0.1], inf),
    \atk, Pseq([0, 0, 0.1], inf),
    \rel, Pstutter(8, Pseq([0.5, 0.1], inf)),
    \mod, 10,  //added modulator scaling
    \filtAmt, Pseq([Pseq([200], 6), 1000], inf),
    \out, Pseq([60, 0, 0], inf),  //reverb on every third note
)).play;
)

(
Pdef(\pat3, PmonoArtic(\avsynth,
    \freq, Pseq([50, 200, 50, 2000], inf),  //change frequencies
    \dur, 0.125,
    \legato, 0.01,
    \atk, 0,
    \rel, 0.1,
    \filtAmt, Pseq([Pseq([1], 17), 2, 3, 4, 5, 6, 7, 8, 9, 8, 7, 6, 5, 4, 3, 2]*100, inf),
    \filtQ, 0.05,
    \mod, 10,  //scale up modulation
    \amp, Pseq([0.4, 0.1], inf),
    \out, 50,  //distort
)).play;
)

//set effect parameters
a.set(\dist, 80)  //more distortion
a.set(\dist, 2)  //less distortion
b.free  //turn off reverb
a.free  //free distortion

Pdef(\pat1).stop
Pdef(\pat2).stop
Pdef(\pat3).stop

//--something different
(
b.free; b= Synth(\avverb, [\in, 60, \room, 15]);
a.free; a= Synth(\avdist, [\in, 50, \dist, 30]);
Pdef(\pat1, PmonoArtic(\avsynth,
    \freq, Pseq([Pseq([300, 400, 200], 2), Pseq([600, 700], 2), 700], inf),
    \dur, Pseq([Pseq([0.125], 2), Pseq([0.125/4], 16)], inf),
    \atk, Pstutter(8, Pseq([0, 0, 0, 0.1, 0.1, 0.1, 0.2, 0.3, 1], inf)),
    \rel, 0.3,
    \legato, Pseq([0.1, 1, 1, 0.5, 1], inf),
    \out, Pseq([50, 0, 60, 50, 50], inf),
    \pan, Pseq([0.5, 0, -0.5], inf),
)).play;
)

//another thing
(
b.free; b= Synth(\avverb, [\in, 60, \room, 1]);
Pdef(\pat1, PmonoArtic(\avsynth,
    \freq, Pseq([400, 600, 700], inf),
    \dur, 0.25,
    \out, 60,
)).play;
Pdef(\pat2, PmonoArtic(\avsynth,
    \freq, Pseq([400, 600, 700, \rest], inf)*0.5,  //rests can be used in freq pattern
    \dur, 0.75,
    \legato, 0.25,
    \out, 60,
)).play;
)

//another
(
b.free; b= Synth(\avverb, [\in, 60, \room, 1]);
Pdef(\pat1, PmonoArtic(\avsynth,
    \freq, Pseq([1, 2, 3], inf)*400,
    \dur, 0.1,
    \amp, Pstutter(2, Pseq([0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 8, 7, 6, 5, 4, 3, 2, 1, Pseq([0], 16)], inf)*0.01),  //fade in/out
    \legato, 0.5,
    \rel, 3,
    \out, 60,
)).play;
Pdef(\pat2, PmonoArtic(\avsynth,
    \freq, Pstutter(Pseq([1, 2, 3, 4, 3, 2], inf), Pseq([400, 600, 700, 200], inf))*0.75*Pseq([0.5, 0.75, 1, 1, 1, 2], inf),
    \dur, 0.125/3*4,
    \atk, 0.001,
    \rel, 3*Pseq([1, 2, 3, 2, 1, 0.75, 0.5, 0.5, 0.5], inf),
    \mod, Pseq([0, 1, 0, 0, 10], inf),
    \pan, Pseq([0.5, -0.5], inf),
    \filtAmt, Pstutter(5, Pseq([200, 400, 500, 600, 700, 2000], inf))*2,
    \filtQ, Pstutter(5, Pseq([0.1, 0.05, 0.01], inf)),
    \filtLag, Pseq([0.1, 0, 0, 0], inf),
    \legato, Pseq([0.1, 0.2, 0.5, 2]*0.1, inf),
    \out, 0,
)).play;
)

//yet different - oneliners
(
b.free; b= Synth(\avverb, [\in, 60, \room, 1]);
a.free; a= Synth(\avdist, [\in, 50, \dist, 20]);
Pdef(\pat1, PmonoArtic(\avsynth, \freq, Pseq([400, 600, 700], inf), \legato, Pseq([0.5, 1.5], inf), \dur, 0.1)).play;
Pdef(\pat2, PmonoArtic(\avsynth, \freq, Pseq([Pseq([300, 400, 200], 2), 600, 700], inf), \legato, Pseq([0.5, 1.5], inf), \dur, 0.1)).play;
Pdef(\pat3, PmonoArtic(\avsynth, \freq, Pseq([Pseq([160], 16), Pseq([120], 32)], inf), \legato, 0.2, \dur, 0.1)).play;
)

(
Pdef(\pat1, PmonoArtic(\avsynth, \freq, Pseq([1, 2, 3, 4]*800, inf), \legato, Pseq([0.5, 0.9, 1.5], inf), \dur, 0.1, \amp, Pseq([0, 0, 1, 1, 1]*0.1, inf))).play;
Pdef(\pat2, PmonoArtic(\avsynth, \freq, Pseq([Pseq([300, 400, 200], 2), 600, 700], inf), \legato, Pseq([0.5, 1.5], inf), \dur, 0.1)).play;
Pdef(\pat3, PmonoArtic(\avsynth, \freq, Pseq([Pseq([160], 16), Pseq([120], 32)], inf), \legato, 0.2, \dur, 0.1, \out, Pseq([0, 0, 60], inf))).play;
)

(
Pdef(\pat3, PmonoArtic(\avsynth, \freq, Pseq([Pseq([160, 360, 400], 4), Pseq([120, Pseq([50, 60, 70]*5, 4)], 3)], inf), \legato, Pseq([0.2, 0.4, 0.1, 2], inf), \dur, 0.1, \out, Pseq([50, 0, 0, 0, 0, 60], inf), \pan, Pseq([-0.7, 0.7, 0], inf))).play;
)

//--things to explore
TempoClock.tempo= 0.9;
TempoClock.tempo= 0.8;
TempoClock.tempo= 0.7;

PdefAllGui();  //simple gui

Pdef(\pat1).fadeTime= 8;  //set crossfade time
Pdef(\pat2).fadeTime= 8;
Pdef(\pat3).fadeTime= 8;

Pdef(\pat1).quant= 4;  //start next 4/4 bar
Pdef(\pat2).quant= 4;
Pdef(\pat3).quant= 4;
Pdef(\pat3).stop;
Pdef(\pat2).stop;
Pdef(\pat1).stop;
Pdef(\pat3).play;
Pdef(\pat2).play;
Pdef(\pat1).play;

//etc. lots more to learn about pdef and patterns (Prand, Pwhite, Pseg)
```

links
==

inspiration http://thedotisblack.tumblr.com

Terry Riley - [A Rainbow in Curved Air](https://www.youtube.com/watch?v=5PNbEfLIEDs)
