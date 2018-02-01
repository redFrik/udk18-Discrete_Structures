using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Phasing3D : MonoBehaviour {
	public Transform prefab;    //holds our initial gameobject
	List<Transform> clones = new List<Transform>();
	int dim = 10;    //10x10*10= 1000 objects in total
	public Vector3 rotationSpeed;  //rotation in x y z
	public float spread= 0.0F;     //phasing offset
	public Vector3 scaling;        //scaling in x y z
	void Start() {  //do once when starting
		float spacing= 1.25F;    //space between gameobjects
		float scale = 0.5F;     //size of gameobject
		rotationSpeed.x= 1.0F;
		rotationSpeed.y= 1.0F;
		rotationSpeed.z= 1.0F;
		scaling.x= 2.0F;
		scaling.y= 0.1F;
		scaling.z= 0.5F;
		prefab.transform.localScale = new Vector3(scale, scale, scale);
		for (int x = 0; x < dim; x++) {
			for (int y = 0; y < dim; y++) {
				for (int z = 0; z < dim; z++) {

					////variant 1
					clones.Add(Instantiate (prefab, new Vector3(x-(dim/2), y-(dim/2), z-(dim/2))*spacing, Quaternion.identity));

					////variant 2
//					float xx = Mathf.Sin ((x * dim + y) / Mathf.PI * 2 * dim * dim);
//					float yy = Mathf.Cos ((x * dim + y) / Mathf.PI * 2 * dim * dim);
//					float zz = Mathf.Sin (z / (1.0F * dim)*Mathf.PI);
//					clones.Add(Instantiate(prefab, new Vector3(xx*zz, yy*zz, z/(1.0F*dim)*2-1)*4.5F*spacing, Quaternion.identity));
				}
			}
		}
		transform.position= new Vector3(0, 0, -10);  //default camera position
	}
	void Update() {  //do every frame - many times per second
		int i= 0;
		foreach(Transform ct in clones) {
			ct.localEulerAngles = rotationSpeed*(Time.frameCount+(i*spread));
			ct.localScale = scaling*((i+1)/(1.0F*dim*dim*dim));
			i++;
		}
	}
}