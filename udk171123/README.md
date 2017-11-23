euclidean rhythms and text
--------------------

supercollider
==

first install a quark by typing this inside supercollider...

```supercollider
Quarks.install("Bjorklund");
```

and then restart supercollider (or just recompile the class library).

now copy and paste all the code below into a new document.

```supercollider
Bjorklund(8, 16);  //eight ones distributed over 16 beats
Bjorklund(9, 16);  //nine ones distributed over 16 beats
Bjorklund(7, 16);  //seven
Bjorklund(3, 16);  //three

Bjorklund(3, 8);  //tresillo
Bjorklund(5, 8);  //cinquillo
Bjorklund(9, 16);  //rumba
Bjorklund(9, 16).rotate(2);  //rotated rumba (shifted two steps)

//the Bjorklund class isn't so useful by its own. instead we want the pattern version Pbjorklund

a= Pbjorklund(9, 16).asStream;
a.next;  //run this line many times


s.boot;
s.meter;
s.scope;

//--load a sound and an effect
(
SynthDef(\avbang, {|out= 0, amp= 0.1, freq= 400, pan= 0, gate= 1, atk= 0.001, rel= 0.1, cur= -4, mod= 0|
    var env= EnvGen.ar(Env.asr(atk, amp, rel, cur), gate, doneAction:2);
    var src= SinOscFB.ar(freq, env.lag(mod), env);
    OffsetOut.ar(out, Pan2.ar(src*env, pan));
}).add;
SynthDef(\avverb, {|in= 0, out= 0, room= 1, drylevel= 0.1|
    var snd= In.ar(in, 2);
    var efx= Mix({AllpassN.ar(HPF.ar(snd, {50.0.rrand(100)}!2), 0.1, {0.005.rrand(0.1)}!2, 3*room, 0.15)}!8);
    efx= efx+Mix({AllpassN.ar(HPF.ar(efx, {100.0.rrand(300)}!2), 0.25, {0.05.exprand(0.25)}!2, 4*room, 0.1)}!4);
    Out.ar(out, snd*drylevel+efx);
}).add;
)

//--set the tempo
TempoClock.tempo= 118/60;  //bpm / 60 = beats per second

(
Pdef(\tick, PmonoArtic(\avbang,
    \freq, 1000,
    \dur, 1,
    \legato, 0.1,
    \amp, 0.5,
)).play;
Pdef(\eu, PmonoArtic(\avbang,
    \freq, 400,
    \dur, 0.25,
    \amp, Pbjorklund(9, 16)*0.5,  //check this
)).play;
)

//same thing but longer to write
(
Pdef(\eu, PmonoArtic(\avbang,
    \freq, 400,
    \dur, 0.25,
    \amp, Pseq([1, 0, 1, 1, 0, 1, 0, 1, 0, 1, 1, 0, 1, 0, 1, 0], inf)*0.5,  //this is what Pbjorklund automatically gives us
)).play;
)

//playing around with the 'k' parameter
(
Pdef(\eu, PmonoArtic(\avbang,
    \freq, Pstutter(16, Pseq([400, 500], inf)),  //cycle two different frequencies each  lasting 16 beats
    \dur, 0.25,
    \amp, Pbjorklund(7, 16)*0.5,  //try to change 7 to something else here like 8, 9, 5, 6 etc and observe the result
)).play;
)

//change base ('n') to 8
(
Pdef(\eu, PmonoArtic(\avbang,
    \freq, Pstutter(8, Pseq([400, 500], inf)),  //note now 8here
    \dur, 0.25,
    \amp, Pbjorklund(5, 8)*0.5,  //note now 8 here (cinquillo)
)).play;
)

//change base to 32
(
Pdef(\eu, PmonoArtic(\avbang,
    \freq, Pstutter(32, Pseq([400, 500], inf)),  //note now 32 here
    \dur, 0.25,
    \amp, Pbjorklund(15, 32)*0.5,  //note now 32 here. try to change 15 to something else
)).play;
)

//of course the base ('n') can be anything but then you will get less 'musical' patterns - try!

//euclidean pattern in combination with frequency pattern
(
Pdef(\eu, PmonoArtic(\avbang,
    \freq, Pseq([100, 200, 300, 400, 500, 600, 700, 800], inf),  //pattern - 8 frequencies divide evenly for 32 beats
    \dur, 0.25,
    \amp, Pbjorklund(15, 32)*0.5,
)).play;
)

(
Pdef(\eu, PmonoArtic(\avbang,
    \freq, Pseq([100, 200, 300, 400, 500, 600, 700, 800, 100], inf),  //added one extra - now phasing
    \dur, 0.25,
    \amp, Pbjorklund(15, 32)*0.5,
)).play;
)

(
Pdef(\eu, PmonoArtic(\avbang,
    \freq, Pseq([100, 200, 300, 400, 500, 600, 700, 800, 100], inf),
    \dur, 0.25,
    \rel, 0.2,  //slightly longer duration
    \amp, Pbjorklund(17, 32)*0.5,  //changed from 15 to 17
)).play;
)

(
Pdef(\eu, PmonoArtic(\avbang,
    \freq, Pseq([100, 200, 300, 400, 500, 600, 700, 800, 100], inf),
    \dur, 0.25,
    \mod, Pseg(Pseq([0, 1], inf), 32).trace,  //slowly changing modulator
    \rel, 0.2,
    \amp, Pbjorklund(17, 32)*0.5,
    \pan, Pseq([-0.5, 0.5], inf),  //with added panning
)).play;
)

//--effects
b.free; b= Synth(\avverb, [\in, 60, \room, 10]);  //reverb on bus 60

(
Pdef(\eu).quant= 4;
Pdef(\eu, PmonoArtic(\avbang,
    \freq, Pseq([100, 200, 300, 400, 500, 600, 700, 800, 100], inf),
    \dur, 0.25,
    \mod, Pseg(Pseq([0, 1], inf), 32),
    \rel, 0.2,
    \amp, Pbjorklund(17, 32)*0.5,
    \pan, Pseq([-0.5, 0.5], inf),
    \out, Pseq([0, 60], inf),  //send every second note to the reverb
)).play;
)

Pdef(\tick).stop;  //stop the annoying metronome

//add a second sound
(
Pdef(\eu2).quant= 4;
Pdef(\eu2, PmonoArtic(\avbang,
    \freq, Pseq([400, 500, 200], inf),
    \dur, 0.25,
    \amp, Pbjorklund(9, 16)*0.5,
)).play;
)

//a third sound (kickdrum)
(
Pdef(\eu3).quant= 4;
Pdef(\eu3, PmonoArtic(\avbang,
    \freq, 50,
    \dur, 1,
    \atk, 0,
    \legato, 0.01,
    \amp, 1,
)).play;
)

(
Pdef(\eu4).quant= 4;
Pdef(\eu4, PmonoArtic(\avbang,
    \freq, Pstutter(8, Pseq([1, 2, 3, 4, 5, 6, 7, 8], inf))*100,
    \dur, 0.25,
    \amp, Pbjorklund(5, 16)*0.75,
    \out, 60,
)).play;
)

//simplify
(
Pdef(\eu, PmonoArtic(\avbang,
    \freq, Pseq([400, 200], inf),
    \dur, Pbjorklund2(9, 16)*0.25,
    \amp, 0.4,
)).play;
Pdef(\eu2, PmonoArtic(\avbang,
    \freq, 100,
    \dur, 1,
    \legato, 0.4,
    \amp, 0.5,
)).play;
)

Pdef(\eu).stop;
Pdef(\eu2).stop;
Pdef(\eu3).stop;
Pdef(\eu4).stop;
b.free;  //turn off the reverb synth



//--recording
(
~buffers.do{|x| x.free};  //clear all
~buffers= {Buffer.alloc(s, s.sampleRate*1)}.dup(4);  //create four buffers, each 1sec long
SynthDef(\avrec, {|buf|
    RecordBuf.ar(Limiter.ar(SoundIn.ar), buf, loop:0, doneAction:2);
}).add;
SynthDef(\avplay, {|out= 0, buf, rate= 1, offset= 0, atk= 0.005, amp= 0.1, rel= 0.1, cur= -4, gate= 1, pan= 0|
    var env= EnvGen.ar(Env.asr(atk, 1, rel, cur), gate, doneAction:2);
    var src= PlayBuf.ar(1, buf, rate*BufRateScale.ir(buf), 1, offset*BufFrames.ir(buf), 1);
    OffsetOut.ar(out, Pan2.ar(src*amp*env, pan));
}).add;
)

//--change the tempo
TempoClock.tempo= 112/60;  //bpm / 60 = beats per second

//run these four lines one by one and make some noise (e.g. whistle for 1sec) in the microphone meanwhile
Synth(\avrec, [\buf, ~buffers[0]]);  //record into the first buffer
Synth(\avrec, [\buf, ~buffers[1]]);  //record into the second buffer
Synth(\avrec, [\buf, ~buffers[2]]);  //third
Synth(\avrec, [\buf, ~buffers[3]]);  //fourth

//this should start playing the first sound you recorded
(
Pdef(\play0).quant= 4;
Pdef(\play0, PmonoArtic(\avplay,
    \buf, ~buffers[0],  //which buffer to play from
    \dur, 0.25,
    \legato, 0.5,
    \amp, Pbjorklund(7, 16),
)).play;
)

(
Pdef(\play0, PmonoArtic(\avplay,
    \buf, ~buffers[0],
    \dur, 0.25,
    \offset, Pseq([0, 0.5], inf),  //using offset (start position in buffer 0-1) to create variation
    \legato, 0.5,
    \amp, Pbjorklund(7, 16),
)).play;
)

//adding a second sequencer playing from the second buffer
(
Pdef(\play1).quant= 4;
Pdef(\play1, PmonoArtic(\avplay,
    \buf, ~buffers[1],
    \dur, 0.25,
    \offset, Pseq([0, 0.5, 0.25], inf),
    \legato, 0.5,
    \rate, Pstutter(4, Pseq([0.5, 1, 1, 2], inf)),  //playback rate as a pattern - experiment
    \amp, Pbjorklund(4, 16),  //steady pulse
)).play;
)

//a third player
(
Pdef(\play2).quant= 4;
Pdef(\play2, PmonoArtic(\avplay,
    \buf, ~buffers[2],
    \dur, 0.25,
    \offset, 0,
    \legato, 0.5,
    \rel, Pseg(Pseq([0.1, 0.9], inf), 24),  //slowly change over time (48 beats)
    \amp, Pbjorklund(7, 16, inf, 4),  //notice the offset here (4). will 'rotate' the pattern
    \pan, Pseq([0.5, 0, -0.5], inf),
)).play;
)

//the fourth player
(
Pdef(\play3).quant= 4;
Pdef(\play3, PmonoArtic(\avplay,
    \buf, ~buffers[3],
    \dur, 0.25,
    \offset, 0,
    \legato, 0.1,
    \rate, Pseg(Pseq([1, 1.9], inf), 16),  //slowly change pitch
    \amp, Pbjorklund(9, 16, inf, 8),  //notice the offset here (8)
    \pan, Pseq([0.5, -0.5], inf),
)).play;
)

Pdef(\play0).stop;
Pdef(\play1).stop;
Pdef(\play2).stop;
Pdef(\play3).stop;


//one can also use a pattern for which buffer to play
(
Pdef(\play0, PmonoArtic(\avplay,
    \buf, Pseq([~buffers[0], ~buffers[1], ~buffers[2], ~buffers[3]], inf),
    \dur, 0.25,
    \legato, 0.75,
    \rate, 0.9,
    \amp, Pbjorklund(9, 16),
)).play;
)

//another common technique is to play different sound for 1 and 0 in the euclidean pattern
//there are many ways to accomplish this - here's one
(
Pdef(\play1).stop;
Pdef(\play0, PmonoArtic(\avplay,
    \buf, Pswitch([~buffers[0], ~buffers[1]], Pbjorklund(9, 16)),  //0 or 1 select different buffers
    \dur, 0.25,
    \legato, 0.75,
    \rate, 1.1,
    \amp, 1,  //note the static 1 here
)).play;
)

//another technique to do the same thing using two players...
(
Pdef(\play0, PmonoArtic(\avplay,
    \buf, ~buffers[0],
    \dur, 0.25,
    \legato, 0.75,
    \rate, 1.1,
    \amp, Pbjorklund(9, 16),
)).play;
Pdef(\play1, PmonoArtic(\avplay,
    \buf, ~buffers[1],
    \dur, 0.25,
    \legato, 0.75,
    \rate, 1.1,
    \amp, 1-Pbjorklund(9, 16),  //note the inversion here (1-)
)).play;
)

Pdef(\play0).stop;
Pdef(\play1).stop;

//there is also the duration variant Pbjorklund2
(
Pdef(\play0, PmonoArtic(\avplay,
    \buf, ~buffers[0],
    \dur, Pbjorklund2(9, 16)*0.25,  //results in the duration pattern [0.5, 0.25, 0.5, 0.5, 0.5, 0.25, 0.5, 0.5, 0.5]
    \legato, 0.75,
    \amp, 1,  //note static 1 here
)).play;
)

//soundscape generator with long attack and release
(
Pdef(\play0, PmonoArtic(\avplay,
    \buf, ~buffers[0],  //also try Pseq([~buffers[0], ~buffers[1], ~buffers[2]], inf),
    \rate, Pseq([-1, -2, -3, -0.5], inf),  //all backwards at different rates
    \dur, Pbjorklund2(9, 16)*3,  //long durations
    \offset, Pseq([0, 0.33, 0.67], inf),  //vary starting positions according to a pattern
    \legato, 0.9,
    \atk, 2,  //attack time
    \rel, 13,  //release time
    \pan, Pseq([0.25, 0.5, 0], inf),
    \amp, 0.8,
)).play;
)

Pdef(\play0).stop;  //should stop eventually
```

also try adapting some examples from [last week](https://github.com/redFrik/udk18-Discrete_Structures/tree/master/udk171116#supercollider) to use Pbjorklund.

- - -

unity3d
==

note: this will also work really well in 2D.

* start unity and create a new **3D** project. give it a name (here text)
* create a new script by selecting Assets / Create / C# Script
* give the script the name 'Text' by typing under the white icon
* double click the white C# script icon to open it in MonoDevelop
* copy and paste in the code here below replacing what was there originally

```cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Text : MonoBehaviour {
    public Transform prefab;    //holds our initial gameobject
    List<Transform> clones = new List<Transform> ();
    int num = 30;    //set number of clones
    void Start () {  //do once when starting
        for (int i = 0; i < num; i++) {
            clones.Add (Instantiate (prefab, new Vector3 (0, 0, 0), Quaternion.identity));
        }
        prefab.gameObject.SetActive (false);  //hide prefab object
    }
    void RotateText(Transform clone, int shift) {
        string instr = clone.GetComponent<TextMesh>().text;
        string outstr = "";
        for (int j = 0; j < instr.Length; j++) {
            outstr = outstr + (instr [(j + shift) % instr.Length]);
        }
        clone.GetComponent<TextMesh>().text = outstr;
    }
    void Update () {  //do every frame - many times per second
        int i = 1;    //keep track of clone index

        //variant 1 - turn around in a circle
        foreach(Transform ct in clones) {
            ct.localEulerAngles = new Vector3 (0, 0, i*1.0F/num*360.0F);
            i++;
        }

        //variant 2 - scale up
        //        foreach(Transform ct in clones) {
        //            ct.localScale = new Vector3 (i*0.25F, i*0.25F, 1);
        //            i++;
        //        }

        //variant 3 - positioning
        //        foreach(Transform ct in clones) {
        //            ct.localPosition = new Vector3 (i, i%3, 1);
        //            i++;
        //        }

        //variant 4 - more positioning
        //        foreach(Transform ct in clones) {
        //            ct.localPosition = new Vector3 (i%7*1.5F, i%9*0.4F, 1);
        //            i++;
        //        }

        //variant 5 - animation position
        //        foreach (Transform ct in clones) {
        //            ct.localPosition = new Vector3 ((i % (Time.frameCount*0.01F%5) * 8.0F)-18, i % (Time.frameCount+(i*0.1F)%12) * 0.2F, 1);
        //            i++;
        //        }

        //variant 6 - animation scaling
        //        foreach (Transform ct in clones) {
        //            ct.localScale = new Vector3 ((Time.frameCount+(i*10))*0.01F%5, i % (Time.frameCount+(i*0.3F)%8)*0.25F, 1);
        //            i++;
        //        }

        //variant 7 - animation rotation
        //        foreach (Transform ct in clones) {
        //            ct.localEulerAngles = new Vector3 (0, 0, (Time.frameCount+(i*0.9F))*0.01F*360.0F);
        //            i++;
        //        }

        //variant 7 - more animation rotation
        //        foreach (Transform ct in clones) {
        //            ct.localEulerAngles = new Vector3 (0, 0, (Time.frameCount*(i*-0.0001F))*360.0F);
        //            i++;
        //        }

        //now combine form above and change some values
        //        foreach (Transform ct in clones) {
        //            ct.localScale = new Vector3 ((Time.frameCount+(i*10))*0.01F%5, i % (Time.frameCount+(i*0.3F)%8)*0.25F, 1);
        //            ct.localEulerAngles = new Vector3 (0, 0, (Time.frameCount*(i*-0.0001F))*360.0F);
        //            i++;
        //        }
        
        //special - string reordering - can also go in Start to do it once
        //      for (int j = 0; j < num; j++) {
        //          RotateText(clones[j], 1);
        //      }
    }
}
```

* save and switch back to unity
* in the upper left hierachy window, click to select the 'Main Camera'
* attach the script to the camera by selecting Component / Scripts / Text (or just drag and drop the script onto the camera)
* select GameObject / 3D Object / 3D Text. this game object will become our prefab from which the clone will be made
* edit the 3D Text object in its inspector and set the text to something (here 'Euclidean').
* also set Anchor to 'Middle center'

![00anchor](00anchor.png?raw=true "00anchor")

* select the main camera and in the inspector click on the little circle next to prefab. select the New Text by doubleclicking in the dialog that pops up (or just drag the sphere onto the prefab variable slot)
* press play and you should see a circle of words

![01circle](01circle.png?raw=true "01circle")

now play around with the code. change things and try the different variants by commenting/uncommenting out different blocks of code. try changing the number of clones created.

also change the background, text, colours etc.

![02animation](02animation.png?raw=true "02animation")

links
==

https://www.youtube.com/watch?v=I8uk9pRFmms

Godfried Toussaint - [The Euclidean Algorithm Generates Traditional Musical Rhythms](http://cgm.cs.mcgill.ca/~godfried/publications/banff.pdf)
