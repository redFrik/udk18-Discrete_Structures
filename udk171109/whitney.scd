(
var width= 500, height= 500;
var win= Window("whitney", Rect(100, 100, width, height));
var usr= UserView(win, Rect(0, 0, width, height));
~num= 50;
~speed= -0.5;
~radius= width*0.5/(~num*2);
~spread= ~radius*2;
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
	~num.do{|index|
		var rho= index+0.5*~spread;
		var theta= usr.frame/(index+1)*~speed;
		Pen.addOval(Rect.aboutPoint(Point(0, 0)+Polar(rho, theta), ~radius, ~radius));
	};
	Pen.fill;
};
CmdPeriod.doOnce({win.close});
win.front;
)

~speed= -0.1
~spread= 50
~num= 5
~radius= 25


~speed= pi*0.99
~spread= 30
~num= 10
~radius= 15
