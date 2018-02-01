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

			////variant 1 - circle
//			go.transform.localPosition = new Vector3 (
//				Mathf.Sin(theta)*scale,
//				Mathf.Cos(theta)*scale,
//				Mathf.Sin(theta)*scale
//			);
//			tr.time = 0.05F;

			////variant 2 - circle spiral
//			            go.transform.localPosition = new Vector3 (
//			                Mathf.Sin(theta)*scale+Mathf.Sin(theta*10),
//			                Mathf.Cos(theta)*scale+Mathf.Cos(theta*10),
//			                Mathf.Sin(theta)*scale+Mathf.Sin(theta*10)
//			            );
//			            tr.time = 0.05F;

			////variant 3 - varying width
//			            go.transform.localPosition = new Vector3 (
//			                Mathf.Sin(theta)*scale+Mathf.Sin(theta*7),
//			                Mathf.Cos(theta)*scale+Mathf.Cos(theta*10),
//			                0
//			            );
//			            tr.startWidth = Mathf.Sin(theta+(Time.frameCount*0.2F))+1.0F;
//			            tr.endWidth = Mathf.Sin(theta+(Time.frameCount*0.11F))+1.0F;
//			            tr.time = 0.1F;

			////variant 4 - wave
			            go.transform.localPosition = new Vector3 (
			                Mathf.Lerp(-scale*2, scale*2, i/num),
			                Mathf.Cos(theta)*scale,
			                Mathf.Sin(theta)*scale
			            );
			            tr.startWidth = 0.3F;
			            tr.endWidth = 0.0F;
			            tr.time = 5.0F;

			////variant 5 - wave with mouse control
//			            go.transform.localPosition = new Vector3 (
//			                Mathf.Lerp(-scale*2, scale*2, i/num),
//			                Mathf.Cos(theta * (Input.mousePosition.x*0.02F))*scale,
//			                Mathf.Sin(theta * (Input.mousePosition.x*0.02F))*scale
//			            );
//			            tr.startWidth = 0.1F;
//			            tr.endWidth = 0.1F;
//			            tr.time = Input.mousePosition.y * 0.01F;

		}
	}
}