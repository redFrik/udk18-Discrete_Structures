techno and mirrors
--------------------


supercollider
==

```supercollider
s.boot;
s.meter;
s.scope;

//--load a few sounds
(
SynthDef(\avbd, {|out= 0, amp= 0.1, freq= 60, pan= 0, atk= 0.001, rel= 0.1, mod= 0, cur= 0, gate= 1|
    var env= EnvGen.ar(Env.asr(atk, 1, rel, cur), gate, doneAction:2);
    var src= SinOsc.ar(freq*(2-env.lag3), 0.5pi, mod+1).tanh*amp;
    OffsetOut.ar(out, Pan2.ar(src*env, pan));
}).add;
SynthDef(\avhh, {|out= 0, amp= 0.1, pan= 0, atk= 0.001, rel= 0.1, cur= -5, mod= 0, gate= 1|
    var env= EnvGen.ar(Env.adsr(atk, atk, 0.5, rel, 1, cur), gate, doneAction:2);
    var src= RHPF.ar(ClipNoise.ar(amp), 1000, (10-mod).max(0.1))*LFTri.ar(1000);
    OffsetOut.ar(out, Pan2.ar(src*env, pan));
}).add;
SynthDef(\avbass, {|out= 0, amp= 0.1, freq= 100, pan= 0, atk= 0.01, rel= 0.2, cur= -4, mod= 0, gate= 1|
    var env= EnvGen.ar(Env.adsr(atk, atk, 0.75, rel, 1, cur), gate, doneAction:2);
    var src= BLowPass4.ar(Saw.ar(freq, amp), freq+(mod+1*100).min(900), 1-env+0.1);
    OffsetOut.ar(out, Pan2.ar(src*env, pan));
}).add;
SynthDef(\avlead, {|out= 0, amp= 0.1, freq= 500, pan= 0, atk= 0.01, rel= 0.1, cur= -4, gate= 1|
    var env= EnvGen.ar(Env.asr(atk, 1, rel, cur), gate, doneAction:2);
    var src= Pulse.ar(freq, 1-env.lag(1)*0.5, amp);
    OffsetOut.ar(out, Pan2.ar(src*env, pan));
}).add;
SynthDef(\avswop, {|out= 0, amp= 0.1, start= 5000, end= 500, mod= 0, pan= 0, atk= 0.01, rel= 4, cur= -4, gate= 1|
    var env= EnvGen.ar(Env.asr(atk, 1, rel, cur), gate, doneAction:2);
    var src= BPF.ar(WhiteNoise.ar(amp+SinOsc.ar(mod, 0.5pi, amp*0.25)), XLine.kr(start, end, 20), 2-env);
    OffsetOut.ar(out, Pan2.ar(src*env, pan+SinOsc.kr(0.1, 0, 0.3)));
}).add;
)

//--set the tempo
TempoClock.tempo= 117/60;  //bpm / 60 = beats per second


//simple bassdrum (kick)
(
Pdef(\bd).quant= 4;
Pdef(\bd, PmonoArtic(\avbd,
    \freq, 50,
    \dur, 1,
    \legato, 0.1,
    \amp, 0.5,
)).play;
)

//simple snaredrum (here using the avhh synth though)
(
Pdef(\sn).quant= 4;
Pdef(\sn, PmonoArtic(\avhh,
    \dur, 0.5,
    \mod, 8,
    \rel, 0.3,
    \legato, 0.1,
    \amp, Pseq([0, 0.3], inf),
)).play;
)

(
Pdef(\hh).quant= 4;
Pdef(\hh, PmonoArtic(\avhh,
    \dur, 0.25,
    \legato, 0.1,
    \amp, Pseq([0.05, 0.1], inf),
)).play;
)

Pdef(\hh).stop;
Pdef(\sn).stop;
Pdef(\bd).stop;

//a general tendency in music is that lower instruments play less often but louder
//and higher pitched (smaller) instruments play faster but softer
//most music in our culture today is also based on 4, 8, 16, 32 etc

(
Pdef(\bd).quant= 4;
Pdef(\bd, PmonoArtic(\avbd,
    \freq, 50,
    \dur, 1,
    \legato, 0.1,
    \amp, Pseq([1, 1, 1, 1, 1, 1, 1, 0], inf)*0.5,  //added a pattern with a single rest - try changing the number of 1s and 0s
)).play;
)

//notice how combinations of 7+1, 6+2, 15+1, 14+2 etc sounds 'logic'
//while other combiantions (like 6+1) sounds 'odd' and slightly irregular to our ears

(
Pdef(\bd).quant= 4;
Pdef(\bd, PmonoArtic(\avbd,
    \freq, 50,
    \dur, 1,
    \legato, 0.1,
    \amp, Pseq([1, 0.5, 1, 0.5, 1, 0.5, 1, 0], inf)*0.5,  //every second louder to further emphasise the subdivition of 2
)).play;
)

(
Pdef(\bd).quant= 4;
Pdef(\bd, PmonoArtic(\avbd,
    \freq, 50,
    \dur, 1,
    \legato, 0.1,
    \amp, Pseq([0.5, 1, 0.5, 1, 0.5, 1, 0.5, 0], inf)*0.5,  //compare with this
)).play;
)

(
Pdef(\bd).quant= 4;
Pdef(\bd, PmonoArtic(\avbd,
    \freq, 50,
    \dur, 1,
    \legato, 0.1,
    \amp, Pseq([1, 1, 1, 1, 1, 1, 1, 0], inf)*Pseq([0.25, 0.5], inf),  //another way to write the same thing
)).play;
)

//instead of amplitude patterns and a rest to mark two bars (8 beats in 4/4) we can use the duration pattern to make a rest
(
Pdef(\bd).quant= 4;
Pdef(\bd, PmonoArtic(\avbd,
    \freq, 50,
    \dur, Pseq([1, 1, 1, 1, 1, 1, 2], inf),  //duration pattern - should add up to 8
    \legato, 0.1,
    \amp, 0.5,
)).play;
)

(
Pdef(\bd).quant= 4;
Pdef(\bd, PmonoArtic(\avbd,
    \freq, 50,
    \dur, Pseq([1, 1, 1, 1, 1, 1, 1.5, 0.5], inf),  //duration pattern - hickup
    \legato, 0.1,
    \amp, 0.5,
)).play;
)

(
Pdef(\bd).quant= 4;
Pdef(\bd, PmonoArtic(\avbd,
    \freq, 50,
    \dur, Pseq([1, 1, 1, 1, 1, 1, 0.75, 0.75, 0.5], inf),  //experiment
    \legato, 0.1,
    \amp, 0.5,
)).play;
)

//same thing but written using amplitude pattern (notice the overall shorter duration)
(
Pdef(\bd).quant= 4;
Pdef(\bd, PmonoArtic(\avbd,
    \freq, 50,
    \dur, 0.25,
    \legato, 0.1,
    \amp, Pseq([1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0], inf)*0.5,  //longwinded
)).play;
)

//or again same thing but using nested patterns
(
Pdef(\bd).quant= 4;
Pdef(\bd, PmonoArtic(\avbd,
    \freq, 50,
    \dur, 0.25,
    \legato, 0.1,
    \amp, Pseq([Pseq([1, 0, 0, 0], 6), 1, 0, 0, 1, 0, 0, 1, 0], inf)*0.5,  //nested
)).play;
)

//fractions are common in music
(
Pdef(\bd).quant= 4;
Pdef(\bd, PmonoArtic(\avbd,
    \freq, 50,
    \dur, Pseq([1, 1, 1, 1, 1, 1, 2/3, 2/3, 2/3], inf),
    \legato, 0.1,
    \amp, 0.5,
)).play;
)

(
Pdef(\bd).quant= 4;
Pdef(\bd, PmonoArtic(\avbd,
    \freq, 50,
    \dur, Pseq([1, 1, 1, 1, 1, 1, 1/3, 1/3, 1/3, 1/3, 1/3, 1/3], inf),  //play six 1/3 in the place of two. 6*(1/3)= 2
    \legato, 0.1,
    \amp, 0.5,
)).play;
)

(
Pdef(\bd).quant= 4;
Pdef(\bd, PmonoArtic(\avbd,
    \freq, 50,
    \dur, Pseq([1, 1, 2/3, 1/3, 1, 1, 1, 2/3, 1/3, 1/3, 1/3, 1/3], inf),
    \legato, 0.1,
    \amp, 0.5,
)).play;
)

[1, 1, 2/3, 1/3, 1, 1, 1, 2/3, 1/3, 1/3, 1/3, 1/3].sum

//now in combination with amplitudes
(
Pdef(\bd).quant= 4;
Pdef(\bd, PmonoArtic(\avbd,
    \freq, 50,
    \dur, Pseq([1, 1, 2/3, 1/3, 1, 1, 1, 2/3, 1/3, 1/3, 1/3, 1/3], inf),
    \legato, 0.1,
    \amp, Pseq([1, 1, 1, 0.5, 1, 1, 1, 1, 0.5, 0.25, 0.5, 0.75], inf)*0.5,
)).play;
)

//adding a second instrument
(
Pdef(\hh).quant= 4;
Pdef(\hh, PmonoArtic(\avhh,
    \dur, 0.5,
    \legato, 0.1,
    \amp, Pseq([0, 1], inf)*0.25,
    \pan, Pseq([0.3, 0, -0.3], inf),
)).play;
)

(
Pdef(\hh).quant= 4;
Pdef(\hh, PmonoArtic(\avhh,
    \dur, Pseq([0.25, 0.25, 0.5], inf),
    \legato, 0.1,
    \amp, Pseq([0, 1], inf)*0.25,
    \pan, Pseq([0.3, 0, -0.3], inf),
)).play;
)

(
Pdef(\hh).quant= 4;
Pdef(\hh, PmonoArtic(\avhh,
    \dur, Pseq([0.25, 0.25, 0.5], inf),
    \legato, 0.1,
    \amp, Pseq([0.25, 1], inf)*0.25,
    \pan, Pseq([0.3, 0, -0.3], inf),
)).play;
)

(
Pdef(\hh).quant= 4;
Pdef(\hh, PmonoArtic(\avhh,
    \dur, Pseq([0.25, 0.25, 0.5], inf),
    \legato, Pseq([0.1, 0.2, 0.1, 0.1], inf),  //small variation can liven things up
    \amp, Pseq([0.25, 1], inf)*0.25,
    \pan, Pseq([0.3, 0, -0.3], inf),
)).play;
)

//adding a third instrument
(
Pdef(\bass).quant= 4;
Pdef(\bass, PmonoArtic(\avbass,
    \freq, 100,
    \dur, 0.25,
    \rel, 1,
    \legato, 0.5,
    \amp, Pseq([1, 0], inf)*0.5,
)).play;
)

//Pstutter(n, pattern)
(
Pdef(\bass).quant= 4;
Pdef(\bass, PmonoArtic(\avbass,
    \freq, Pseq([100, 50, 50], inf),
    \dur, 0.25,
    \mod, Pstutter(8, Pseq([0, 1, 2, 3, 4, 5, 4, 3, 2, 1], inf)),  //Pstutter repeats each value in a pattern x times
    \rel, 1,
    \legato, 0.5,
    \amp, Pseq([1, 0], inf)*0.25,
)).play;
)

//Pseg(levels, durs)
(
Pdef(\bass).quant= 4;
Pdef(\bass, PmonoArtic(\avbass,
    \freq, Pseq([100, 50, 50, 120], inf),
    \dur, 0.25,
    \mod, Pseg(Pseq([0, 10], inf), 16),  //Pseg makes ramping patterns
    \rel, 0.3,  //shorter release
    \legato, 0.5,
    \amp, Pseq([0.5, 0.5, 0.5, 0.25], inf),
)).play;
)

(
Pdef(\bass).quant= 4;
Pdef(\bass, PmonoArtic(\avbass,
    \freq, Pseq([100, 50, 50, 120], inf)*Pseq([Pseq([1], 64), Pseq([1.25], 64)], inf),  //transpose each 64*0.25/4= 4 bar
    \dur, 0.25,
    \mod, Pseg(Pseq([0, 10], inf), 16),
    \rel, 0.3,
    \legato, 0.5,
    \amp, Pseq([0.5, 0.5, 0.5, 0.25], inf),
)).play;
)

//more instruments
(
Pdef(\arp1).quant= 4;
Pdef(\arp2).quant= 4;
Pdef(\arp3).quant= 4;
Pdef(\arp1, PmonoArtic(\avlead,
    \freq, Pseq([100, 120, 400, 300], inf),
    \dur, 0.25,
    \legato, 0.2,
    \amp, Pseq([1, 1, 1, 0], inf)*Pseg(Pseq([0, 0.1], inf), 48),
    \pan, -0.3,
)).play;
Pdef(\arp2, PmonoArtic(\avlead,
    \freq, Pseq([100, 120, 300, 50, 120, 300], inf)*2,
    \dur, 0.25,  //0.25= 1/4
    \legato, 0.2,
    \amp, Pseq([1, 1, 0], inf)*Pseg(Pseq([0, 0.1], inf), 44),
    \pan, 0.3,
)).play;
Pdef(\arp3, PmonoArtic(\avlead,
    \freq, Pseq([100, 120, 300, 400, 120, 120, 120], inf)*2,
    \dur, 0.125,  //0.125= 1/8
    \legato, 0.2,
    \amp, Pseq([1, 0], inf)*0.1,
)).play;
)

//note quant= 1 so not bound to 4/4 bar
(
Pdef(\swop).quant= 1;
Pdef(\swop, PmonoArtic(\avswop,
    \dur, 32,
    \legato, 0.8,
    \amp, Pseq([1, 0], inf)*0.05,
)).play;
)

Pdef(\swop).stop

//taking away (muting) instruments is a common way to build form and structure
Pdef(\bass).stop;
Pdef(\bd).stop;
Pdef(\hh).stop;

Pdef(\bass).play;
Pdef(\bd).play;
Pdef(\hh).play;

Pdef(\arp1).stop;
Pdef(\arp2).stop;
Pdef(\arp3).stop;



//something different
(
Pdef(\bd).quant= 4;
Pdef(\bd, PmonoArtic(\avbd,
    \freq, 50,
    \dur, 1,
    \legato, 0.1,
    \amp, 0.5
    )).play;
    Pdef(\hh).quant= 4;
    Pdef(\hh, PmonoArtic(\avhh,
    \mod, Pseq([0, 0, 9, 0], inf),
    \dur, 0.25,
    \legato, Pseg(Pseq([0.05, 0.2, 0.05], inf), 32).trace,
    \amp, 0.1,
    \pan, Pseq([0.5, 0, -0.5, 0], inf),
    )).play;
    Pdef(\bass).quant= 4;
    Pdef(\bass, PmonoArtic(\avbass,
    \freq, 50,
    \amp, 0.3,
    \mod, Pseq([0, 0, 0, 0, 0, 0, 0, 1], inf),
    \pan, Pseq([-0.7, 0.7], inf),
)).play;
)

//yet different
(
Pdef(\bd).quant= 4;
Pdef(\bd, PmonoArtic(\avbd,
    \freq, 75,
    \dur, 1,
    \legato, 0.1,
    \mod, Pseq([0, 0, 0.5, 0], inf),
    \amp, 0.5
)).play;
Pdef(\hh).quant= 4;
Pdef(\hh, PmonoArtic(\avhh,
    \mod, Pseq([0, 0, 9, 0], inf),
    \dur, 1/8,
    \legato, 0.05,
    \rel, Pseg(Pseq([0.1, 0.2], inf), 32),
    \amp, Pseq([0.05, 0.1], inf),
    \mod, Pseg(Pseq([0, 16], inf), Pseq([16, 0], inf)),
    \pan, Pseq([0.5, 0, -0.5, 0], inf),
)).play;
Pdef(\bass).quant= 4;
Pdef(\bass, PmonoArtic(\avbass,
    \freq, Pseq([50, 75], inf)*Pstutter(4, Pseq([1, 1, 1, 1, 1.25, 1.25, 1.5, 1.5], inf)),
    \dur, Pseq([0.85, 0.15], inf),
    \amp, 0.3,
    \rel, 0.3,
    \legato, Pseq([1, 0.1], inf),
    \mod, Pseq([0.5, 0, 0, 6, 0, 0, 0, 1], inf),
    \pan, Pseq([-0.3, 0.3], inf),
)).play;
)

(
Pdef(\swop).quant= 1;
Pdef(\swop, PmonoArtic(\avswop,
    \dur, 8,
    \start, 300,
    \end, 3000,
    \atk, 4,
    \rel, 9,
    \mod, TempoClock.beatDur*3,
    \amp, Pseq([0.1, 0, 0], inf),
)).play;
)

(
Pdef(\arp3).quant= 4;
Pdef(\arp3, PmonoArtic(\avlead,
    \freq, Pseq([75, 150], inf)*Pseq([Pseq([1, 2, 2, 2, 3], 12), Pseq([1, 1, 1, 2, 4], 4)], inf),
    \dur, 0.25,
    \legato, Pseg(Pseq([0.05, 0.2], inf), Pseq([24, 0], inf)),
    \atk, 0.1,
    \amp, 0.1,
    \pan, Pseq([-0.3, 0, 0.3], inf),
)).play;
)

(
Pdef(\arp2).quant= 4;
Pdef(\arp2, PmonoArtic(\avlead,
    \freq, 75*4,
    \dur, 0.25,
    \legato, 0.2,
    \atk, 0.1,
    \amp, 0.05,
    \pan, Pseq([-0.4, 0.4], inf),
)).play;
)

(
Pdef(\arp1).quant= 4;
Pdef(\arp1, PmonoArtic(\avlead,
    \freq, Pseq([Pseq([75], 8*8*3), Pseq([150], 8*8)], inf)*Pstutter(8, Pseq([1, 2, 3, 3, 3, 4], inf)),
    \dur, 0.5,
    \legato, 0.12,
    \atk, 0.2,
    \rel, 0.5,
    \amp, 0.1,
    \pan, Pseq([0.1, -0.1], inf),
)).play;
)

Pdef(\hh).stop;
Pdef(\bd).stop;
Pdef(\bass).stop;
Pdef(\arp2).stop;
Pdef(\arp1).stop;
Pdef(\arp3).stop;
Pdef(\swop).stop;
```
- - -

things to explore:

* rewrite to use `\degree`, `\scale`, `\octave` and `\gtranspose` instead of direct `\freq`
* try to copy effect synths from previously and send output of some pdef to them using `\out`
* change the synthdefs while it is playing (carefull with headphones - might be suddenly loud)
* rewrite the synthdefs to use soundfile samples...

```supercollider
(
~avbd.free; ~avbd= Buffer.readChannel(s, "/Users/asdf/soundfiles/Dirt-Samples/808bd/BD0010.WAV", channels:[0]);
SynthDef(\avbd, {|out= 0, amp= 0.1, freq= 60, pan= 0, atk= 0.001, rel= 0.1, mod= 0, cur= 0, gate= 1|
    var env= EnvGen.ar(Env.asr(atk, 1, rel, cur), gate, doneAction:2);
    var rate= freq.linlin(60, 90, 0.9, 1.2);  //arbitrary scaling
    var src= PlayBuf.ar(1, ~avbd, rate)*amp;
    OffsetOut.ar(out, Pan2.ar(src*env, pan));
}).add;
)
```

* search for "minimal techno" on youtube (e.g. https://www.youtube.com/watch?v=NL6JHjlaqKA ) and listen analytically. can you see the [pattern] code for your inner eye?
* build sections with blocks of code you evaluate. call them something like intro, beats, more, break, more, outro. a very simple example can look like this...

```supercollider
(
//intro - stop everything except bd
Pdef(\hh).stop;
Pdef(\bd).play;
Pdef(\bass).stop;
Pdef(\arp2).stop;
)

(
//more - adding hh and bass
Pdef(\hh).play;
Pdef(\bass).play;
)

(
//break
Pdef(\bass).stop;
)

(
//more2
Pdef(\bass).play;
Pdef(\arp2).play;
)

(
//break2
Pdef(\bass).stop;
Pdef(\hh).stop;
Pdef(\arp2).play;
)

(
//full
Pdef(\bd).play;
Pdef(\bass).play;
Pdef(\hh).play;
Pdef(\arp2).play;
)
```

unity3d
==

playing with render textures and feedback...



- - -

links
==

Unity Tutorial: Render Textures https://www.youtube.com/watch?v=pA7ZC8owaeo

