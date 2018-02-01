using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour {
	private int numX= 16;    //must match in sc
	private int numY= 12;    //must match in sc
	private int numZ= 9;    //must match in sc
	public float scaleAmp= 50;  //how large to scale up
	public float minAmp= 0.0F;    //set to 0.01F or something to make the object not completely disappear
	public float ampFadeRate= 0.9F; //how fast to shrink
	public OSC oscHandler;
	public Transform prefab;    //holds our initial gameobject
	List<Transform> clones = new List<Transform>();
	void Start () {
		Application.runInBackground = true;
		oscHandler.SetAddressHandler("/cube", CubeFunc);
		for (int z = 0; z < numZ; z++) {
			for (int y = 0; y < numY; y++) {
				for (int x = 0; x < numX; x++) {
					clones.Add (Instantiate (prefab, new Vector3 (x, y, z), Quaternion.identity));
				}
			}
		}
		prefab.gameObject.SetActive(false);  //hide prefab object
	}
	void Update () {
		Vector3 minAmpVec = new Vector3 (minAmp, minAmp, minAmp);
		foreach (Transform ct in clones) {
			ct.localScale = Vector3.Max(ct.localScale * ampFadeRate, minAmpVec);
		}
	}
	void CubeFunc(OscMessage msg) {
		int x = msg.GetInt (0);
		int y = msg.GetInt (1);
		int z = msg.GetInt (2);
		float degree = msg.GetInt (3);
		float amp = msg.GetFloat (4)*scaleAmp;
		clones[x+(y*numX)+(z*numX*numY)].localScale = new Vector3 (amp, amp, amp);
	}
}