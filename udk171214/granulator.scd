s.boot;

(
~folder= "/Users/asdf/Desktop/loops";  //edit this to point to a FOLDER with many soundfiles
~buffers.do{|x| x.free};
~buffers= (~folder+/+"*").pathMatch.collect{|x| Buffer.readChannel(s, x, channels:[0])};
)

//--load a sampler (a synth definition for playing a short grain of a buffer)
(
SynthDef(\avgrain, {|out= 0, buf, rate= 1, offset= 0, time= 0.1, amp= 0.1, pan= 0|
	var env= EnvGen.ar(Env.sine(time), doneAction:2);
	var src= PlayBuf.ar(1, buf, rate*BufRateScale.ir(buf), 1, offset*BufFrames.ir(buf), 1);
	OffsetOut.ar(out, Pan2.ar(src*env*amp, pan));
}).add;
)

//test
(
Pdef(\granulator, Pbind(\instrument, \avgrain,
	\buf, Pseq(~buffers, inf),
	\dur, 0.125,
	\time, 0.125,
	\offset, 0,
	\amp, 0.75,
)).play;
)

//main granulator
(
Pdef(\granulator, Pbind(\instrument, \avgrain,
	\buf, Pwrand(~buffers, Pdefn(\weights, 1.dup(~buffers.size).normalizeSum), inf),
	\dur, Pdefn(\dur, 0.125/8),
	\time, Pkey(\dur)*Pdefn(\overlap, 2.5),
	\rate, Pdefn(\rate, Pwhite(1, 1, inf)),
	\offset, Pdefn(\offset, Pwhite(0.25, 0.5, inf)),
	\amp, 0.5*Pdefn(\amp, Pwhite(1, 1, inf))*Pwrand([1, 0], Pdefn(\density, [1, 0]), inf),
	\pan, Pgauss(0, Pdefn(\pan, 0.25)),
)).play;
)

//gui interface
(
var win= Window("granulator", Rect(10, 10, 400, 600));
var mul;
win.layout= VLayout(
	StaticText().string_("buffer weights / mixer:"),
	mul= MultiSliderView().value_(Pdefn(\weights).source).elasticMode_(1)
	.action_({|v| Pdefn(\weights, v.value.normalizeSum)}),
	StaticText().string_("scan through buffers:"),
	Slider().orientation_(\horizontal)
	.action_({|v| mul.valueAction_(({|i| abs(i/(~buffers.size-1)-v.value).max(0.001)** -2}!~buffers.size).normalizeSum)}),
	StaticText().string_("duration:"),
	Slider().orientation_(\horizontal).value_(Pdefn(\dur).source*10)
	.action= {|v| Pdefn(\dur, v.value*0.1+0.001)},
	StaticText().string_("rate range:"),
	RangeSlider().orientation_(\horizontal).lo_(0.5).hi_(0.5)
	.action= {|v| Pdefn(\rate, Pwhite(v.lo*2, v.hi*2, inf))},
	StaticText().string_("overlap:"),
	Slider().orientation_(\horizontal).value_(Pdefn(\overlap).source*0.05)
	.action= {|v| Pdefn(\overlap, v.value*20)},
	StaticText().string_("offset range:"),
	RangeSlider().orientation_(\horizontal).lo_(0.25).hi_(0.5)
	.action= {|v| Pdefn(\offset, Pwhite(v.lo, v.hi, inf))},
	StaticText().string_("density:"),
	Slider().orientation_(\horizontal).value_(1)
	.action= {|v| Pdefn(\density, [v.value, 1-v.value])},
	StaticText().string_("amplitude range:"),
	RangeSlider().orientation_(\horizontal).lo_(1).hi_(1)
	.action= {|v| Pdefn(\amp, Pwhite(v.lo, v.hi, inf))},
	StaticText().string_("panning spread:"),
	Slider().orientation_(\horizontal).value_(0.25)
	.action= {|v| Pdefn(\pan, v.value)}
);
CmdPeriod.doOnce({win.close});
win.front;
)
