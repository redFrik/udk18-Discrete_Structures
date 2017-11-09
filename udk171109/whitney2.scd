s.latency= 0.03;
s.boot;

(
SynthDef(\wping, {|out= 0, amp= 0.1, atk= 0.001, rel= 0.2, cur= -4, freq= 400|
	var env= EnvGen.ar(Env.perc(atk, rel, amp, cur), doneAction:2);
	var src= SinOsc.ar(freq);
	OffsetOut.ar(out, Pan2.ar(src, 0, env));
}).add;
)

(
var width= 500, height= 500;
var win= Window("whitney", Rect(100, 100, width, height));
var usr= UserView(win, Rect(0, 0, width, height));
var lastTheta;
~num= 20;
~speed= -0.5;
~radius= width*0.5/(~num*2);
~spread= ~radius*2;
~factor= 1;
~root= 60;
~scale= Scale.major;
lastTheta= FloatArray.newClear(~num);
usr.background= Color.black;
usr.animate= true;
usr.drawFunc= {
	Pen.smoothing= false;
	Pen.fillColor= Color.white;
	Pen.stringAtPoint("fps:"+usr.frameRate.round(0.1), Point(10, 10));
	Pen.translate(width*0.5, height*0.5);  //offset all drawing to middle
	Pen.strokeColor= Color.white;  //draw a white reference line
	Pen.moveTo(Point(0, 0));
	Pen.lineTo(Point(0, 0)+Polar(width*0.5, 0));
	Pen.stroke;
	Pen.fillColor= Color.red;
	lastTheta= lastTheta.extend(~num, 0);
	~num.do{|index|
		var rho= index+0.5*~spread;
		var theta= usr.frame/(index*~factor+1)*~speed;
		if(((lastTheta[index]%2pi)-(theta%2pi)).abs>pi, {
			//("hit"+index).postln;  //debug
			Synth(\wping, [
				\freq, ~scale.degreeToFreq(index, ~root.midicps, 1),
				\atk, 0.001,
				\rel, 0.2,
				\cur, -4,
			]);
		});
		lastTheta[index]= theta;
		Pen.addOval(Rect.aboutPoint(Point(0, 0)+Polar(rho, theta), ~radius, ~radius));
	};
	Pen.fill;
};
CmdPeriod.doOnce({win.close});
win.front;
)

~radius= 10;
~spread= 10;
~speed= -0.35;
~factor= 1.5
~factor= 1.01
~factor= 0.6667
~factor= 2.5
~scale= Scale.dorian;
~scale= Scale.chromatic;
~scale= Scale.lydian;
~scale= Scale.minor;
~root= 48
~root= 50
~root= 58
~root= 56
~root= 60
~speed= 1;
~factor= 0.25;
~num= 5
~num= 15
~num= 25