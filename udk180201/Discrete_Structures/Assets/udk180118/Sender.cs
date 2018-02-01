using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sender : MonoBehaviour {
	public OSC oscHandler;
	public string address = "/pos";
	void Start () {
		Application.runInBackground = true;
	}
	void Update () {
		float x = Mathf.Sin (Time.time * 3.0F) * 5.5F;
		float y = Mathf.Cos (Time.time * 5.0F) * 4.4F;
		float z = Mathf.Sin (Time.time * 2.0F) * 3.3F;
		transform.localPosition= new Vector3 (x, y, z);

		OscMessage msg= new OscMessage();
		msg.address = address;
		msg.values.Add(x);
		msg.values.Add(y);
		msg.values.Add(z);
		oscHandler.Send(msg);
	}
}