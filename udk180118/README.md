networking
--------------------

this time we will connect the two programs.

osc (OpenSound Control) is a protocol for network messages. boh supercollider and unity can receive and send these.

supercollider
==

first make sure we all connect to the same network.

```supercollider
s.options.maxLogins= 8;
s.reboot;
s.meter;
s.scope;

//--load a sound
(
SynthDef(\avclick, {|out= 0, freq= 400, atk= 0.01, rel= 0.1, cur= -9, amp= 0.1, gate= 1, pan= 0, mod= 0|
    var env= EnvGen.ar(Env.asr(atk, 1, rel, cur), gate, doneAction:2);
    var snd= VarSaw.ar(freq, 0, env.lag*0.5+mod).sin;
    Out.ar(out, Pan2.ar(snd*env, pan, amp*Line.kr(1, 0)));
}).add;
)

//start simple
(
Pdef(\pat1, PmonoArtic(\avclick,
    \degree, Pseq([0, 1, 2, 3, 4, 5, 6], inf),
    \dur, 0.5,
    \legato, 0.05,
)).play;
)

(
Pdef(\pat1, PmonoArtic(\avclick,
    \degree, Pseq([0, 1, 2, 3, 4, 5, 6], inf),
    \octave, Pseq([5, 5, 4, 5, 5, 6], inf),  //adding octave transposition
    \dur, 0.25,  //and play faster
    \legato, 0.05,
)).play;
)

(
Pdef(\pat1, PmonoArtic(\avclick,
    \degree, Pseq([0, 1, 2, 3, 4, 5, 6], inf),
    \octave, Pseq([5, 5, 4, 5, 5, 6], inf),
    \scale, Scale.major(Tuning.just),  //scale + tuning
    \dur, Pseq([Pseq([0.25], 12), 0.125, 0.125], inf),  //strange rhythm
    \rel, Pseq([Pseq([0.1], 11), 0.6], inf),  //some notes longer
    \legato, 0.05,
)).play;
)

Pdef(\pat1).stop;

//--
//now figure out the ip of some other machine running sc
//must be connected to the same wifi network
a= Server("neighbour", NetAddr("192.168.1.101", 57110));
b= Server("neighbour2", NetAddr("192.168.1.102", 57110));
//also try to add more here c, d, e etc

(
Pdef(\pat1, PmonoArtic(\avclick,
    \degree, Pseq([0, 1, 2, 3, 4, 5, 6], inf),
    \dur, 1,  //slow
    \legato, 0.05,
    \server, Pseq([s, a, b], inf).trace,  //this decides which server to play on
)).play;
)

(
Pdef(\pat1, PmonoArtic(\avclick,
    \degree, Pseq([0, 1, 2, 3, 4, 5, 6], inf),
    \dur, 0.25,  //faster
    \legato, 0.05,
    \server, Pseq([s, s, a, b], inf),
)).play;
)

Pdef(\pat1).stop;
```

if you get the error message...
```
ERROR: Primitive '_NetAddr_SendBundle' failed.
caught exception 'send_to: Host is down' in primitive in method NetAddr:sendBundle
```

then that means the server on one of the ip addresses is not reachable.

- - -

unity3d
==

* start unity and create a new 3D (or 2D) project
* go to <https://github.com/thomasfredericks/UnityOSC> and click the green download button
* get the .zip file and uncompress it
* find the file `OSC.cs` the zip you just uncompressed (UnityOSC-master / Assets / OSC)
* drag&drop it into unity's assets window
* select 'GameObject / Create Empty'
* drag&drop the osc script file onto the new game object
* in the inspector set the 'Out Port' to 57120 (this is the default port of sc)

your scene should look like this...

![00osc](00osc.png?raw=true "00osc")

receiving
--

* select 'GameObject / 3D Object / Cube'
* create a new script by selecting Assets / Create / C# Script
* give the script the name 'Receiver' by typing under the white icon
* double click the white C# script icon to open it in MonoDevelop
* copy and paste in the code here below replacing what was there originally

```cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Receiver : MonoBehaviour {
    public OSC oscHandler;
    void Start () {
        Application.runInBackground = true;
        oscHandler.SetAddressHandler("/position", Position);
        oscHandler.SetAddressHandler("/scale", Scale);
        oscHandler.SetAddressHandler("/rotation", Rotation);
    }
    void Update () {
    }
    void Position(OscMessage msg) {
        float x = msg.GetFloat(0);
        float y = msg.GetFloat(1);
        float z = msg.GetFloat(2);
        transform.localPosition = new Vector3(x, y, z);
    }
    void Scale(OscMessage msg) {
        float x = msg.GetFloat(0);
        float y = msg.GetFloat(1);
        float z = msg.GetFloat(2);
        transform.localScale = new Vector3(x, y, z);
    }
    void Rotation(OscMessage msg) {
        float x = msg.GetFloat(0);
        float y = msg.GetFloat(1);
        float z = msg.GetFloat(2);
        transform.localEulerAngles = new Vector3(x, y, z);
    }
}
```

* save and switch back to unity
* drag&drop the receiver script onto the Cube
* select the Cube in the hierarchy window
* in the inspector find the script and where it says 'Osc Handler'
* drag&drop the empty GameObject onto that variable slot
* click play and nothing should happen
* switch to supercollider and run the following lines one by one

```supercollider
n= NetAddr("127.0.0.1", 6969)
n.sendMsg(\position, 1.1, 2.2, 3.3);
n.sendMsg(\position, 0, 0, 0);
n.sendMsg(\scale, 5, 1, 10);
n.sendMsg(\rotation, 45, 35, 25);

Routine.run({300.do{|i| n.sendMsg(\rotation, i, i, i); (1/60).wait}});
```

and then try with our patterns...

```supercollider(
s.latency= 0.03;
n= NetAddr("127.0.0.1", 6969);
Pdef(\patOsc, PmonoArtic(\avclick,
    \degree, Pseq([0, 1, 2, 3, 4, 5, 6, 7], inf),
    \dur, 0.25,
    \legato, 0.1,
    \osc, Pfunc({|e| n.sendMsg(\position, e.degree-5, 0, 0)}),
)).play;
)

(
s.latency= 0.03;
n= NetAddr("127.0.0.1", 6969);
Pdef(\patOsc, PmonoArtic(\avclick,
    \degree, Pseq([0, 1, 2, 3, 4, 5, 6, 7], inf),
    \dur, Pseq([0.25, 0.25, 0.125], inf),
    \legato, 0.2,
    \amp, Pbjorklund(11, 16)*0.2,
    \osc, Pfunc({|e|
        n.sendMsg(\position, e.degree-5, 0, 0);
        n.sendMsg(\scale, e.amp*25+1, e.amp*25+1, e.amp*25+1);
        n.sendMsg(\rotation, e.dur*360, e.dur*360, e.dur*360);
    }),
)).play;
)

//a second conflicting pattern sequencer (overriding the osc messages above)
(
Pdef(\patOsc2, PmonoArtic(\avclick,
    \degree, Pseq([0, 1, 2, 3, 4, 5, 6, 7], inf)+5,
    \dur, Pseq([0.25, 0.25, 0.125], inf),
    \legato, 0.5,
    \amp, Pbjorklund(13, 16)*0.2,
    \osc, Pfunc({|e|
        n.sendMsg(\position, e.degree-5, 0, 0);
        n.sendMsg(\scale, e.amp*10+1, e.amp*10+1, e.amp*10+1);
        n.sendMsg(\rotation, e.dur*90, e.dur*90, e.dur*90);
    }),
)).play;
)

Pdef(\patOsc).stop;
Pdef(\patOsc2).stop;
```

go to unity and click stop.

sending
--

* select 'GameObject / 3D Object / Sphere'
* create a new script by selecting Assets / Create / C# Script
* give the script the name 'Sender' by typing under the white icon
* double click the white C# script icon to open it in MonoDevelop
* copy and paste in the code here below replacing what was there originally

```cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sender : MonoBehaviour {
    public OSC oscHandler;
    void Start () {
        Application.runInBackground = true;
    }
    void Update () {
        float x = Mathf.Sin (Time.time * 3.0F) * 5.5F;
        float y = Mathf.Cos (Time.time * 5.0F) * 4.4F;
        float z = Mathf.Sin (Time.time * 2.0F) * 3.3F;
        transform.localPosition= new Vector3 (x, y, z);

        OscMessage msg= new OscMessage();
        msg.address = "/pos";
        msg.values.Add(x);
        msg.values.Add(y);
        msg.values.Add(z);
        osc.Send(msg);
    }
}
```

* save and switch back to unity
* drag&drop the receiver script onto the Sphere
* select the Sphere in the hierarchy window
* again in the inspector find the script and where it says 'Osc Handler'
* drag&drop the empty GameObject onto that variable slot
* click play and the Sphere should move around
* switch to supercollider and run the following lines one by one

```supercollider
OSCdef.trace(true, true);  //this should start posting a lot of data

OSCdef.trace(false);  //turn it off
```

if that works set up a receiver for the `\pos` address in sc like this...

```supercollider
OSCdef(\pos, {|msg| p= msg.copyRange(1, 3).postln}, \pos);  //assign to variable p and also post
```

so now `p[0]` will contain our x position, `p[1]` the y and `p[2]` z position.

we can use them in a patter sequencer like this...

```supercollider
(
OSCdef(\pos, {|msg| p= msg.copyRange(1, 3)}, \pos);
Pdef(\patxyz, PmonoArtic(\avclick,
    \degree, Pfunc({p[1].linlin(-5, 5, -8, 8)}).round,  //y-pos (height) mapped to pitch
    \pan, Pfunc({p[0].linlin(-5, 5, -1, 1)}),  //x-pos controls left-right panning
    \dur, 0.125,
    \mod, Pfunc({p[2].linexp(-5, 5, 0.01, 0.5)}),  //z-pos controls modulation
    \legato, Pseq([Pseq([0.2], 12), 1, 1, 1, 0.2], inf),
    \rel, Pfunc({p[2].linexp(-5, 5, 0.01, 1)}),  //z-pos (depth) also for note length
    \amp, Pbjorklund(19, 32)*Pfunc({p[2].linlin(-5, 5, 0.5, 0.01)}),  //and for amplitude
)).play;
)
```

now try writing the script for the movement of the Sphere.

things to try...

* send osc to your neighbour - either from sc or from unity
* try controlling the cube from sc at the same time as the sphere is controlling the sound
* advanced: connect osc feedback by having patterns both controlling and being controlled the sphere (or cube, or both crossconnected).
* advanced: set up osc to/from some example we did previously

links
==

https://thomasfredericks.github.io/UnityOSC/
