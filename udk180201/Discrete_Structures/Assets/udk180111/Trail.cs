using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trail : MonoBehaviour {
	TrailRenderer trail;
	void Start () {
		Application.runInBackground = true;
		trail = gameObject.AddComponent<TrailRenderer>();
		trail.material = new Material (Shader.Find("Particles/Additive"));
	}

	////variant 1
//	void Update () {
//		trail.time = 1.2F;
//		trail.startColor = Color.blue;
//		trail.endColor = Color.black;
//		trail.startWidth = 0.5F;
//		trail.endWidth = 0.0F;
//		float x = (Mathf.Sin (Time.time * 15.0F) + Mathf.Sin (Time.time * 1.3F)) * 1.5f ;
//		float y = (Mathf.Sin (Time.time * 14.0F) + Mathf.Sin (Time.time * 1.2F)) * 1.5f ;
//		float z = (Mathf.Sin (Time.time * 13.0F) + Mathf.Sin (Time.time * 1.1F)) * 1.5f ;
//		transform.localPosition= new Vector3 (x, y+5.0f, z);
//	}

	////variant 2
	void Update () {
        trail.time = 2.2F;
        trail.startColor = Color.blue;
        trail.endColor = Color.black;
        trail.startWidth = 0.5F;
        trail.endWidth = 0.0F;
        float x = Mathf.Sin (Time.time * 3.0F) * 5.5F;
        float y = Mathf.Cos (Time.time * 5.0F) * 4.4F;
        float z = Mathf.Sin (Time.time * 4.0F) * 5.5F;
        transform.localPosition= new Vector3 (Mathf.Sin(x)*5.0f, Mathf.Sin(y)*5.0f, Mathf.Sin(z)*5.0f);
    }

	////variant 3
//	void Update () {
//		trail.time = 0.3F;
//		trail.startColor = Color.white;
//		trail.endColor = Color.black;
//		trail.startWidth = 0.5F;
//		trail.endWidth = 0.0F;
//		float[] data = new float[] {0.1f, 0.2f, 0.3f, 0.2f, 0.1f, -0.2f, -1.3f, -0.1f};    //add your own floats here
//		float x = Time.time*4.0F%20.0F-10.0f;   //left to right scanning - can also be a lookup, sine or whatever
//		float y= data[(int)Mathf.Round(Time.frameCount*0.2F) % data.Length]*10.0F;
//		float z= data[(int)Mathf.Round(Time.frameCount*0.1F) % data.Length]*10.0F+10.0F;
//		transform.localPosition= new Vector3 (x, y, z);
//	}

}