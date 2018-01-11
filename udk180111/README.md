deeper into the code
--------------------

this time we repeat what we have learned and at the same time dive deeper into some syntax details and concepts we skipped over.

supercollider
==

warm up / repetition...

```supercollider
s.boot;
s.meter;
s.scope;

//--load sounds and effects (from 19oct)
(
SynthDef(\avsynth, {|out= 0, freq= 400, atk= 0.01, rel= 0.1, cur= -4, amp= 0.1, gate= 1, pan= 0, mod= 1, filtAmt= 200, filtQ= 0.1, filtLag= 0|
    var env= EnvGen.ar(Env.asr(atk, 1, rel, cur), gate, doneAction:2);
    var snd= VarSaw.ar(freq, 0, LFDNoise3.ar(env*mod, 0.5, 0.5));
    filtAmt= filtAmt.clip(4, 4000);
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
    \freq, Pseq([Pseq([1300, 300], 2), 400, 600, 700], inf),
    \dur, 0.125,
    \cur, Pseq([0, 0, 0, 0, -12, 0, 0, 12], inf),
    \legato, 0.2,
)).play;
)

(
Pdef(\pat2, PmonoArtic(\avsynth,
    \freq, Pseq([Pseq([1300, 300], 2), 400, 600, 700, \rest, \rest], inf)*0.5,
    \dur, 0.125,
    \cur, Pseq([0, 0, 0, 0, -12, 0, 0, 12], inf),
    \legato, 0.2,
    \pan, Pseq([-0.5, 0.5], inf),
)).play;
)

//--effects
a.free; a= Synth(\avdist, [\in, 50, \dist, 40]);  //distorion on bus 50
b.free; b= Synth(\avverb, [\in, 60, \out, 50, \room, 10]);  //reverb on bus 60

Pdef(\pat1).set(\out, 60);
Pdef(\pat2).set(\out, 0);

(
Pdef(\pat3, PmonoArtic(\avsynth,
    \freq, Pseq([50, 75, 100, 50, 60, 75, 75], inf),
    \dur, 4,
    \amp, 0.2,
    \legato, 1,
)).play;
)

a.set(\dist, 80);
a.set(\dist, 40);
b.set(\drylevel, 1);
b.set(\drylevel, 0.1);
Pdef(\pat3).set(\out, 60);
a.set(\dist, 90);
Pdef(\pat3).set(\mod, 5);

Pdef(\pat2).stop;
Pdef(\pat1).stop;
Pdef(\pat3).set(\mod, 50);
Pdef(\pat3).set(\mod, 500);
Pdef(\pat3).stop;

a.free; b.free;
```

things to explore...

* midicps, midinote, degree, octave, detune, scale, tuning (`\scale, Scale.minor(Tuning.just)`)
* busses, effects, node order
* legato, atk, rel, cur
* etc

- - -

unity3d
==

* start unity and create a new 3D (or 2D) project. give it a name (here 'trails').
* select the Main Camera and set 'Clear Flags' to 'Solid Color'
* select a background
* select GameObject / Create Empty
* create a new script by selecting Assets / Create / C# Script
* give the script the name 'Trail' by typing under the white icon
* double click the white C# script icon to open it in MonoDevelop
* copy and paste in the code here below replacing what was there originally

```cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trail : MonoBehaviour {
    TrailRenderer trail;
    void Start () {
        Application.runInBackground = true;
        trail = gameObject.AddComponent<TrailRenderer>();
        trail.material = new Material (Shader.Find("Particles/Additive"));
    }
    void Update () {
        trail.time = 10.0F;
        trail.startColor = Color.white;
        trail.endColor = Color.black;
        trail.startWidth = 0.5F;
        trail.endWidth = 0.0F;
        float x = Mathf.Sin (Time.time * 3.0F) * 5.5F;
        float y = Mathf.Cos (Time.time * 5.0F) * 4.4F;
        float z = 0.0F;
        transform.localPosition= new Vector3 (Mathf.Round(x), y, z);
    }
}

```

* switch back to unity and press play. you should see something like this...

![00template](00template.png?raw=true "00template")

now edit the script while the scene is running and try changing the code in the update method. (you only need to switch back and forth between monodevelop and unity for the script to reload)

things to explore...

* scale time to draw faster, scale x, y to draw wider
* add a sine function for z `float z = Mathf.Sin (Time.time * 7.0F) * 5.5F;`
* try to round some other numbers
* modulate start color and trail time
* add the script to a new (visible) game object like a sphere
* etc

look-up table
--

instead of sine functions one can use an array of floats. here is a variation of the above. replace the update method with this...

```cs
void Update () {
    trail.time = 0.3F;
    trail.startColor = Color.white;
    trail.endColor = Color.black;
    trail.startWidth = 0.5F;
    trail.endWidth = 0.0F;
    float[] data = new float[] {0.1f, 0.2f, 0.3f, 0.2f, 0.1f, -0.2f, -1.3f, -0.1f};    //add your own floats here
    float x = Time.time*4.0F%20.0F-10.0f;   //left to right scanning - can also be a lookup, sine or whatever
    float y= data[(int)Mathf.Round(Time.frameCount*0.2F) % data.Length]*10.0F;
    float z= data[(int)Mathf.Round(Time.frameCount*0.1F) % data.Length]*10.0F+10.0F;
    transform.localPosition= new Vector3 (x, y, z);
}
```

first person character
--

* select GameObject / 3D Object / Plane
* select Assets / Import Package / Characters
* click 'import' in the window that pops up to import all
* find the prefab FPSController under Assets / Standard Assets / Characters / FirstPersonCharacter / Prefabs

![01prefab](01prefab.png?raw=true "01prefab")

* drag&drop the FPSController onto the hierarchy window
* disable the Main Camera
* press play and you should be able to walk around inside your trails using the arrow keys + mouse
* optionally edit the settings for the camera inside the FirstPersonCharacter to use the same settings (Solid Color) as your main camera had in the example above

![02fps](02fps.png?raw=true "02fps")
