using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class SomeObjects : MonoBehaviour {
	public Transform prefab;    //holds our initial gameobject
	List<Transform> clones = new List<Transform>();
	int num= 30;  //set number of clones
	public float rotation1Speed;
	public float rotation2Speed;
	public float rotation3Speed;
	public float spread;
	void Start() {  //do once when starting
		rotation1Speed= -0.002F;
		rotation2Speed = 0.01F;
		rotation3Speed = 0.003F;
		spread = 0.15F;
		for (int i = 0; i < num; i++) {
			clones.Add (Instantiate (prefab, new Vector3 (0, 0, 0), Quaternion.identity));
		}
		prefab.gameObject.SetActive(false);  //hide prefab object
	}
	void Update() {  //do every frame - many times per second
		int i= 1;    //keep track of clone index
		foreach(Transform ct in clones) {
			ct.localPosition= new Vector3(
				Mathf.Sin (Time.frameCount*rotation1Speed*i+(Mathf.PI*0.5F))+(Mathf.Sin(Time.frameCount*rotation2Speed)*(spread*i)),
				Mathf.Cos (Time.frameCount*rotation1Speed*i+(Mathf.PI*0.5F))-(Mathf.Cos(Time.frameCount*rotation2Speed)*(spread*i)),
				Mathf.Sin (Time.frameCount*rotation3Speed*i+(Mathf.PI*0.5F))+(Mathf.Cos(Time.frameCount*rotation2Speed)*(spread*i))
			);
			ct.localEulerAngles = new Vector3 (
				Mathf.Sin (Time.frameCount * rotation1Speed * i) * 100.0F,
				Mathf.Cos (Time.frameCount * rotation1Speed * i) * 100.0F,
				1
			);
			i++;
		}
	}
}