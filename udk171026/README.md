pattern phasing
--------------------

this time we will experiment with a very (the most?) simple way of combining patterns... shifting one of them in time or space so that they play out of phase.

supercollider
==

```supercollider
s.boot;
s.meter;
s.scope;

Pseq  //seq stands for sequence and P for pattern
Pseq([1, 2, 3, 4, 5], inf);  //numbers in a sequence form a pattern, inf is the number of repetitions (inf= infinite)
Pseq([1, 2, 3, 4, 5], 3);  //this will repeate only 3 times and then the pattern is finished
Pseq([0.1, 4, 0.3, -3], inf);  //can be any numbers (and also symbols or other patterns)
Pseq([0, 1, Pseq([5, 4, 3], 2), -1], inf);  //nested patterns. result: 0, 1, 5, 4, 3, 5, 4, 3, -1, 0, 1, 5, 4 3, 5, 4, 3, -1, 0, 1 etc infinite times

Pdef  //def stands for definition. here used as a kind of global variable / placeholder
Pdef(\somename).play;  //start the Pdef named \somename
Pdef(\somename).stop;  //stop \somename
Pdef(\other).stop;     //refer to another Pdef called \other and stop it

PmonoArtic  //is our monophonic sequencer. it has a lot of built-in functionallity
a= PmonoArtic(\mysound).play;  //first argument must be synthdef name (here causes error because we did not load any sound called \mysound yet)
a.stop;  //stop the [broken] sequencer
//additional arguments are then key, value pairs. or key, pattern pairs. for example...
a= PmonoArtic(\mysound, \freq, 400, \amp, 0.2).play;  //start sequencer with mysound - frequency 400 and amplitude 0.2
a.stop;
a= PmonoArtic(\mysound, \freq, Pseq([400, 500, 600, 700], inf), \amp, 0.2).play;  //here instead of 400 static value we use a pattern
a.stop;
//below we will look at more PmonoArtic functions


//--load sounds and effects
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

//adding a third sequencer
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

//now patterns with different lengths (not divisable by 4 like above)
(
Pdef(\ping1, PmonoArtic(\avping,
    \freq, Pseq([300, 400, 600, 533], inf),
    \dur, 0.125,
    \legato, 0.2,
    \amp, Pseq([1, 1, 0.25, 0, 0.5], inf),  //5 in total - gives five amplitudes against four frequencies
)).play;
)

(
Pdef(\ping1, PmonoArtic(\avping,
    \freq, Pseq([300, 400, 600, 533, 800, 700], inf),  //now six frequencies here
    \dur, 0.125,
    \legato, 0.2,
    \amp, Pseq([1, 1, 0.25, 0, 0.5], inf),  //still 5 - gives five against six
)).play;
)

//six against five against four
(
Pdef(\ping1, PmonoArtic(\avping,
    \freq, Pseq([300, 400, 600, 533, 800, 700], inf),
    \dur, 0.125,
    \legato, Pseq([1, 0.2, 0.1, 0.05], inf),  //legato can also be a pattern - here four
    \amp, Pseq([1, 1, 0.25, 0, 0.5], inf),
)).play;
)

//six against five against four against three against two
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

//but quickly more complex with 7 instead of 6 frequencies
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

//lots of parameters and patterns
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


//--clapping music
//Steve Reich - Clapping Music https://www.youtube.com/watch?v=lzkOFJMI5i8
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


//--piano phase
//Steve Reich - Piano Phase https://www.youtube.com/watch?v=7P_9hDzG1i0
//simplified version
(
Pdef(\ping1).stop;
Pdef(\ping2).stop;
Pdef(\ping1, PmonoArtic(\avping,
\freq, Pseq([64, 66, 71, 73, 74, 66, 64, 73, 71, 66, 74, 73], inf).midicps,
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


- - -

links
==

Steve Reich - Clapping Music https://www.youtube.com/watch?v=QNZQzpWCTlA

poi https://www.youtube.com/watch?v=PHOO01O_pAQ
