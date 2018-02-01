using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Receiver : MonoBehaviour {
	public OSC oscHandler;
	Renderer rend;    //added this variable
	void Start () {
		Application.runInBackground = true;
		oscHandler.SetAddressHandler("/position", Position);
		oscHandler.SetAddressHandler("/scale", Scale);
		oscHandler.SetAddressHandler("/rotation", Rotation);
		oscHandler.SetAddressHandler("/colour", Colour);    //and this line
		rend= GetComponent<Renderer>();    //and here we set the rend variable
	}
	void Update () {
	}
	void Position(OscMessage msg) {
		float x = msg.GetFloat(0);
		float y = msg.GetFloat(1);
		float z = msg.GetFloat(2);
		transform.localPosition = new Vector3(x, y, z);
	}
	void Scale(OscMessage msg) {
		float x = msg.GetFloat(0);
		float y = msg.GetFloat(1);
		float z = msg.GetFloat(2);
		transform.localScale = new Vector3(x, y, z);
	}
	void Rotation(OscMessage msg) {
		float x = msg.GetFloat(0);
		float y = msg.GetFloat(1);
		float z = msg.GetFloat(2);
		transform.localEulerAngles = new Vector3(x, y, z);
	}
	void Colour(OscMessage msg) {    //added this method that sets the colour of the material of the renderer when a message arrives
		float r = msg.GetFloat(0);
		float g = msg.GetFloat(1);
		float b = msg.GetFloat(2);
		float a = msg.GetFloat(3);
		rend.material.color = new Color (r, g, b, a);
	}
}