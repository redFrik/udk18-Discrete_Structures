using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartMove : MonoBehaviour {
	public float xspeed= 2.0F;
	public float yspeed= 3.0F;
	public float zspeed= 4.0F;
	public float scale = 5.0F;
	public float xrot = 1.0F;
	public float yrot = 0.0F;
	public float zrot = 0.0F;
	void Start () {
	}
	void Update () {
		transform.localPosition = new Vector3 (Mathf.Sin (Time.time * xspeed) * scale, Mathf.Cos (Time.time * yspeed) * scale, 0);
		//transform.localPosition = new Vector3 (Mathf.Sin (Time.time * xspeed) * scale, Mathf.Cos (Time.time * yspeed) * scale, Mathf.Sin (Time.time * zspeed) * scale);
		transform.localEulerAngles = new Vector3 (Time.time*xrot, Time.time*yrot, Time.time*zrot);
	}
}