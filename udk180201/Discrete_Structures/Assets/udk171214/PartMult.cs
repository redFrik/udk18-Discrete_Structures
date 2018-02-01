using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartMult : MonoBehaviour {
	public GameObject prefab;
	public float spread= 1.0F;
	public float scale = 5.0F;
	int num= 10;  //number of objects (and particle systems)
	List<GameObject> list= new List<GameObject>();
	void Start() {
		for (int i = 0; i < num; i++) {
			list.Add (Instantiate (prefab, new Vector3 (0, 0, 0), Quaternion.identity));
		}
		prefab.gameObject.SetActive (false);  //hide prefab object
	}
	void Update () {
		float i = 0;
		foreach (GameObject go in list) {
			i++;
			float theta = (Time.time * spread) + ((i / num)*Mathf.PI*2);
			ParticleSystem ps = go.GetComponent<ParticleSystem> ();
			ps.startColor = new Color(1-(i/num), 0.1F, i/num, 1.0F);
			var em= ps.emission; em.rateOverTime= i*10;
			//var sh = ps.shape; sh.arc = i*10;

			go.transform.localPosition = new Vector3 (
				Mathf.Sin(theta)*scale,
				Mathf.Cos(theta)*scale,
				1
			);
		}
	}
}