whitney
--------------------

[john whitney](https://en.wikipedia.org/wiki/John_Whitney_(animator))

"_...if one element were set to move at a given rate, the next element might be moved two times that rate. Then the third would move at three times that rate and so on._" - John Whitney, Digital Harmony page 38

Memo Akten - https://www.memo.tv/works/simple-harmonic-motion/

https://www.youtube.com/watch?v=yVkdfJ9PkRQ  Pendulum Waves

https://www.memo.tv/works/simple-harmonic-motion-12-for-16-percussionists-at-rncm/

supercollider
==

first open and run the code in [whitney.scd](https://github.com/redFrik/udk18-Discrete_Structures/blob/master/udk171109/whitney.scd?raw=true)

and then [whitney2.scd](https://github.com/redFrik/udk18-Discrete_Structures/blob/master/udk171109/whitney2.scd?raw=true)

after trying them out copy and paste the code below into a new supercollider document.

```supercollider
s.boot;
s.meter;
s.scope;

//--load a sound
(
SynthDef(\avball, {|out= 0, freq= 500, atk= 0.001, rel= 0.3, amp= 0.1, gate= 1, pan= 0, dir= 1, rate= 5, mod= 0|
    var env= EnvGen.ar(Env.adsr(atk, atk, 0.5, rel), gate, doneAction:2);
    var snd= SinOscFB.ar(dir**Sweep.ar(0, rate)*freq, SinOsc.ar(mod).abs);
    OffsetOut.ar(out, Pan2.ar(snd*env, pan, amp));
}).add;
)

//--warmup
(
Pdef(\ball1, PmonoArtic(\avball,
    \freq, 400,  //change this
    \legato, 0.1,
    \dur, 1,  //and this
    \amp, 0.5,
)).play;
)

(
Pdef(\ball1, PmonoArtic(\avball,
    \freq, 400,  //first ball at 400hz
    \legato, 0.1,
    \dur, 0.25,  //duration 0.25
    \amp, 0.5,
)).play;
Pdef(\ball2, PmonoArtic(\avball,
    \freq, 500,  //second ball at 500hz
    \legato, 0.1,
    \dur, 0.5,  //with double duration
    \amp, 0.5,
)).play;
Pdef(\ball3, PmonoArtic(\avball,
    \freq, 600,  //third ball at 600hz
    \legato, 0.1,
    \dur, 0.75,  //three times the duration of ball1
    \amp, 0.5,
)).play;
)

(
Pdef(\ball1, PmonoArtic(\avball,
    \freq, Pseq([500, 400, 300, 200], inf),
    \dir, Pseq([0.5, 1, 2, 3], inf),
    \rate, Pstutter(4, Pseq([1, 2, 3, 14], inf)),
    \dur, 0.2,
    \legato, 0.1,
    \amp, 0.5,
)).play;
Pdef(\ball2, PmonoArtic(\avball,
    \freq, Pseq([500, 400, 300, 200], inf)*1.25,
    \dir, Pseq([0.5, 1, 2, 3], inf),
    \rate, Pstutter(4, Pseq([1, 2, 3, 14], inf)),
    \dur, 0.3,
    \legato, 0.1,
    \amp, 0.5,
)).play;
Pdef(\ball3, PmonoArtic(\avball,
    \freq, Pseq([500, 400, 300, 200], inf)*1.5,
    \dir, Pseq([0.5, 1, 2, 3], inf),
    \rate, Pstutter(4, Pseq([1, 2, 3, 14], inf)),
    \dur, 0.4,
    \legato, 0.1,
    \amp, 0.5,
)).play;
)

Pdef.all.do{|x| x.clear};

//--whitney
(
Pdef.all.do{|x| x.clear};
12.do{|i|  //do something 12 times
    Pdef(("ball"++i).asSymbol, PmonoArtic(\avball,  //create ball0, ball1 ball2 etc
        \freq, 500+(100*i),  //assing frequencies 500, 600, 700 etc
        \dur, i+1*0.25,  //assign durations 0.25, 0.5, 0.75 etc
        \dir, 1,
        \legato, 0.1,
        \amp, 0.1,
    )).play;
};
)

(
Pdef.all.do{|x| x.clear};
12.do{|i|
    Pdef(("ball"++i).asSymbol, PmonoArtic(\avball,
        \freq, 300+(100*i),
        \dur, i+1*0.25,
        \sustain, 0.1,  //sustain instead of legato to have note duration independent of \dur
        \amp, 0.1,
        \pan, i.linlin(0, 11, -0.9, 0.9),  //added panning
    )).play;
};
)

(
Pdef.all.do{|x| x.clear};
12.do{|i|
    Pdef(("ball"++i).asSymbol, PmonoArtic(\avball,
        //\freq, 500+(100*i),
        \degree, i,  //note scale degree here instead of freq
        \dur, i+1*0.25,
        \dir, 1,
        \sustain, 0.1,
        \amp, 0.1,
        \pan, i.linlin(0, 11, -0.9, 0.9),
    )).play;
};
)

(
Pdef.all.do{|x| x.clear};
10.do{|i|
    Pdef(("ball"++i).asSymbol, PmonoArtic(\avball,
        \degree, i,
        \scale, Scale.minor,  //added scale
        \dur, 1+(i+1*0.5)*Pseq([0.5, 0.25], inf),  //added pattern for dur
        \sustain, 0.1,
        \amp, 0.1,
        \pan, i.linlin(0, 11, -0.9, 0.9)
    )).play;
};
)

(
Pdef.all.do{|x| x.clear};
10.do{|i|
    Pdef(("ball"++i).asSymbol, PmonoArtic(\avball,
        \degree, i,
        \scale, Scale.minor,
        \dur, 1+(i+1*0.5)*Pseq([0.5, 0.25], inf),
        \mod, Pseq([0, 0, 0, 1], inf),  //modulation - see synthdef above
        \sustain, Pseq([0.1, 0.1, 1], inf),  //added pattern for sustain
        \amp, 0.1,
        \pan, i.linlin(0, 11, -0.9, 0.9)
    )).play;
};
)

(
Pdef.all.do{|x| x.clear};
10.do{|i|
    Pdef(("ball"++i).asSymbol, PmonoArtic(\avball,
        \degree, i,
        \scale, Scale.major,
        \dur, 0.5+(i+1*0.5)*Pseq([0.5, 0.25], inf),
        \mod, 5,  //static modulation - try changing to something higher or a pattern like Pseq([60, 0, 0], inf)
        \sustain, 0.01,  //shorter sustain
        \amp, 0.1,
        \pan, i.linlin(0, 11, -0.9, 0.9)
    )).play;
};
)

//something different
(
Pdef.all.do{|x| x.clear};
7.do{|i|
    Pdef(("ball"++i).asSymbol, PmonoArtic(\avball,
        \degree, i,
        \scale, Scale.minor,
        \dur, 0.5+(i+1*0.5)*2,  //*2 to make everything slower
        \atk, 5,  //long attack time
        \rel, 5,  //long release time
        \dir, 0.99,  //always falling a little bit
        \rate, 1,  //how fast to fall
        \sustain, 0.05,  //note duration
        \amp, 0.4,
        \pan, i.linlin(0, 11, -0.9, 0.9)
    )).play;
};
)

//some different sounds to try
(
SynthDef(\avball, {|out= 0, freq= 500, atk= 0.001, rel= 0.3, amp= 0.1, gate= 1, pan= 0, dir= 1, rate= 5, mod= 0|
    var env= EnvGen.ar(Env.adsr(atk, atk, 0.5, rel), gate, doneAction:2);
    var snd= SinOsc.ar(dir**Sweep.ar(0, rate)*freq, SinOsc.ar(mod)*pi);
    OffsetOut.ar(out, Pan2.ar(snd*env, pan, amp));
}).add;
)

(
SynthDef(\avball, {|out= 0, freq= 500, atk= 0.001, rel= 0.3, amp= 0.1, gate= 1, pan= 0, dir= 1, rate= 5, mod= 0|
    var env= EnvGen.ar(Env.adsr(atk, atk, 0.5, rel), gate, doneAction:2);
    var snd= VarSaw.ar(dir**Sweep.ar(0, rate)*freq, 0, SinOsc.ar(mod, pi)*mod%1);
    OffsetOut.ar(out, Pan2.ar(snd*env, pan, amp));
}).add;
)

(
SynthDef(\avball, {|out= 0, freq= 500, atk= 0.001, rel= 0.3, amp= 0.1, gate= 1, pan= 0, dir= 1, rate= 5, mod= 0|
    var env= EnvGen.ar(Env.adsr(atk, atk, 0.5, rel), gate, doneAction:2);
    var snd= SinOscFB.ar(dir**Sweep.ar(0, rate)*freq, LFSaw.ar(mod, freq%2)+1/2);
    OffsetOut.ar(out, Pan2.ar(snd*env, pan, amp));
}).add;
)
```

- - -

unity3d
==

first we create a simple 2d scene with a single sphere rotating around.

* start unity and create a new **2D** project. give it a name (here whitney2d)
* create a new script by selecting Assets / Create / C# Script
* give the script the name 'Whitney2D' by typing under the white icon
* double click the white C# script icon to open it in MonoDevelop
* copy and paste in the code here below replacing what was there originally


```cs
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Whitney2D : MonoBehaviour {
    public Transform prefab;    //holds our initial gameobject
    List<Transform> clones = new List<Transform>();
    public float rotationSpeed;
    public float spread;
    void Start() {  //do once when starting
        rotationSpeed= 0.1F;
        spread = 1.0F;
        clones.Add (Instantiate (prefab, new Vector2 (0, 0), Quaternion.identity));
        transform.position= new Vector3(0, 0, -10);  //default camera position
    }
    void Update() {  //do every frame - many times per second
        foreach(Transform ct in clones) {
            ct.localPosition= new Vector2(
                Mathf.Sin (Time.frameCount*rotationSpeed)*spread,
                Mathf.Cos (Time.frameCount*rotationSpeed)*spread
            );
        }
    }
}
```

* save and switch back to unity
* in the upper left hierachy window, click to select the 'Main Camera'
* attach the script to the camera by selecting Component / Scripts / Whitney2D
* create a new sphere by selecting GameObject / 3D Object / Sphere. this game object will become our prefab from which the clone will be made
* again select the main camera and in the inspector click on the little circle next to prefab. select the Sphere by doubleclicking in the dialog that pops up
* optionally create a light by selecting GameObject / Light / Directional Light
* press play and you should see one rotating sphere (the clone) around another (the prefab)
* change the variables...

![00whitney](00whitney.png?raw=true "00whitney")

study the code and try to make some changes. what if `spread` is also oscillating...
```cs
Mathf.Sin (Time.frameCount*rotationSpeed)*Mathf.Sin(spread*Time.frameCount*0.01F),
Mathf.Cos (Time.frameCount*rotationSpeed)*Mathf.Cos(spread*Time.frameCount*0.01F)
```

or...

```cs
Mathf.Sin (Time.frameCount*rotationSpeed)*Mathf.Sin(spread*Time.frameCount*0.01F),
Mathf.Cos (Time.frameCount*rotationSpeed)*Mathf.Sin(spread*Time.frameCount*0.01F)
```

now let us modify the scipt a little bit and create many objects.

* copy and paste in the code here below replacing what was there before
* study the differences (bascially added two for loops and two variables)

```cs
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Whitney2D : MonoBehaviour {
    public Transform prefab;    //holds our initial gameobject
    List<Transform> clones = new List<Transform>();
    int num= 10;  //added this variable to set number of clones
    public float rotationSpeed;
    public float spread;
    void Start() {  //do once when starting
        rotationSpeed= 0.1F;
        spread = 1.0F;
        for (int i = 0; i < num; i++) {    //added for loop here
            clones.Add (Instantiate (prefab, new Vector2 (0, 0), Quaternion.identity));
        }
        transform.position= new Vector3(0, 0, -10);  //default camera position
    }
    void Update() {  //do every frame - many times per second
        int i= 1;    //added this variable to keep track of clone index
        foreach(Transform ct in clones) {
            ct.localPosition= new Vector2(
                Mathf.Sin (Time.frameCount*rotationSpeed)*(spread*i), //note spread*i
                Mathf.Cos (Time.frameCount*rotationSpeed)*(spread*i)
            );
            i++;
        }
    }
}
```

again study the code and make changes. what is happening here...
```cs
Mathf.Sin (Time.frameCount*rotationSpeed+(i*0.1F))*(spread*i), //note spread*i
Mathf.Cos (Time.frameCount*rotationSpeed)*(spread*i)
```

and why is it so different from this...
```cs
Mathf.Sin (Time.frameCount*rotationSpeed+(i*0.1F))*(spread*i), //note spread*i
Mathf.Cos (Time.frameCount*rotationSpeed+(i*0.1F))*(spread*i)
```

try with different values for `0.1F`

and then what if the offset `i*0.1F` was oscillating...
```cs
Mathf.Sin (Time.frameCount*rotationSpeed+(i*Mathf.Sin(Time.frameCount*0.01F)))*(spread*i), //note spread*i
Mathf.Cos (Time.frameCount*rotationSpeed+(i*Mathf.Sin(Time.frameCount*0.01F)))*(spread*i)
```

or...
```cs
Mathf.Sin (Time.frameCount*rotationSpeed+(i*Mathf.Sin(Time.frameCount*0.01F)))*(spread*i), //note spread*i
Mathf.Cos (Time.frameCount*rotationSpeed+(i*Mathf.Cos(Time.frameCount*0.01F)))*(spread*i)
```

last we change the code into a 'whitney' system with the following two lines...
```cs
Mathf.Sin (Time.frameCount*rotationSpeed*i+(Mathf.PI*0.5F))*(spread*i),
Mathf.Cos (Time.frameCount*rotationSpeed*i+(Mathf.PI*0.5F))*(spread*i)
```

the `(Mathf.PI*0.5F)` part is just to add an offset to the rotation. makes the convergent point happen at the right hand side instead of at the top. if you set rotationSpeed to something like `0.0314159` (pi/100) you get the characteristic whitney system.

3d
--

now the same thing but in three dimensions.

* start unity and create a new **3D** project. give it a name (here whitney3d)
* create a new script by selecting Assets / Create / C# Script
* give the script the name 'Whitney3D' by typing under the white icon
* double click the white C# script icon to open it in MonoDevelop
* copy and paste in the code here below replacing what was there originally

```cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whitney3D : MonoBehaviour {
    public Transform prefab;    //holds our initial gameobject
    List<Transform> clones = new List<Transform>();
    int num= 10;  //added this variable to set number of clones
    public float rotationSpeed;
    public float spread;
    void Start() {  //do once when starting
        rotationSpeed= 0.01F;
        spread = 1.0F;
        for (int i = 0; i < num; i++) {    //added for loop here
            clones.Add (Instantiate (prefab, new Vector3 (0, 0, 0), Quaternion.identity));
        }
        transform.position= new Vector3(0, 0, -10);  //default camera position
    }
    void Update() {  //do every frame - many times per second
        int i= 1;    //added this variable to keep track of clone index
        foreach(Transform ct in clones) {
            ct.localPosition= new Vector3(
                Mathf.Sin (Time.frameCount*rotationSpeed*i+(Mathf.PI*0.5F))*(spread*i),
                Mathf.Cos (Time.frameCount*rotationSpeed*i+(Mathf.PI*0.5F))*(spread*i),
                0
            );
            i++;
        }
    }
}
```

* save and switch back to unity
* in the upper left hierachy window, click to select the 'Main Camera'
* attach the script to the camera by selecting Component / Scripts / Whitney3D
* create a new cube by selecting GameObject / 3D Object / Cube. this game object will become our prefab from which the clone will be made
* again select the main camera and in the inspector click on the little circle next to prefab. select the Cube by doubleclicking in the dialog that pops up
* press play and you should see one rotating cube (the clone) around another (the prefab)
* change the variables. things to try...
  * try copying the modifications we did in the 2d example above (the two lines that update the x and y positions)
  * try adding more objects and zoom out main camera (z more negative).
  * change clear flags on main camera to 'Depth only' and play with the light
  * add an oscillator for the third dimension by making the lines in the update for loop look like this...

```cs
ct.localPosition= new Vector3(
    Mathf.Sin (Time.frameCount*rotationSpeed*i+(Mathf.PI*0.5F))*(spread*i),
    Mathf.Cos (Time.frameCount*rotationSpeed*i+(Mathf.PI*0.5F))*(spread*i),
    Mathf.Sin (Time.frameCount*rotationSpeed*0.4F*i+(Mathf.PI*0.5F))*(spread*i)
);
```

and last we can add individual rotation to each object like this (full script)...
```cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whitney3D : MonoBehaviour {
    public Transform prefab;    //holds our initial gameobject
    List<Transform> clones = new List<Transform>();
    int num= 100;  //added this variable to set number of clones
    public float rotationSpeed;
    public float spread;
    void Start() {  //do once when starting
        rotationSpeed= 0.001F;
        spread = 0.1F;
        for (int i = 0; i < num; i++) {    //added for loop here
            clones.Add (Instantiate (prefab, new Vector3 (0, 0, 0), Quaternion.identity));
        }
        transform.position= new Vector3(0, 0, -10);  //default camera position
    }
    void Update() {  //do every frame - many times per second
        int i= 1;    //added this variable to keep track of clone index
        foreach(Transform ct in clones) {
            ct.localPosition= new Vector3(
                Mathf.Sin (Time.frameCount*rotationSpeed*i+(Mathf.PI*0.5F))*(spread*i),
                Mathf.Cos (Time.frameCount*rotationSpeed*i+(Mathf.PI*0.5F))*(spread*i),
                Mathf.Sin (Time.frameCount*rotationSpeed*0.4F*i+(Mathf.PI*0.5F))*(spread*i)
            );
            ct.localEulerAngles = new Vector3 (
                Mathf.Sin (Time.frameCount * rotationSpeed * i) * 60.0F,
                Mathf.Cos (Time.frameCount * rotationSpeed * i) * 50.0F,
                1
            );
            i++;
        }
    }
}
```

![01whitney](01whitney.png?raw=true "01whitney")

one last example where we modulate position, rotation and scale in 3d...

```cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whitney3D : MonoBehaviour {
    public Transform prefab;    //holds our initial gameobject
    List<Transform> clones = new List<Transform>();
    int num= 100;  //added this variable to set number of clones
    public float rotationSpeed;
    public float spread;
    void Start() {  //do once when starting
        rotationSpeed= -0.001F;
        spread = 0.2F;
        for (int i = 0; i < num; i++) {
            clones.Add (Instantiate (prefab, new Vector3 (0, 0, 0), Quaternion.identity));
        }
        transform.position= new Vector3(0, 0, -60);  //default camera position
        prefab.gameObject.SetActive(false);  //hide prefab object
    }
    void Update() {  //do every frame - many times per second
        int i= 1;
        foreach(Transform ct in clones) {
            ct.localPosition= new Vector3(
                Mathf.Sin (Time.frameCount*rotationSpeed*i+(Mathf.PI*0.5F))*(spread*i),
                Mathf.Cos (Time.frameCount*rotationSpeed*i+(Mathf.PI*0.5F))*(spread*i),
                Mathf.Sin (Time.frameCount*rotationSpeed*0.4F*i+(Mathf.PI*0.5F))*(spread*i)
            );
            ct.localEulerAngles = new Vector3 (
                Mathf.Sin (Time.frameCount * rotationSpeed * i) * 10.0F,
                Mathf.Cos (Time.frameCount * rotationSpeed * i) * 22.0F,
                1
            );
            ct.localScale = new Vector3 (
                (Mathf.Sin (Time.frameCount * rotationSpeed * i) + 1.0F)* 8.0F,
                (Mathf.Cos (Time.frameCount * rotationSpeed * i) + 1.0F) * 9.0F,
                (Mathf.Sin (Time.frameCount * rotationSpeed * i) + 1.0F) * 10.0F
            );
            i++;
        }
    }
}
```

- - -

links
==

see also https://github.com/redFrik/udk02-Audiovisual_Programming/blob/master/10rotation.scd

and these [supercollider classes](https://fredrikolofsson.com/f0blog/whitney-balls/)

John Whitney’s Digital Harmony – On the Complementarity of Music and Visual Art - https://www.dataisnature.com/?p=2274

http://whitneymusicbox.org/
