advanced examples
--------------------

first a warmup example in supercollider...

```supercollider
Quarks.install("Bjorklund");    //if you have not already
//recompile or restart sc

s.boot;
s.meter;
s.scope;

//--load a sound
(
SynthDef(\avball, {|out= 0, freq= 500, atk= 0.001, rel= 0.3, amp= 0.1, gate= 1, pan= 0, dir= 1, rate= 5, mod= 0|
    var env= EnvGen.ar(Env.adsr(atk, atk, 0.5, rel), gate, doneAction:2);
    var snd= VarSaw.ar(dir**Sweep.ar(0, rate)*freq, 0, SinOsc.ar(mod, pi)*mod%1);
    OffsetOut.ar(out, Pan2.ar(snd*env, pan, amp));
}).add;
)

//note this will start loud - watch your volume
(
var numX= 16, numY= 8, numZ= 16;
numZ.do{|z|
    if(z>0, {   //skip first because Pbjorklund2(0) makes no sense
        numY.do{|y|
            var name= "pat%_%".format(z, y).asSymbol;   //a unique symbol
            Pdef(name, PmonoArtic(\avball,
                \dur, Pbjorklund2(z, numX, inf, y)*0.125+(z+y*0.0001),
                \amp, 0.02*Pdefn(name, 1),
                \degree, y+z,
                \rel, 0.15,
                \octave, 4,
                \dir, 1.1,
                \rate, 2,
                \pan, Pgauss(0, 0.25, inf),
                \scale, Scale.minor,
                \sustain, 0.05, //sustain here instead of legato because we have Pbjorklund2 with durations
            )).play;
        };
    });
};
)

//try changing Scale, octave and the way degree is calculated

//other things to try...
TempoClock.tempo= 0.2;
TempoClock.tempo= 1;

//turn off all volumes
(
var numX= 16, numY= 8;
numX.do{|x|
    numY.do{|y|
        var name= "pat%_%".format(x, y).asSymbol;
        Pdefn(name, 0); //0 means silence, 1 standard amplitude
    };
};
)

//create a gui with volume controls
(
var numX= 16, numY= 8;
Window().layout_(GridLayout.columns(
    *{|x|
        {|y|
            var name= "pat%_%".format(x, y).asSymbol;
            Slider().action_{|v| Pdefn(name, v.value)};
        }!numY
    }!numX
)).front;
)

//turn on some at random (10% on, 90% off) - run this multiple times
(
var numX= 16, numY= 8;
numX.do{|x|
    numY.do{|y|
        var name= "pat%_%".format(x, y).asSymbol;
        Pdefn(name, [0, 1].wchoose([0.9, 0.1])); //random scale amplitude
    };
};
)

//some other sounds to try - evaluate them while the above is playing
(
SynthDef(\avball, {|out= 0, freq= 500, atk= 0.001, rel= 0.3, amp= 0.1, gate= 1, pan= 0, dir= 1, rate= 5, mod= 0|
    var env= EnvGen.ar(Env.adsr(atk, atk, 0.5, rel), gate, doneAction:2);
    var snd= SinOscFB.ar(dir**Sweep.ar(0, rate)*freq, LFSaw.ar(mod, freq%2)+1/2);
    OffsetOut.ar(out, Pan2.ar(snd*env, pan, amp));
}).add;
)
(
SynthDef(\avball, {|out= 0, freq= 500, atk= 0.001, rel= 0.3, amp= 0.1, gate= 1, pan= 0, dir= 1, rate= 5, mod= 0|
    var env= EnvGen.ar(Env.adsr(atk, atk, 0.5, rel), gate, doneAction:2);
    var snd= SinOscFB.ar(dir**Sweep.ar(0, rate)*freq, SinOsc.ar(mod).abs);
    OffsetOut.ar(out, Pan2.ar(snd*env, pan, amp));
}).add;
)
(
SynthDef(\avball, {|out= 0, freq= 400, atk= 0.01, rel= 0.1, cur= -9, amp= 0.1, gate= 1, pan= 0, mod= 0|
    var env= EnvGen.ar(Env.asr(atk, 1, rel, cur), gate, doneAction:2);
    var snd= VarSaw.ar(freq, 0, env.lag*0.5+mod).sin;
    Out.ar(out, Pan2.ar(snd*env, pan, amp*Line.kr(1, 0)));
}).add;
)

//try with this...
var numX= 12, numY= 12, numZ= 12;   //this is really pushing the computer

//anc with different scales and tunings...
\scale, Scale.major(Tuning.just),

//stop with cmd+. or
Pdef.all.do{|x| x.stop};

//or this to stop them all over 14.4 seconds
fork{Pdef.all.do{|x| x.stop.postln; 0.1.wait}};
```

osc123
==

this example uses four scripts (Line, Plane, Cube and Listener). we can keep them in the same basic scene but not run them simultaneously.

setup
--

* start unity and create a new 3D project
* go to <https://github.com/thomasfredericks/UnityOSC> and click the green download button
* get the .zip file and uncompress it
* find the file `OSC.cs` inside the zip you just uncompressed (UnityOSC-master / Assets / OSC)
* drag&drop it into unity's assets window
* select 'GameObject / Create Empty'
* drag&drop the osc script file onto the new game object
* in the inspector set the 'Out Port' to 57120 (this is the default port of sc)
* select GameObject / 3D Object / Cube. this game object will become our prefab from which the clone will be made
* set the position of the Main Camera to something like x 7.5, y 5.5, z -12
* optionally set the Clear Flag to Solid Color and the Background to black

Line (1D)
--

* create a new script by selecting Assets / Create / C# Script
* give the script the name 'Line' by typing under the white icon
* double click the white C# script icon to open it in MonoDevelop
* copy and paste in the code here below replacing what was there originally

```cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour {
    private int numX= 16;    //must match in sc
    public float scaleAmp= 50;  //how large to scale up
    public float minAmp= 0.0F;    //set to 0.01F or something to make the object not completely disappear
    public float ampFadeRate= 0.9F; //how fast to shrink
    public OSC oscHandler;
    public Transform prefab;    //holds our initial gameobject
    List<Transform> clones = new List<Transform>();
    void Start () {
        Application.runInBackground = true;
        oscHandler.SetAddressHandler("/line", LineFunc);
        for (int x = 0; x < numX; x++) {
            clones.Add (Instantiate (prefab, new Vector3 (x, 0, 0), Quaternion.identity));
        }
        prefab.gameObject.SetActive(false);  //hide prefab object
    }
    void Update () {
        Vector3 minAmpVec = new Vector3 (minAmp, minAmp, minAmp);
        foreach (Transform ct in clones) {
            ct.localScale = Vector3.Max(ct.localScale * ampFadeRate, minAmpVec);
        }
    }
    void LineFunc(OscMessage msg) {
        int x = msg.GetInt (0);
        int y = msg.GetInt (1);
        int z = msg.GetInt (2);
        float degree = msg.GetInt (3);
        float amp = msg.GetFloat (4)*scaleAmp;
        clones[x].localScale = new Vector3 (amp, amp, amp);
    }
}
```

* save and switch back to unity
* drag&drop the Line script onto the Main Camera
* select the Main Camera in the hierarchy window in the inspector find the script we just attached
* drag&drop the empty GameObject onto the 'Osc Handler' variable slot
* drag&drop the Cube onto the 'Prefab' variable slot
* click play

switch to supercollider and run this code...

```supercollider
//--line
(
var numX= 16;
var y= 0;
var z= 10;  //just to pick one in the middle
var off= 0;  //starting index offset
var name= "pat%_%".format(z, y).asSymbol;
var n= NetAddr("127.0.0.1", 6969);
s.latency= 0.05;
Pdef(name, PmonoArtic(\avball,
    \dur, 0.125,
    \amp, Pbjorklund(z, numX, inf, off)*0.02,
    \degree, y,
    \octave, 4,
    \scale, Scale.minor,
    \atk, 0.001,
    \rel, 0.15,
    \dir, 1,
    \rate, 5,
    \mod, 0,
    \pan, Pgauss(0, 0.25, inf),
    \legato, 0.1,
    \x, Pseries(off, 1, inf)%numX,
    \rest, Pswitch([Rest(), 1], Pkey(\amp)>0),
    \osc, Pfunc({|e| if(e.rest==1, {n.sendMsg(\line, e.x, y, z, e.degree, e.amp)}); e}),
)).play;
)
```

you should see a line of 16 cubes reacting on the rhythm.

try changing the pattern while it is running. e.g. set `z` to 12, increase `\mod`, play with `\legato` and `\dur`

try changing the Scale Amp, Min Amp Amp Fade Rate variables in unity while it is running.

also try changing the number of cubes (numX). you will need to change both in sc and in the unity script.

when done click stop and deactivate the Line script in the main camera inspector.

![00deactivate](00deactivate.png?raw=true "00deactivate")

and then `Pdef.all.do{|x| x.stop};` or cmd+. to stop all in sc.

Plane (2D)
--

* create a new script by selecting Assets / Create / C# Script
* give the script the name 'Plane' by typing under the white icon
* double click the white C# script icon to open it in MonoDevelop
* copy and paste in the code here below replacing what was there originally

```cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour {
    private int numX= 16;    //must match in sc
    private int numY= 12;    //must match in sc
    public float scaleAmp= 50;  //how large to scale up
    public float minAmp= 0.0F;    //set to 0.01F or something to make the object not completely disappear
    public float ampFadeRate= 0.9F; //how fast to shrink
    public OSC oscHandler;
    public Transform prefab;    //holds our initial gameobject
    List<Transform> clones = new List<Transform>();
    void Start () {
        Application.runInBackground = true;
        oscHandler.SetAddressHandler("/plane", PlaneFunc);
        for (int y = 0; y < numY; y++) {
            for (int x = 0; x < numX; x++) {
                clones.Add (Instantiate (prefab, new Vector3 (x, y, 0), Quaternion.identity));
            }
        }
        prefab.gameObject.SetActive(false);  //hide prefab object
    }
    void Update () {
        Vector3 minAmpVec = new Vector3 (minAmp, minAmp, minAmp);
        foreach (Transform ct in clones) {
            ct.localScale = Vector3.Max(ct.localScale * ampFadeRate, minAmpVec);
        }
    }
    void PlaneFunc(OscMessage msg) {
        int x = msg.GetInt (0);
        int y = msg.GetInt (1);
        int z = msg.GetInt (2);
        float degree = msg.GetInt (3);
        float amp = msg.GetFloat (4)*scaleAmp;
        clones[x+(y*numX)].localScale = new Vector3 (amp, amp, amp);
    }
}
```

* save and switch back to unity
* drag&drop the Plane script onto the Main Camera
* select the Main Camera in the hierarchy window in the inspector find the script we just attached
* drag&drop the empty GameObject onto the 'Osc Handler' variable slot
* drag&drop the Cube onto the 'Prefab' variable slot
* click play

switch to supercollider and run this code...

```supercollider
//--plane
(
var numX= 16;
var numY= 12;
var z= 10;
var n= NetAddr("127.0.0.1", 6969);
s.latency= 0.05;
numY.do{|y|
    var off= 0; //here 2.rand or y or y%2 or [0,9,1].choose etc
    var name= "pat%_%".format(z, y).asSymbol;
    Pdef(name, PmonoArtic(\avball,
        \dur, 0.125,
        \amp, Pbjorklund(z, numX, inf, off)*0.02,
        \degree, y,
        \octave, 4,
        \scale, Scale.minor,
        \atk, 0.001,
        \rel, 0.15,
        \dir, 1,
        \rate, 5,
        \mod, 0,
        \pan, Pgauss(0, 0.25, inf),
        \legato, 0.1,
        \x, Pseries(off, 1, inf)%numX,
        \rest, Pswitch([Rest(), 1], Pkey(\amp)>0),
        \osc, Pfunc({|e| if(e.rest==1, {n.sendMsg(\plane, e.x, y, z, e.degree, e.amp)}); e}),
    )).play;
};
)
```

you should see a grid of 16 * 12 cubes reacting on the rhythm.

this example become much more interesting when you change the starting offset indices.
```supercollider
var off= 2.rand;
```
```supercollider
var off= y;
```
```supercollider
var off= y%2;
```
```supercollider
var off= [0, 9, 1].choose
```

together with this also start messing the with duration for each (then they will drift out of sync). change this line...
```supercollider
\dur, 0.125+(y*0.0005),
```

experiment more with the sc patterns code. also in unity change background, add a material to the prefab, edit the script and public variables.

when done click stop and deactivate the Plane script in the inspector.

and then `Pdef.all.do{|x| x.stop};` or cmd+. to stop all in sc.

Cube (3D)
--

* create a new script by selecting Assets / Create / C# Script
* give the script the name 'Cube' by typing under the white icon
* double click the white C# script icon to open it in MonoDevelop
* copy and paste in the code here below replacing what was there originally

```cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour {
    private int numX= 16;    //must match in sc
    private int numY= 12;    //must match in sc
    private int numZ= 9;    //must match in sc
    public float scaleAmp= 50;  //how large to scale up
    public float minAmp= 0.0F;    //set to 0.01F or something to make the object not completely disappear
    public float ampFadeRate= 0.9F; //how fast to shrink
    public OSC oscHandler;
    public Transform prefab;    //holds our initial gameobject
    List<Transform> clones = new List<Transform>();
    void Start () {
        Application.runInBackground = true;
        oscHandler.SetAddressHandler("/cube", CubeFunc);
        for (int z = 0; z < numZ; z++) {
            for (int y = 0; y < numY; y++) {
                for (int x = 0; x < numX; x++) {
                    clones.Add (Instantiate (prefab, new Vector3 (x, y, z), Quaternion.identity));
                }
            }
        }
        prefab.gameObject.SetActive(false);  //hide prefab object
    }
    void Update () {
        Vector3 minAmpVec = new Vector3 (minAmp, minAmp, minAmp);
        foreach (Transform ct in clones) {
            ct.localScale = Vector3.Max(ct.localScale * ampFadeRate, minAmpVec);
        }
    }
    void CubeFunc(OscMessage msg) {
        int x = msg.GetInt (0);
        int y = msg.GetInt (1);
        int z = msg.GetInt (2);
        float degree = msg.GetInt (3);
        float amp = msg.GetFloat (4)*scaleAmp;
        clones[x+(y*numX)+(z*numX*numY)].localScale = new Vector3 (amp, amp, amp);
    }
}
```

* save and switch back to unity
* drag&drop the Cube script onto the Main Camera
* select the Main Camera in the hierarchy window in the inspector find the script we just attached
* drag&drop the empty GameObject onto the 'Osc Handler' variable slot
* drag&drop the Cube onto the 'Prefab' variable slot
* click play

switch to supercollider and run this code...

```supercollider
//--cube
(
var numX= 16;
var numY= 12;
var numZ= 9;
var n= NetAddr("127.0.0.1", 6969);
s.latency= 0.05;
numZ.do{|z|
    if(z>0, {
        numY.do{|y|
            var off= 0;  //change here
            var name= "pat%_%".format(z, y).asSymbol;
            Pdef(name, PmonoArtic(\avball,
                \dur, 0.125,  //and here
                \amp, Pbjorklund(z, numX, inf, off)*0.02,
                \degree, y,
                \octave, 4,
                \scale, Scale.minor,
                \atk, 0.001,
                \rel, 0.15,
                \dir, 1,
                \rate, 5,
                \mod, 0,
                \pan, Pgauss(0, 0.25, inf),
                \legato, 0.1,
                \x, Pseries(off, 1, inf)%numX,
                \rest, Pswitch([Rest(), 1], Pkey(\amp)>0),
                \osc, Pfunc({|e| if(e.rest==1, {n.sendMsg(\cube, e.x, y, z, e.degree, e.amp)}); e}),
            )).play;
        };
    });
};
)
```
you should have a cube of 16 * 12 * 9 cubes reacting on the rhythm.

again change offset for each amplitude pattern...
```supercollider
var off= 0;
```
```supercollider
var off= 3.rand;
```
```supercollider
var off= [0, 5].choose;
```

etc.

and again try to break up the sync like this...

```supercollider
\dur, 0.125+(y*0.001),
```
```supercollider
\dur, 0.125+(y%2*0.125),
```

the last one will create two groups - even and odd rows - one playing the patterns at half the speed.

when done click stop and deactivate the Cube script in the inspector.

and then `Pdef.all.do{|x| x.stop};` or cmd+. to stop all in sc.

![01cube](01cube.png?raw=true "01cube")

Listener
--

last we will push the computer even more and make it so that unity send back osc. there will be a flood of numbers with distances from the main camera position to each cube. in sc we will use that to scale the amplitude so that only cubes nearby the camera will be heard. a bit like you can only hear the nearby cubes as you move around.

* create a new script by selecting Assets / Create / C# Script
* give the script the name 'Listener' by typing under the white icon
* double click the white C# script icon to open it in MonoDevelop
* copy and paste in the code here below replacing what was there originally

```cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Listener : MonoBehaviour {
    private int numX= 16;    //must match in sc
    private int numY= 12;    //must match in sc
    private int numZ= 9;    //must match in sc
    public float scaleAmp= 50;  //how large to scale up
    public float minAmp= 0.0F;    //set to 0.01F or something to make the object not completely disappear
    public float ampFadeRate= 0.9F; //how fast to shrink
    public string address = "/dist";
    public OSC oscHandler;
    public Transform prefab;    //holds our initial gameobject
    List<Transform> clones = new List<Transform>();
    Vector3 lastPosition= new Vector3(-999, -999, -999);
    void Start () {
        Application.runInBackground = true;
        oscHandler.SetAddressHandler("/listen", ListenFunc);
        for (int z = 0; z < numZ; z++) {
            for (int y = 0; y < numY; y++) {
                for (int x = 0; x < numX; x++) {
                    clones.Add (Instantiate (prefab, new Vector3 (x, y, z), Quaternion.identity));
                }
            }
        }
        prefab.gameObject.SetActive(false);  //hide prefab object
    }
    void Update () {
        Vector3 minAmpVec = new Vector3 (minAmp, minAmp, minAmp);
        foreach (Transform ct in clones) {
            ct.localScale = Vector3.Max(ct.localScale * ampFadeRate, minAmpVec);
        }
        if (transform.localPosition != lastPosition) {    //only send if camera moved
            lastPosition = transform.localPosition;
            for (int z = 0; z < numZ; z++) {    //because packet limit is 1000 we break it into numZ messages
                OscMessage msg = new OscMessage ();
                msg.address = address;
                msg.values.Add (z);    //first an index
                for (int y = 0; y < numY; y++) {
                    for (int x = 0; x < numX; x++) {
                        int i = x + (y * numX) + (z * numX * numY);
                        float distance= Vector3.Distance (lastPosition, clones[i].localPosition);
                        msg.values.Add (distance);
                    }
                }
                oscHandler.Send (msg);
            }
        }
    }
    void ListenFunc(OscMessage msg) {
        int x = msg.GetInt (0);
        int y = msg.GetInt (1);
        int z = msg.GetInt (2);
        float degree = msg.GetInt (3);
        float amp = msg.GetFloat (4)*scaleAmp;
        clones[x+(y*numX)+(z*numX*numY)].localScale = new Vector3 (amp, amp, amp);
    }
}
```

* save and switch back to unity
* drag&drop the Listener script onto the Main Camera
* select the Main Camera in the hierarchy window in the inspector find the script we just attached
* drag&drop the empty GameObject onto the 'Osc Handler' variable slot (check that the Out Port is set to 57120)
* drag&drop the Cube onto the 'Prefab' variable slot
* click play

switch to supercollider and run this code...

```supercollider
//--listener
(
var numX= 16;
var numY= 12;
var numZ= 9;
var n= NetAddr("127.0.0.1", 6969);
s.latency= 0.05;
numZ.do{|z|
    if(z>0, {
        numY.do{|y|
            var off= 0;  //change here
            var name= "pat%_%".format(z, y).asSymbol;
            Pdef(name, PmonoArtic(\avball,
                \dur, 0.125,//+(y*0.001),
                \amp, Pbjorklund(z, numX, inf, off)*0.02*Pdefn(name, Pseq(1!numX, inf)),
                \degree, y,
                \octave, 4,
                \scale, Scale.minor,
                \atk, 0.001,
                \rel, 0.15,
                \dir, 1,
                \rate, 5,
                \mod, 0,
                \pan, Pgauss(0, 0.25, inf),
                \legato, 0.1,
                \x, Pseries(off, 1, inf)%numX,
                \rest, Pswitch([Rest(), 1], Pkey(\amp)>0),
                \osc, Pfunc({|e| if(e.rest==1, {n.sendMsg(\listen, e.x, y, z, e.degree, e.amp)}); e}),
            )).play;
        };
    });
};
OSCdef(\listener, {|msg|
    var z= msg[1];
    numY.do{|y|
        var name= "pat%_%".format(z, y).asSymbol;
        var arr= msg.copyRange(y*numX+2, y+1*numX+1);
        Pdefn(name, Pseq(arr.abs.lincurve(0, 10, 1, 0, -1), inf));  //10 here is hearing area
    };
}, \dist);
)
```

move the main camera around and you should hear and see the patterns objects come and go.

if you do not hear or see anything move the camera to the middle (7.5, 5.5, 4)

links
==

https://en.wikibooks.org/wiki/Designing_Sound_in_SuperCollider
