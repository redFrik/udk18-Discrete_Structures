more samples and trails
--------------------

supercollider
==

TODO

NOTE: can not be `.mp3`. in supercollider it is recommended to use `.aiff` or `.wav`

```supercollider
s.boot;
s.meter;
s.scope;

//TODO

```

- - -

unity3d
==

trails in unity are very similar to the lines we played with [last week](https://github.com/redFrik/udk18-Discrete_Structures/tree/master/udk171130#unity3d).

NOTE: again this will require unity version 5.5 or newer. and if you try it in 2D remember to add a light.

* start unity and create a new 3D project. give it a name (here 'trails').
* select Assets / Create / Material
* give the material a name (can be anything - here 'TrailMat') by typing under the icon
* at the top of the inspector for the material select Shader / Unlit / Texture
* also click on the Base Texture icon and select 'Default-Particle'
* select GameObject / Create Empty
* select Component / Effects / Trail Renderer
* in the inspector find Materials / Element 0 and set it to your TrailMat
* also set Time right below to something less than the default 5 (here 2 seconds)

![00time](00time.png?raw=true "00time")

* press play and try changing the x, y, z positions in the GameObject's Transform. you should see a trail following along
* also, while running, try switching to the scene view and click and drag the game object around in there

![01drag](01drag.png?raw=true "01drag")

* also try setting `Min Vertex Distance` to something high like 8 and observe what happens

so trails basically follow any object but are erased after a fixed time. now move the game object around automatically with a script...

* create a new script by selecting Assets / Create / C# Script
* give the script the name 'Trails' by typing under the white icon
* double click the white C# script icon to open it in MonoDevelop
* copy and paste in the code here below replacing what was there originally

```cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trails : MonoBehaviour {
    public float xspeed= 0.2F;
    public float yspeed= 0.3F;
    public float zspeed= 1.0F;
    public float scale = 5.0F;
    void Start () {
    }
    void Update () {
        transform.localPosition = new Vector3 (Mathf.Sin (Time.frameCount * xspeed) * scale, Mathf.Cos (Time.frameCount * yspeed) * scale, 0);
        //transform.localPosition= new Vector3 (Mathf.Sin(Time.frameCount*xspeed)*scale, Mathf.Cos(Time.frameCount*yspeed)*scale, Mathf.Sin(Time.frameCount*zspeed)*scale);
        //transform.localPosition= new Vector3 (((Time.frameCount+1)/2%2*2-1)*scale+(Mathf.Sin(Time.frameCount*xspeed*xspeed)*scale), (Time.frameCount/2%2*2-1)*scale, Time.frameCount*(zspeed*0.025F)%scale);
        //transform.localPosition= new Vector3 ((Time.frameCount/2%2*Mathf.Sin(Time.frameCount*xspeed))*scale, (Time.frameCount/3%2*Mathf.Sin(Time.frameCount*yspeed))*scale, (Time.frameCount/4%2*Mathf.Sin(Time.frameCount*zspeed))*scale);
    }
}
```

* attach the script to the GameObject by drag and drop.
* play around with the script variables - specially try setting `xspeed`, `yspeed` to 1, 1.25, 0.5, 0.33333 etc and compare it with slightly _nudged_ values like 1.251, 0.49, 0.32 etc
* don't forget to change the `Min Vertex Distance`, `Time` and other parameters
* also try the different variants in the script (by uncommenting and commenting out lines)
* drag a photo from your computer into the Assets, then onto the Base Texture icon in Material inspector
* try some different shaders

![02script](02script.png?raw=true "02script")

multiple
--

now we will make a new project with many objects and many trails. this time trying to do most of it in the script.

* start unity and create a new 3D project. give it a name (here 'trails2').
* create a new script by selecting Assets / Create / C# Script
* give the script the name 'Trails2' by typing under the white icon
* double click the white C# script icon to open it in MonoDevelop
* copy and paste in the code here below replacing what was there originally

```cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trails2 : MonoBehaviour {
    public Material mat;
    public float spread= 0.01F;
    public float scale = 5.0F;
    int num= 50;  //number of objects
    List<GameObject> list= new List<GameObject>();
    void Start () {
        mat = new Material (Shader.Find ("Particles/Additive"));  //create a material - comment out to add your own
        for (int i = 0; i < num; i++) {
            GameObject go= new GameObject();  //create a new empty object
            go.AddComponent<TrailRenderer>();  //add a trail renderer component
            TrailRenderer tr = go.GetComponent<TrailRenderer> ();
            tr.material = mat;  //set the material to the trail renderer
            tr.time = 0.2F; //default length
            tr.startWidth = 0.1F;  //default width
            list.Add(go);  //add the object to the list of objects
        }
    }
    void Update () {
        float i = 0;
        foreach (GameObject go in list) {
            i++;
            float theta = (Time.frameCount * spread) + ((i / num)*Mathf.PI*2);
            TrailRenderer tr = go.GetComponent<TrailRenderer> ();

            //variant 1 - circle
            go.transform.localPosition = new Vector3 (
                Mathf.Sin(theta)*scale,
                Mathf.Cos(theta)*scale,
                Mathf.Sin(theta)*scale
            );
            tr.time = 0.05F;

            //variant 2 - circle spiral
            //            go.transform.localPosition = new Vector3 (
            //                Mathf.Sin(theta)*scale+Mathf.Sin(theta*10),
            //                Mathf.Cos(theta)*scale+Mathf.Cos(theta*10),
            //                Mathf.Sin(theta)*scale+Mathf.Sin(theta*10)
            //            );
            //            tr.time = 0.05F;

            //variant 3 - varying width
            //            go.transform.localPosition = new Vector3 (
            //                Mathf.Sin(theta)*scale+Mathf.Sin(theta*7),
            //                Mathf.Cos(theta)*scale+Mathf.Cos(theta*10),
            //                0
            //            );
            //            tr.startWidth = Mathf.Sin(theta+(Time.frameCount*0.2F))+1.0F;
            //            tr.endWidth = Mathf.Sin(theta+(Time.frameCount*0.11F))+1.0F;
            //            tr.time = 0.1F;

            //variant 4 - wave
            //            go.transform.localPosition = new Vector3 (
            //                Mathf.Lerp(-scale*2, scale*2, i/num),
            //                Mathf.Cos(theta)*scale,
            //                Mathf.Sin(theta)*scale
            //            );
            //            tr.startWidth = 0.3F;
            //            tr.endWidth = 0.0F;
            //            tr.time = 5.0F;
            //
            
            //variant 5 - wave with mouse control
            //            go.transform.localPosition = new Vector3 (
            //                Mathf.Lerp(-scale*2, scale*2, i/num),
            //                Mathf.Cos(theta * (Input.mousePosition.x*0.02F))*scale,
            //                Mathf.Sin(theta * (Input.mousePosition.x*0.02F))*scale
            //            );
            //            tr.startWidth = 0.1F;
            //            tr.endWidth = 0.1F;
            //            tr.time = Input.mousePosition.y * 0.01F;
            
        }
    }
}

```

![03wave](03wave.png?raw=true "03wave")

2D
--

last let us try out trails in a **2D** project.

* restart unity and create a new 2D project. give it a name (here trails3)
* create a new script by selecting Assets / Create / C# Script
* give the script the name 'Trails3' by typing under the white icon
* double click the white C# script icon to open it in MonoDevelop
* copy and paste in the code here below replacing what was there originally

```cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trails3 : MonoBehaviour {
    public Transform prefab;    //holds our initial gameobject
    public float scale = 4.0F;
    public float spread = 0.23F;
    public float speed = 0.01F;
    List<Transform> clones = new List<Transform> ();
    int num = 30;    //set number of clones
    void Start () {  //do once when starting
        for (int i = 0; i < num; i++) {
            clones.Add (Instantiate (prefab, new Vector3 (0, 0, 0), Quaternion.identity));
        }
        prefab.gameObject.SetActive (false);  //hide prefab object
    }
    void Update () {
        float i= 0;    //keep track of clone index
        foreach (Transform ct in clones) {
            ct.localPosition= new Vector3(Mathf.Lerp (-9, 9, i / num), Mathf.Sin(Time.frameCount*speed+(i*spread))*scale, Mathf.Cos(Time.frameCount*speed+(i*spread))*scale);
            i++;
        }
    }
}
```

* save and switch back to unity
* add a light by selecting GameObject / Light / Directional Light (remember to test different positions, rotation, colours etc later )
* in the upper left hierarchy window, click to select the 'Main Camera'
* attach the script to the camera by selecting Component / Scripts / Trails3 (or just drag and drop the script onto the camera)
* select GameObject / 3D Object / Sphere (or whatever you feel like - can also be an empty game object)
* select the Main Camera and in the inspector click on the little circle next to prefab. select the Sphere by doubleclicking in the dialog that pops up (or just drag the sphere onto the prefab variable slot)
* add a Trail Renderer to the Sphere (by first clicking sphere in the hierarchy window, then selecting Component / Effects / Trail Renderer)
* set `Time` to 5, `Min Vertex Distance` to 0, `Width` to 5, `Corner Vertices` to 50, `End Cap Verticies` also to 50 and in the pop up Texture Mode select Tile

![04material](04material.png?raw=true "04material")

* select Assets / Create / Material
* give the material a name (can be anything - here 'Trail3Mat') by typing under the icon
* at the top of the inspector for the material select Shader / Particle / Additive (or some other shader also possible)
* also click on the Particle Texture icon and select 'Default-Particle' (or import your own photo and add it here)
* finally find Materials /Element 0 in the Sphere's inspector (under Trail Renderer) and set it to Trail3Mat

![05strange](05strange.png?raw=true "05strange")

play around (don't forget to change variables speed, spread, scale etc in the code, hack it and write your own variants)

links
==

Board To Bits - [Unity 101 playlist](https://www.youtube.com/watch?v=kH_piLCynto&list=PL5KbKbJ6Gf9_H4MZC1v6ETs_ifnLfNJVw)
