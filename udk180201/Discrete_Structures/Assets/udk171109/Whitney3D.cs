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

			////variant 1
			ct.localPosition= new Vector3(
				Mathf.Sin (Time.frameCount*rotationSpeed*i+(Mathf.PI*0.5F))*(spread*i),
				Mathf.Cos (Time.frameCount*rotationSpeed*i+(Mathf.PI*0.5F))*(spread*i),
				0
			);

			////variant 2
//			ct.localPosition= new Vector3(
//				Mathf.Sin (Time.frameCount*rotationSpeed*i+(Mathf.PI*0.5F))*(spread*i),
//				Mathf.Cos (Time.frameCount*rotationSpeed*i+(Mathf.PI*0.5F))*(spread*i),
//				Mathf.Sin (Time.frameCount*rotationSpeed*0.4F*i+(Mathf.PI*0.5F))*(spread*i)
//			);
//			ct.localEulerAngles = new Vector3 (
//				Mathf.Sin (Time.frameCount * rotationSpeed * i) * 60.0F,
//				Mathf.Cos (Time.frameCount * rotationSpeed * i) * 50.0F,
//				1
//			);

			////variant 3
//			ct.localPosition= new Vector3(
//				Mathf.Sin (Time.frameCount*rotationSpeed*i+(Mathf.PI*0.5F))*(spread*i),
//				Mathf.Cos (Time.frameCount*rotationSpeed*i+(Mathf.PI*0.5F))*(spread*i),
//				Mathf.Sin (Time.frameCount*rotationSpeed*0.4F*i+(Mathf.PI*0.5F))*(spread*i)
//			);
//			ct.localEulerAngles = new Vector3 (
//				Mathf.Sin (Time.frameCount * rotationSpeed * i) * 10.0F,
//				Mathf.Cos (Time.frameCount * rotationSpeed * i) * 22.0F,
//				1
//			);
//			ct.localScale = new Vector3 (
//				(Mathf.Sin (Time.frameCount * rotationSpeed * i) + 1.0F)* 8.0F,
//				(Mathf.Cos (Time.frameCount * rotationSpeed * i) + 1.0F) * 9.0F,
//				(Mathf.Sin (Time.frameCount * rotationSpeed * i) + 1.0F) * 10.0F
//			);

			i++;
		}
	}
}