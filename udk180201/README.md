recapitulation
--------------------

a project with most of the examples from this semester...

* download the folder Discrete_Structures from this repository (go up one level, click the green button, download zip).
* start unity, click open and select the folder Discrete_Structures
* now try out the different scenes by double clicking on them

![00project](00project.png?raw=true "00project")

export
--

when you want to perform your unity work or share it with others, you can 'build' it. load a project and try...

* select File / Build & Run

![01build](01build.png?raw=true "01build")

this will produce a standalone application that should run on a similar machine as you used when building it.

* select Edit / Project Settings / Player

with the settings found here are also worth exploring. you can specify things like splash screen, icon, resolution etc.

![02playersettings](02playersettings.png?raw=true "02playersettings")

* select File / Build Settings...

here there are some more advanced options for build for older operating systems etc. with the WebGL module you can build your project to run on a webpage. anyone with a modern browser can see and interact with it.

![03buildsettings](03buildsettings.png?raw=true "03buildsettings")

similar with Android and iOS modules for running your project on devices with these systems.

just note that these modules require you to download and install big modules that take a lot of disk space and probably also sign up for developer accounts.

unity3d also offer building in the cloud.

fade and quant
--

how to cross fade and play your sequences in sync.

```supercollider
s.boot;

(
SynthDef(\avball, {|out= 0, freq= 500, atk= 0.001, rel= 0.3, amp= 0.1, gate= 1, pan= 0, dir= 1, rate= 5, mod= 0|
    var env= EnvGen.ar(Env.adsr(atk, atk, 0.5, rel), gate, doneAction:2);
    var snd= VarSaw.ar(dir**Sweep.ar(0, rate)*freq, 0, SinOsc.ar(mod, pi)*mod%1);
    OffsetOut.ar(out, Pan2.ar(snd*env, pan, amp));
}).add;
)

Pdef(\myseq).fadeTime= 5;  //5 seconds to cross fade
Pdef(\myseq).quant= 4;  //wait with change until next 4th beat
TempoClock.beats;  //show current elapsed beats since you started sc

(
Pdef(\myseq, PmonoArtic(\avball,
    \degree, Pseq([4, 4, 2, 12, 1, 2, 3, 4, 3, 2, 10, 0, 0, 0, 0], inf),
    \dur, 0.125,
    \mod, Pseq([0, 0.9, 0.1], inf),
    \scale, Scale.minor(Tuning.just),
    \legato, 0.2,
)).play;
)

(
Pdef(\myseq, PmonoArtic(\avball,
    \degree, Pseq([1, 2, 3, 4], inf),
    \dur, 0.125,
    \mod, 5,
    \legato, 0.2,
)).play;
)

//now try to change back and forth between these two sequences with different settings...
Pdef(\myseq).fadeTime= 0.1;
Pdef(\myseq).quant= 8;
TempoClock.tempo= 1.2;  //a bit faster (1.2*60= 70bpm)
```

control busses, midi, pdefn
--

this example show ways how to control your sounds using control rate busses, a midi controller and pdefn:s.

```supercollider
s.latency= 0.05;  //important when using midi - else will react slow
s.boot;

//load a soundfile and create 3 control busses
(
b.free; b= Buffer.read(s, Platform.resourceDir+/+"sounds/a11wlk01.wav");
~rateBus.free; ~rateBus= Bus.control;
~mixBus.free; ~mixBus= Bus.control;
~volBus.free; ~volBus= Bus.control;
)

//start playing
(
a= {
    var snd= PlayBuf.ar(b.numChannels, b, BufRateScale.kr(b), loop: 1);
    var efx= DelayC.ar(snd, 1, LFDNoise0.ar(In.kr(~rateBus)).range(0.01, 1));
    var mix= XFade2.ar(snd, efx, In.kr(~mixBus));
    Splay.ar(mix)*In.kr(~volBus);
}.play;
)

~volBus.set(1);  //turn up the volume

~rateBus.set(2);  //set the stutter rate
~rateBus.set(20);  //set the stutter rate

~mixBus.set(1);  //set effect mix to 100%
~mixBus.set(0)  //mix dry and wet signal 50%
~mixBus.set(-1)  //set original sound

~volBus.set(0.25);  //turn down the volume

//start a second player using the same 3 control busses
(
c= {
    var snd= PlayBuf.ar(b.numChannels, b, BufRateScale.kr(b), loop: 1);
    var efx= DelayC.ar(snd, 1, SinOsc.ar(In.kr(~rateBus)).range(0.01, 1));
    var mix= XFade2.ar(snd, efx, In.kr(~mixBus));
    Splay.ar(mix)*In.kr(~volBus);
}.play;
)

~mixBus.set(0)
~mixBus.set(1)
~rateBus.set(10)
~rateBus.set(0.1)

c.release(3);  //fade out the second player over 3 seconds
a.release(2);  //fade out the first over 2



//now connect a midi device (e.g. korg nanokontroll) and run the following
MIDIIn.connectAll;

MIDIdef.trace(true);  //start debug to see which cc numbers to use below
MIDIdef.trace(false);  //stop posting


//run this to start controlling the busses via midi
//note that each time you press cmd+. to stop these are cleared
(
MIDIdef.cc(\vol, {|val| ~volBus.set(val.linlin(0, 127, 0, 1))}, 2);  //slider1 = cc 2 on my nanokontrol - mapped to volume control bus
MIDIdef.cc(\rate, {|val| ~rateBus.set(val.linexp(0, 127, 1, 100))}, 3);  //slider2 = cc 3 on my nanokontrol - mapped to rate (note the linexp instead of linlin)
MIDIdef.cc(\mix, {|val| ~mixBus.set(val.linlin(0, 127, -1, 1))}, 4);  //slider3 = cc 4 on my nanokontroll - mapped to dry-wet mix

d= {
    var snd= PlayBuf.ar(b.numChannels, b, BufRateScale.kr(b), loop: 1);
    var efx= DelayC.ar(snd, 1, LFDNoise1.ar(In.kr(~rateBus)).range(0.01, 1).lag(0.1));
    var mix= XFade2.ar(snd, efx, In.kr(~mixBus));
    Splay.ar(mix)*In.kr(~volBus);
}.play;
)

d.release(2);


//and patterns can also read values from control busses

(
MIDIdef.cc(\vol, {|val| ~volBus.set(val.linlin(0, 127, 0, 1))}, 2);
MIDIdef.cc(\rate, {|val| ~rateBus.set(val.linlin(0, 127, -12, 12))}, 3);
MIDIdef.cc(\mix, {|val| ~mixBus.set(val.linexp(0, 127, 0.01, 0.5))}, 4);
Pdef(\mysong, PmonoArtic(\default,
    \degree, Pseq([0, 1, 2, Pfin(3, Pfunc({~rateBus.getSynchronous.postln}))], inf),
    \dur, 0.125,
    \amp, Pfunc({~volBus.getSynchronous}),
    \legato, Pfunc({~mixBus.getSynchronous}),
)).play;
)


//although another probably better way is to use a Pdefn
//so we free the busses as they are no longer needed...
~volBus.free;
~mixBus.free;
~rateBus.free;

//and then rewrite the example using three Pdefn
(
Pdefn(\vol, 0.1);  //default volume
Pdefn(\mix, 0);
Pdefn(\rate, 0);
MIDIdef.cc(\vol, {|val| Pdefn(\vol, val.lincurve(0, 127, 0, 1, 0))}, 2);
MIDIdef.cc(\rate, {|val| Pdefn(\rate, val.linlin(0, 127, -12, 12))}, 3);
MIDIdef.cc(\mix, {|val| Pdefn(\mix, val.linexp(0, 127, 0.01, 0.5))}, 4);
Pdef(\mysong, PmonoArtic(\default,
    \degree, Pseq([0, 1, 2, Pdefn(\rate)], inf),
    \dur, 0.125,
    \amp, Pdefn(\vol),
    \legato, Pdefn(\mix),
)).play;
)
```

links
==

code repository... https://sccode.org

how to install the steam vr plugin... https://github.com/redFrik/udk16-Immersive_Technologies/tree/master/udk170105#virtual-reality
