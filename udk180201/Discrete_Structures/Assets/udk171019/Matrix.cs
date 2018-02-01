using UnityEngine;
using System.Collections;

public class Matrix : MonoBehaviour {
	public Transform prefab;
	public Vector3 camrot;  //camera rotation x y z
	void Start() {
		int dim = 10;  //10x10x10= 1000. careful when increasing this - not too much
		camrot.x = 0.3F;
		camrot.y = 0.2F;
		camrot.z = 0.1F;        //note the third dimension
		float spacing= 5.0F;    //try changing this - around 2.0 is good
		float scale = 1.0F;        //and this - 0.1 is a good value
		prefab.transform.localScale = new Vector3 (scale, scale, scale);
		for (int x = 0; x < dim; x++) {
			for (int y = 0; y < dim; y++) {
				for (int z = 0; z < dim; z++) {    //note the third dimension
					Instantiate(prefab, new Vector3(x-(dim/2), y-(dim/2), z-(dim/2))*spacing, Quaternion.identity);
				}
			}
		}
		transform.position= new Vector3(0, 0, 0);
	}
	void Update() {
		transform.localEulerAngles= camrot*Time.frameCount;
	}
}