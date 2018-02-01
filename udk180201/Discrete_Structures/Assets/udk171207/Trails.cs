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