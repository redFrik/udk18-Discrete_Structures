networking
--------------------

osc (OpenSound Control) is a protocol/technique for sending messages over network. both supercollider and unity can receive and send osc. the network can be _inside_ your machine (between programs) or across a local network (between programs on different computers). there are also ways of sending osc over the internet to remote computers but that's more advanced and will not be considered here.

supercollider
==

first make sure all computers that we want to send messages to are connected to the same [wifi] network.

```supercollider
s.options.maxLogins= 8;
s.options.bindAddress= "0.0.0.0";
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

//sc already uses osc internally - between sclang and scsynth (the server)
//to see the messages run this line...
OSCdef.trace(true, true);  //turn on osc debugging
//these messages we will later pass on to unity

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
OSCdef.trace(false);  //turn off osc debugging

//--
//now figure out the ip of some other machine running sc
//must be connected to the same wifi network
a= Server("neighbour", NetAddr("192.168.1.101", 57110));
b= Server("acrosstable", NetAddr("192.168.1.102", 57110));
//also try to add more servers here and store them in variables c, d, e etc

(
Pdef(\pat1, PmonoArtic(\avclick,
    \degree, Pseq([0, 1, 2, 3, 4, 5, 6], inf),
    \dur, 1,  //slow
    \legato, 0.05,
    \server, Pseq([s, a, b], inf).trace,  //this decides which server to play on - s is your own machine
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

then that means the server at one of the ip addresses is not reachable.

note: there will be lots of latency and hickups in the rhytm. that is to be expected when using wifi and will also depend on the quality of the wifi network - how much traffic etc. if you need to greatly improve timing connect the computers via a router with ethernet cables.

also note: this works because we all added the same synthdef above (called \avclick). if you want to play your own synthdefs on remote servers you will need to first send them over. do that by changing `.add;` in the end to `.send(b);`. this will send the synthdef to the server stored in variable `b`. or another technique to send over arbitrary synthdefs is to first add the servers to a synthdesclib with... `SynthDescLib.global.addServer(b);` and then `.add(\global);` will distribute that synthdef to all servers added to the global synthdesclib.

- - -

unity3d
==

* start unity and create a new 3D (or 2D) project
* go to <https://github.com/thomasfredericks/UnityOSC> and click the green download button
* get the .zip file and uncompress it
* find the file `OSC.cs` inside the zip you just uncompressed (UnityOSC-master / Assets / OSC)
* drag&drop it into unity's assets window
* select 'GameObject / Create Empty'
* drag&drop the osc script file onto the new game object
* in the inspector set the 'Out Port' to 57120 (this is the default port of sc)

your scene should look like this...

![00osc](00osc.png?raw=true "00osc")

note:  the number 6969 is the port number for incoming osc that unity will use. this can be different and change but then you need to adapt your sending code (in sc) to send to that port instead. you can have multiple listening ports open as long as they have different numbers. if you try 57110 for example (and your supercollider server is booted) your unity scene will complain when started... `System.Net.Sockets:Sockets.SocketException: Address already in use`. that means that that port is already taken and some other program is listening on it.

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
* switch to supercollider and run the following lines one by one...

```supercollider
n= NetAddr("127.0.0.1", 6969);
n.sendMsg(\position, 1.1, 2.2, 3.3);
n.sendMsg(\position, 0, 0, 0);
n.sendMsg(\scale, 5, 1, 10);
n.sendMsg(\rotation, 45, 35, 25);

Routine.run({300.do{|i| n.sendMsg(\rotation, i, i, i); (1/60).wait}});
```

and then try with our patterns...

```supercollider
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
    \amp, Pseq([1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1, 1], inf)*0.2,
    \osc, Pfunc({|e|
        n.sendMsg(\position, e.degree-5, 0, 0);
        n.sendMsg(\scale, e.amp*25+1, e.amp*25+1, e.amp*25+1);
        n.sendMsg(\rotation, e.dur*360, e.dur*360, e.dur*360);
    }),
)).play;
)

//start a second conflicting pattern sequencer (overriding the osc messages above)
(
Pdef(\patOsc2, PmonoArtic(\avclick,
    \degree, Pseq([0, 1, 2, 3, 4, 5, 6, 7], inf)+5,
    \dur, Pseq([0.25, 0.25, 0.125], inf),
    \legato, 0.5,
    \amp, Pseq([1, 0, 1, 1, 1, 1, 0, 1, 1, 1, 1, 0, 1, 1, 1, 1], inf)*0.2,
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

things to try...

* click on the Cube and select Component / Physics / Rigidbody. this will make the Cube be affected by scene gravity and constantly fall. run the sc code to reset its position. play around with parameters like Mass and Drag. you can also change world gravity in Edit / Project Settings / Physics
* duplicate the Cube a few times. (the rigidbody will make them bounce off each other as the osc is trying to position them at the same place)
* do not forget changing the Clear Flags and background colours of the Main Camera
* try changing the line `n= NetAddr("127.0.0.1", 6969);` in the supercollider code. make it send osc to another machine on the same network i.e. `n= NetAddr("192.168.1.100", 6969);`. now your sc patterns should control someone else's spheres in unity.

sending
--

in the screenshot above the 'Out IP' and 'Out Port' means to send out osc to another program running on the same machine (your computer). to receive what unity send that program will need to listen for osc on port 57120 - which supercollider does by default. sc can received osc with either the `OSCdef` or `OSCFunc` class.

note: that before we used the port 57110 and not 57120 when making sound on other computers in the network. 57110 is the default port of supercollider _server_. below we will use 57120 and send to supercollider language because it is easier. but you could also send osc from unity directly to sc server and that'd be good to get an efficient and low latency connection between the program. this is more advanced though as the messages will have to be formatted in a special way for the server to understand and not described here.

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
    public string address = "/pos";
    void Start () {
        Application.runInBackground = true;
    }
    void Update () {
        float x = Mathf.Sin (Time.time * 3.0F) * 5.5F;
        float y = Mathf.Cos (Time.time * 5.0F) * 4.4F;
        float z = Mathf.Sin (Time.time * 2.0F) * 3.3F;
        transform.localPosition= new Vector3 (x, y, z);

        OscMessage msg= new OscMessage();
        msg.address = address;
        msg.values.Add(x);
        msg.values.Add(y);
        msg.values.Add(z);
        oscHandler.Send(msg);
    }
}
```

* save and switch back to unity
* drag&drop the sender script onto the Sphere
* select the Sphere in the hierarchy window
* again in the inspector find the script and where it says 'Osc Handler'
* drag&drop the empty GameObject onto that variable slot
* click play and the Sphere should move around
* switch to supercollider and run the following lines one by one

```supercollider
OSCdef.trace(true, true);  //this should start posting a lot of data
OSCdef.trace(false);  //turn it off
```

if that works set up a osc receiver/listener for the `\pos` address in sc like this...

```supercollider
OSCdef(\pos, {|msg| p= msg.copyRange(1, 3).postln}, \pos);  //assign to variable p and also post
```

so now `p[0]` will contain our x position, `p[1]` the y and `p[2]` the z position.

we can use them in a patter sequencer like this...

```supercollider
(
p= [0, 0, 0];
OSCdef(\pos, {|msg| p= msg.copyRange(1, 3)}, \pos);
Pdef(\patxyz, PmonoArtic(\avclick,
    \degree, Pfunc({p[1].linlin(-5, 5, -8, 8)}).round,  //y-pos (height) mapped to pitch
    \pan, Pfunc({p[0].linlin(-5, 5, -1, 1)}),  //x-pos controls left-right panning
    \dur, 0.125,
    \mod, Pfunc({p[2].linexp(-5, 5, 0.01, 0.5)}),  //z-pos controls modulation
    \legato, Pseq([Pseq([0.2], 12), 1, 1, 1, 0.2], inf),
    \rel, Pfunc({p[2].linexp(-5, 5, 0.01, 1)}),  //z-pos (depth) also for note length
    \amp, Pseq([1, 1, 1, 0, 1, 1, 0, 0.5], inf)*Pfunc({p[2].linlin(-5, 5, 0.5, 0.01)}),  //and for amplitude
)).play;
)
```

now try editing the script for the movement of the Sphere (in the Sender script).

things to try...

* send osc to your neighbour - either from sc or from unity
* try controlling the cube from sc at the same time as the sphere is controlling the sound
* advanced: connect osc feedback by having patterns both controlling and being controlled the sphere (or cube, or both cross connected).
* advanced: set up osc to/from some example we did previously this semester

colours
--

we can extend our receiving C# unity script to also understand a message with the address `\colour`. so open the Receiver script and change it to this...

```cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Receiver : MonoBehaviour {
    public OSC oscHandler;
    Renderer rend;    //added this variable
    void Start () {
        Application.runInBackground = true;
        oscHandler.SetAddressHandler("/position", Position);
        oscHandler.SetAddressHandler("/scale", Scale);
        oscHandler.SetAddressHandler("/rotation", Rotation);
        oscHandler.SetAddressHandler("/colour", Colour);    //and this line
        rend= GetComponent<Renderer>();    //and here we set the rend variable
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
    void Colour(OscMessage msg) {    //added this method that sets the colour of the material of the renderer when a message arrives
        float r = msg.GetFloat(0);
        float g = msg.GetFloat(1);
        float b = msg.GetFloat(2);
        float a = msg.GetFloat(3);
        rend.material.color = new Color (r, g, b, a);
    }
}
```

it is almost the same code. there are only a few additions (see the comments). but to make it work you also need to perform the following steps in unity...

* select Assets / Create / Material
* give the material a name (does not matter what)
* in the inspector set the material's Rendering Mode to Transparent
* drag&drop the material onto the Cube (or whichever object that uses the Receiver script)
* press play and then in supercollider try the following lines one by one - the colour of the Cube should switch from red to green to blue.

```supercollider
n= NetAddr("127.0.0.1", 6969);
n.sendMsg(\colour, 1, 0, 0, 1);
n.sendMsg(\colour, 0, 1, 0, 1);
n.sendMsg(\colour, 0, 0, 1, 1);
n.sendMsg(\colour, 0, 0, 1, 0.05);  //blue but with very low alpha
```

if that works then try the following code...

```supercollider
(
s.latency= 0.03;
n= NetAddr("127.0.0.1", 6969);
Pdef(\patOsc, PmonoArtic(\avclick,
    \degree, Pseq([0, 1, 2, 3, 4, 5, 6, 7], inf),
    \dur, 0.125,
    \legato, 0.2,
    \rel, Pseg([0.1, 0.6, 0.1], 18, 'lin', inf),
    \mod, Pseg([0, 1, 0], 17, 'lin', inf),
    \amp, Pseq([1, 1, 1, 1, 1, 1, 0], inf)*0.2,
    \index, Ptime(),
    \osc, Pfunc({|e|
        n.sendMsg(\position, e.degree-3.5, e.mod, 0);
        n.sendMsg(\scale, e.amp*10+0.2, e.amp*10+0.2, 1-e.amp*10);
        n.sendMsg(\rotation, e.index*10, 0, 0);
        n.sendMsg(\colour, e.rel, 1, e.rel, e.mod);
    }),
)).play;
)
```

this will map a lot of parameters from the pattern sequencer and send them over to unity via osc. e.g. mod is controlling the alpha and rel the red and blue colour channels.

links
==

https://thomasfredericks.github.io/UnityOSC/

on osx you can find ip addresses with lanscan https://www.iwaxx.com/lanscan/

fing is another program that works on android and ios

on linux (and also osx) you can install nmap and then in terminal run `nmap -sn 192.168.1.0/24 -oG - | awk '/Up$/{print $2, $3}'`

or, if you own the router, you can also log in to the administration page and normally there find a menu that lists all connected devices.
