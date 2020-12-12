more samples and trails
--------------------

supercollider
==

we need to find some soundfiles to work with.

download this one... https://raw.githubusercontent.com/redFrik/udk18-Discrete_Structures/master/udk171207/ljudfil.aiff (on osx you can alt+click the link to download the file directly)

you can also render your own. on **mac osx** open Terminal and type...

`say hallo`

and if that works write it to a 16bit, 44100 aiff soundfile like this...

`say -o ~/Desktop/hallo.aiff --data-format=BEI16@44100 hallo`

the file should end up on your desktop.

you can also download a wav or aiff from the internet or record your own using for example [Audacity](https://www.audacityteam.org). vocal recordings or speech should work well.

NOTE: can not be `.mp3`. in supercollider it is recommended to use `.aiff` or `.wav`

```supercollider

s.boot;
s.meter;
s.scope;

(
~samp.free;
~samp= Buffer.readChannel(s, "/Users/asdf/Desktop/ljudfil.aiff", channels:[0]);  //edit this line with the path to your own soundfile
)

//--load a sampler (a synth definition for playing buffers)
(
SynthDef(\avsamp, {|out= 0, buf, rate= 1, offset= 0, atk= 0.005, rel= 0.01, cur= -4, amp= 0.1, pan= 0, gate= 1, loop= 1|
    var env= EnvGen.ar(Env.asr(atk, 1, rel, cur), gate, doneAction:2);
    var src= PlayBuf.ar(1, buf, rate*BufRateScale.ir(buf), 1, offset*BufFrames.ir(buf), loop);
    OffsetOut.ar(out, Pan2.ar(src*env*amp, pan));
}).add;
)

TempoClock.tempo= 1;  //let us start with the default tempo (1bps = 60bpm) - but you can change it at any time

(
Pdef(\samp, PmonoArtic(\avsamp,
    \buf, ~samp,
    \dur, ~samp.duration,
    \amp, 0.75,
)).play;
)

(
Pdef(\samp, PmonoArtic(\avsamp,
    \buf, ~samp,
    \dur, ~samp.duration*0.5,  //trigger a new after 50% of the first one
    \rel, 2,  //set a longer duration
    \amp, 0.75,
)).play;
)

(
Pdef(\samp, PmonoArtic(\avsamp,
    \buf, ~samp,
    \dur, ~samp.duration*0.25,  //trigger at 25%, 50% and 75% as well as at 0
    \rel, 2,
    \amp, 0.75,
)).play;
)

(
Pdef(\samp, PmonoArtic(\avsamp,
    \buf, ~samp,
    \dur, ~samp.duration*0.125,  //1/8
    \rel, 2,
    \amp, 0.75,
)).play;
)

(
Pdef(\samp, PmonoArtic(\avsamp,
    \buf, ~samp,
    \dur, ~samp.duration*0.125+Pstutter(10, Pseq([0.05, 0, -0.05], inf)),  //adding small variations every 10th time
    \rel, 2,
    \amp, 0.75,
)).play;
)

(
Pdef(\samp, PmonoArtic(\avsamp,
    \buf, ~samp,
    \dur, ~samp.duration*0.125+Pstutter(10, Pseq([0.05, 0, -0.05], inf)),
    \rel, 6,  //same but with much longer release time
    \amp, 0.75,
)).play;
)

//back to simple
(
Pdef(\samp, PmonoArtic(\avsamp,
    \buf, ~samp,
    \dur, ~samp.duration*0.5,  //two
    \rel, 2,
    \amp, 0.75,
)).play;
)

(
Pdef(\samp, PmonoArtic(\avsamp,
    \buf, ~samp,
    \dur, ~samp.duration*0.5,
    \offset, 0.5,  //now add an offset to both (starting position will be at 50%)
    \rel, 2,
    \amp, 0.75,
)).play;
)

(
Pdef(\samp, PmonoArtic(\avsamp,
    \buf, ~samp,
    \dur, ~samp.duration*0.5,
    \offset, Pseq([0, 0.5], inf),  //note: now in sync - try nudging the 0.5 a little bit up/down
    \rel, 2,
    \amp, 0.75,
)).play;
)

(
Pdef(\samp, PmonoArtic(\avsamp,
    \buf, ~samp,
    \dur, ~samp.duration*0.25,  //now at 1/4
    \offset, Pseq([0, 0.25, 0.5, 0.75], inf),  //again in sync - try nudging or reordering
    \rel, 2,
    \amp, 0.75,
)).play;
)

(
Pdef(\samp, PmonoArtic(\avsamp,
    \buf, ~samp,
    \dur, ~samp.duration*0.125,  //now at 1/8
    \offset, Pseq([0, 0.25, 0.5, 0.75], inf),
    \rel, 2,
    \amp, 0.75,
)).play;
)

(
Pdef(\samp, PmonoArtic(\avsamp,
    \buf, ~samp,
    \dur, ~samp.duration*0.125,
    \offset, Pseq([0, 0.25, 0.5, 0.75], inf)+Pstutter(10, Pseq([0.05, 0, -0.05], inf)),  //plus small variation
    \rel, 2,
    \amp, 0.75,
)).play;
)

(
Pdef(\samp, PmonoArtic(\avsamp,
    \buf, ~samp,
    \dur, ~samp.duration*0.25,  //slower
    \offset, Pseq([0, 1/3, 2/3], inf),  //different offset positions
    \rate, Pstutter(4, Pseq([0.8, 0.9, 1, 1.1, 1.2], inf)),  //pattern for the playback rate
    \rel, 2,
    \amp, 0.75,
)).play;
)

(
Pdef(\samp, PmonoArtic(\avsamp,
    \buf, ~samp,
    \dur, ~samp.duration*0.125,
    \rate, Pseq([0.8, 0.9, 1, 1.1, 1.2], inf),
    \rel, 15,  //very long release
    \amp, 0.75*Pseq([1, 0, 0, 1, 0, 1, 0], inf),  //pattern for amplitudes - 4 out of 7 are muted
)).play;
)

(
Pdef(\samp, PmonoArtic(\avsamp,
    \buf, ~samp,
    \dur, ~samp.duration*0.125,
    \rate, Pseq([0.8, 0.9, 1, 1.1, 1.2], inf)*Pseq([-1, 1], inf),  //every second backwards
    \rel, 15,
    \amp, 0.75*Pseq([1, 0, 0, 1, 0, 1, 0], inf),
)).play;
)

(
Pdef(\samp, PmonoArtic(\avsamp,
    \buf, ~samp,
    \dur, ~samp.duration*0.125,
    \rate, Pseq([0.8, 0.9, 1, 1.1, 1.2], inf)*Pseq([-1, 1], inf)*Pseq([1, 1, 1, 1.5], inf),  //every fourth higher
    \rel, 15,
    \amp, 0.75*Pseq([1, 0, 0, 1, 0, 1, 0], inf),
)).play;
)

(
Pdef(\samp, PmonoArtic(\avsamp,
    \buf, ~samp,
    \dur, ~samp.duration*0.125,
    \rate, Pseq([0.8, 0.9, 1, 1.1, 1.2], inf)*Pseq([-1, 1], inf)*Pseq([1, 1, 1, 1.5], inf),
    \rel, 15,
    \amp, 0.75*Pseq([1, 0, 0, 1, 0, 1, 0], inf),
    \pan, Pseq([0.9, 0.5, 0, -0.5, -0.9, -0.5, 0, 0.5], inf),  //adding panning
)).play;
)

Pdef(\samp).stop;

//that was using a single sample with a single sequencer

//--many independent sequencers

(
Pdef(\short0).quant= 0;
Pdef(\short0, PmonoArtic(\avsamp,
    \buf, ~samp,
    \dur, 2,
    \legato, 0.08,
    \offset, 0.45,  //find a good spot 0.0-1.0 - depends on your sample - also adjust offset accordingly in the sequencers below
    \amp, 0.75,
    \pan, Pseq([-0.5, 0, 0.5], inf),
)).play;

Pdef(\short1).quant= 0;
Pdef(\short1, PmonoArtic(\avsamp,  //a second sequencer
    \buf, ~samp,
    \dur, 2.01,  //slightly slower
    \legato, 0.10,  //slightly longer
    \offset, 0.44,  //slightly earlier
    \amp, 0.75,
    \pan, Pseq([-0.5, 0, 0.5], inf),
)).play;

Pdef(\short2).quant= 0;
Pdef(\short2, PmonoArtic(\avsamp,  //a third sequencer
    \buf, ~samp,
    \dur, 2.02,  //again a bit slower
    \legato, 0.12,  //and longer
    \offset, 0.43,  //and earlier
    \amp, 0.75,
    \pan, Pseq([-0.5, 0, 0.5], inf),
)).play;

Pdef(\short3).quant= 0;
Pdef(\short3, PmonoArtic(\avsamp,  //a fourth sequencer
    \buf, ~samp,
    \dur, 2.03,  //even slower
    \legato, 0.14,  //longer
    \offset, 0.42,  //and earlier
    \amp, 0.75,
    \pan, Pseq([-0.5, 0, 0.5], inf),
)).play;
)

(
Pdef(\short0).stop;
Pdef(\short1).stop;
Pdef(\short2).stop;
Pdef(\short3).stop;
)

//to make many more, let us write this as an algorithm instead

//this should be exactly the same as above...
(
4.do{|i|
    var name= (\short++i).asSymbol;
    Pdef(name).quant= 0;
    Pdef(name, PmonoArtic(\avsamp,
        \buf, ~samp,
        \dur, 2+(i*0.01),
        \legato, 0.08+(i*0.02),
        \offset, 0.45-(i*0.01),
        \amp, 0.75,
        \pan, Pseq([-0.5, 0, 0.5], inf),
    )).play;
};
)

(
12.do{|i|  //many more (12 in total)
    var name= (\short++i).asSymbol;
    Pdef(name).quant= 0;
    Pdef(name, PmonoArtic(\avsamp,
        \buf, ~samp,
        \dur, 2+(i*0.01),
        \legato, 0.08+(i*0.02),
        \offset, 0.45-(i*0.01),
        \amp, 0.65,
        \pan, Pseq([-0.5, 0, 0.5], inf),
    )).play;
};
)

(
30.do{|i|  //even more
    var name= (\short++i).asSymbol;
    Pdef(name).quant= 0;
    Pdef(name, PmonoArtic(\avsamp,
        \buf, ~samp,
        \dur, 2+(i*0.01),
        \legato, 0.08+(i*0.02),
        \offset, 0.45-(i*0.01),
        \amp, 0.55,  //turning down the amplitude for each
        \pan, Pseq([-0.5, 0, 0.5], inf),
    )).play;
};
)

(
30.do{|i|
    var name= (\short++i).asSymbol;
    Pdef(name).quant= 0;
    Pdef(name, PmonoArtic(\avsamp,
        \buf, ~samp,
        \dur, 2+(i*0.01)*3,  //scale all durations *3
        \legato, 0.08+(i*0.02),
        \offset, 0.45-(i*0.01),
        \rate, i.linlin(0, 29, 0.95, 1.05),  //set a unique playbackrate for each
        \amp, 0.45,
        \pan, Pseq([-0.5, 0, 0.5], inf),
    )).play;
};
)

(
30.do{|i|
    var name= (\short++i).asSymbol;
    Pdef(name).quant= 0;
    Pdef(name, PmonoArtic(\avsamp,
        \buf, ~samp,
        \dur, 2+(i*0.01)*3,
        \legato, 0.08+(i*0.02),
        \offset, 0.45-(i*0.01),
        \rate, i%2+1,  //every other one octave up - could be written simpler with Pseq([1, 2], inf)
        \amp, 0.45,
        \pan, Pseq([-0.5, 0, 0.5], inf),
    )).play;
};
)

(
30.do{|i|
    var name= (\short++i).asSymbol;
    Pdef(name).quant= 0;
    Pdef(name, PmonoArtic(\avsamp,
        \buf, ~samp,
        \dur, 2+(i*0.01)*3,
        \legato, 0.08+(i*0.02),
        \offset, 0.45-(i*0.01),
        \rate, i%2+1*Pseq([1, 1, -1], inf),  //every other one octave up and every third backwards - can also be written Pseq([1, 2, -1, 2, 1, -2], inf)
        \amp, 0.45,
        \pan, Pseq([-0.5, 0, 0.5], inf),
    )).play;
};
)

//stop this madness!
(
30.do{|i|
    var name= (\short++i).asSymbol;
    Pdef(name).stop;
};
)

//now try loading different soundfiles (at the top of this page)

//or try recording from internal microphone like last week...
(
SynthDef(\avrec, {|buf|
    RecordBuf.ar(Limiter.ar(SoundIn.ar), buf, loop:0, doneAction:2);
}).add;
)

Synth(\avrec, [\buf, ~samp]);  //record into ~samp buffer (only overwrite what is in memory - not on disk)


//--

//to control the over all volume one can add a Pdefn to all \amp keys.  example:

(
30.do{|i|
    var name= (\short++i).asSymbol;
    Pdef(name).quant= 0;
    Pdef(name, PmonoArtic(\avsamp,
        \buf, ~samp,
        \dur, 2+(i*0.01)*3,
        \legato, 0.08+(i*0.02),
        \offset, 0.45-(i*0.01),
        \rate, i%2+1*Pseq([1, 1, -1], inf),
        \amp, 0.45*Pdefn(\vol, 1),  //note the Pdefn here
        \pan, Pseq([-0.5, 0, 0.5], inf),
    )).play;
};
)

//and then slowly fade in and out like this...
Pdefn(\vol, 0.1);
Pdefn(\vol, 0.5);
Pdefn(\vol, 0.1);
Pdefn(\vol, 0);

//and for a simple gui slider control run this line...
Slider(bounds:Rect(10, 10, 100, 200)).front.action= {|v| Pdefn(\vol, v.value.postln)};

//stop all with
(
30.do{|i|
    var name= (\short++i).asSymbol;
    Pdef(name).stop;
};
)
//or
Pdef.all.do{|x| x.stop}

//or immediately with
CmdPeriod.run;

```

volume
--

another way to control the global volume in supercollider is to use the built-in Volume class.

```supercollider
s.volume= 0;  //in decibels - full volume
s.volume= -12;  //a lot quieter - minus 12dB
s.volume= -inf;  //silence
s.volume= 1.ampdb;  //in 'amp' - full volume converted to dB
s.volume= 0.251.ampdb;  //approximately same as -12dB
s.volume= 0.ampdb;  //silence

s.volume= 0;  //reset the volume
s.volume.gui;  //open a gui window

//fade out
(
var start= 1, end= 0;  //fade from start to end
var time= 3;  //over how many seconds
var resolution= 100;  //in how many steps per second
Routine({
    (time*resolution).do{|i|
        s.volume= i.lincurve(0, time*resolution-1, start, end).ampdb.postln;
        (1/resolution).wait;
    };
    "done".postln;
}).play(AppClock);
)

//and do not forget to reset the volume back to full afterwards
s.volume= 0;
```

- - -

unity3d
==

trails in unity are very similar to the lines we played with [last week](https://github.com/redFrik/udk18-Discrete_Structures/tree/master/udk171130#unity3d).

NOTE: again this will require unity version 5.5 or newer. and if you try it in 2D remember to add a light.

* start unity and create a new 3D project. give it a name (here 'trails').
* select Assets / Create / Material
* give the material a name (can be anything - here 'TrailMat') by typing under the icon
* at the top of the inspector for the material select Shader / Unlit / Texture
* also click on the Base Texture icon and select 'Default-Particle'
* select GameObject / Create Empty
* select Component / Effects / Trail Renderer
* in the inspector find Materials / Element 0 and set it to your TrailMat
* also set Time right below to something less than the default 5 (here 2 seconds)

![00time](00time.png?raw=true "00time")

* press play and try changing the x, y, z positions in the GameObject's Transform. you should see a trail following along
* also, while running, try switching to the scene view and click and drag the game object around in there

![01drag](01drag.png?raw=true "01drag")

* also try setting `Min Vertex Distance` to something high like 8 and observe what happens

so trails basically follow any object but are erased after a fixed time. now move the game object around automatically with a script...

* create a new script by selecting Assets / Create / C# Script
* give the script the name 'Trails' by typing under the white icon
* double click the white C# script icon to open it in MonoDevelop
* copy and paste in the code here below replacing what was there originally

```cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trails : MonoBehaviour {
    public float xspeed= 0.2F;
    public float yspeed= 0.3F;
    public float zspeed= 1.0F;
    public float scale = 5.0F;
    void Start () {
    }
    void Update () {
        transform.localPosition = new Vector3 (Mathf.Sin (Time.frameCount * xspeed) * scale, Mathf.Cos (Time.frameCount * yspeed) * scale, 0);
        //transform.localPosition= new Vector3 (Mathf.Sin(Time.frameCount*xspeed)*scale, Mathf.Cos(Time.frameCount*yspeed)*scale, Mathf.Sin(Time.frameCount*zspeed)*scale);
        //transform.localPosition= new Vector3 (((Time.frameCount+1)/2%2*2-1)*scale+(Mathf.Sin(Time.frameCount*xspeed*xspeed)*scale), (Time.frameCount/2%2*2-1)*scale, Time.frameCount*(zspeed*0.025F)%scale);
        //transform.localPosition= new Vector3 ((Time.frameCount/2%2*Mathf.Sin(Time.frameCount*xspeed))*scale, (Time.frameCount/3%2*Mathf.Sin(Time.frameCount*yspeed))*scale, (Time.frameCount/4%2*Mathf.Sin(Time.frameCount*zspeed))*scale);
    }
}
```

* attach the script to the GameObject by drag and drop.
* play around with the script variables - specially try setting `xspeed`, `yspeed` to 1, 1.25, 0.5, 0.33333 etc and compare it with slightly _nudged_ values like 1.251, 0.49, 0.32 etc
* don't forget to change the `Min Vertex Distance`, `Time` and other parameters
* also try the different variants in the script (by uncommenting and commenting out lines)
* drag a photo from your computer into the Assets, then onto the Base Texture icon in Material inspector
* try some different shaders

![02script](02script.png?raw=true "02script")

multiple
--

now we will make a new project with many objects and many trails. this time trying to do most of it in the script.

* start unity and create a new 3D project. give it a name (here 'trails2').
* create a new script by selecting Assets / Create / C# Script
* give the script the name 'Trails2' by typing under the white icon
* double click the white C# script icon to open it in MonoDevelop
* copy and paste in the code here below replacing what was there originally

```cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trails2 : MonoBehaviour {
    public Material mat;
    public float spread= 0.01F;
    public float scale = 5.0F;
    int num= 50;  //number of objects
    List<GameObject> list= new List<GameObject>();
    void Start () {
        mat = new Material (Shader.Find ("Particles/Additive"));  //create a material - comment out to add your own
        for (int i = 0; i < num; i++) {
            GameObject go= new GameObject();  //create a new empty object
            go.AddComponent<TrailRenderer>();  //add a trail renderer component
            TrailRenderer tr = go.GetComponent<TrailRenderer> ();
            tr.material = mat;  //set the material to the trail renderer
            tr.time = 0.2F; //default length
            tr.startWidth = 0.1F;  //default width
            list.Add(go);  //add the object to the list of objects
        }
    }
    void Update () {
        float i = 0;
        foreach (GameObject go in list) {
            i++;
            float theta = (Time.frameCount * spread) + ((i / num)*Mathf.PI*2);
            TrailRenderer tr = go.GetComponent<TrailRenderer> ();

            //variant 1 - circle
            go.transform.localPosition = new Vector3 (
                Mathf.Sin(theta)*scale,
                Mathf.Cos(theta)*scale,
                Mathf.Sin(theta)*scale
            );
            tr.time = 0.05F;

            //variant 2 - circle spiral
            //            go.transform.localPosition = new Vector3 (
            //                Mathf.Sin(theta)*scale+Mathf.Sin(theta*10),
            //                Mathf.Cos(theta)*scale+Mathf.Cos(theta*10),
            //                Mathf.Sin(theta)*scale+Mathf.Sin(theta*10)
            //            );
            //            tr.time = 0.05F;

            //variant 3 - varying width
            //            go.transform.localPosition = new Vector3 (
            //                Mathf.Sin(theta)*scale+Mathf.Sin(theta*7),
            //                Mathf.Cos(theta)*scale+Mathf.Cos(theta*10),
            //                0
            //            );
            //            tr.startWidth = Mathf.Sin(theta+(Time.frameCount*0.2F))+1.0F;
            //            tr.endWidth = Mathf.Sin(theta+(Time.frameCount*0.11F))+1.0F;
            //            tr.time = 0.1F;

            //variant 4 - wave
            //            go.transform.localPosition = new Vector3 (
            //                Mathf.Lerp(-scale*2, scale*2, i/num),
            //                Mathf.Cos(theta)*scale,
            //                Mathf.Sin(theta)*scale
            //            );
            //            tr.startWidth = 0.3F;
            //            tr.endWidth = 0.0F;
            //            tr.time = 5.0F;
            //
            
            //variant 5 - wave with mouse control
            //            go.transform.localPosition = new Vector3 (
            //                Mathf.Lerp(-scale*2, scale*2, i/num),
            //                Mathf.Cos(theta * (Input.mousePosition.x*0.02F))*scale,
            //                Mathf.Sin(theta * (Input.mousePosition.x*0.02F))*scale
            //            );
            //            tr.startWidth = 0.1F;
            //            tr.endWidth = 0.1F;
            //            tr.time = Input.mousePosition.y * 0.01F;
            
        }
    }
}

```

* attach the script to the main camera and click play

![03wave](03wave.png?raw=true "03wave")

2D
--

last let us try out trails in a **2D** project.

* restart unity and create a new 2D project. give it a name (here trails3)
* create a new script by selecting Assets / Create / C# Script
* give the script the name 'Trails3' by typing under the white icon
* double click the white C# script icon to open it in MonoDevelop
* copy and paste in the code here below replacing what was there originally

```cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trails3 : MonoBehaviour {
    public Transform prefab;    //holds our initial gameobject
    public float scale = 4.0F;
    public float spread = 0.23F;
    public float speed = 0.01F;
    List<Transform> clones = new List<Transform> ();
    int num = 30;    //set number of clones
    void Start () {  //do once when starting
        for (int i = 0; i < num; i++) {
            clones.Add (Instantiate (prefab, new Vector3 (0, 0, 0), Quaternion.identity));
        }
        prefab.gameObject.SetActive (false);  //hide prefab object
    }
    void Update () {
        float i= 0;    //keep track of clone index
        foreach (Transform ct in clones) {
            ct.localPosition= new Vector3(Mathf.Lerp (-9, 9, i / num), Mathf.Sin(Time.frameCount*speed+(i*spread))*scale, Mathf.Cos(Time.frameCount*speed+(i*spread))*scale);
            i++;
        }
    }
}
```

* save and switch back to unity
* add a light by selecting GameObject / Light / Directional Light (remember to test different positions, rotation, colours etc later )
* in the upper left hierarchy window, click to select the 'Main Camera'
* attach the script to the camera by selecting Component / Scripts / Trails3 (or just drag and drop the script onto the camera)
* select GameObject / 3D Object / Sphere (or whatever you feel like - can also be an empty game object)
* select the Main Camera and in the inspector click on the little circle next to prefab. select the Sphere by doubleclicking in the dialog that pops up (or just drag the sphere onto the prefab variable slot)
* add a Trail Renderer to the Sphere (by first clicking sphere in the hierarchy window, then selecting Component / Effects / Trail Renderer)
* set `Time` to 5, `Min Vertex Distance` to 0, `Width` to 5, `Corner Vertices` to 50, `End Cap Verticies` also to 50 and in the pop up Texture Mode select Tile

![04material](04material.png?raw=true "04material")

* select Assets / Create / Material
* give the material a name (can be anything - here 'Trail3Mat') by typing under the icon
* at the top of the inspector for the material select Shader / Particle / Additive (or some other shader also possible)
* also click on the Particle Texture icon and select 'Default-Particle' (or import your own photo and add it here)
* finally find Materials /Element 0 in the Sphere's inspector (under Trail Renderer) and set it to Trail3Mat

![05strange](05strange.png?raw=true "05strange")

play around (don't forget to change variables speed, spread, scale etc in the code, hack it and write your own variants)

links
==

Board To Bits - [Unity 101 playlist](https://www.youtube.com/watch?v=kH_piLCynto&list=PL5KbKbJ6Gf9_H4MZC1v6ETs_ifnLfNJVw)

https://freesound.org

import raw - https://github.com/redFrik/udk17-Digital_Harmony/tree/master/udk170622#audacity
