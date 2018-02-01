using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Text : MonoBehaviour {
	public Transform prefab;    //holds our initial gameobject
	List<Transform> clones = new List<Transform> ();
	int num = 30;    //set number of clones
	void Start () {  //do once when starting
		for (int i = 0; i < num; i++) {
			clones.Add (Instantiate (prefab, new Vector3 (0, 0, 0), Quaternion.identity));
		}
		prefab.gameObject.SetActive (false);  //hide prefab object
	}
	void RotateText(Transform clone, int shift) {
		string instr = clone.GetComponent<TextMesh>().text;
		string outstr = "";
		for (int j = 0; j < instr.Length; j++) {
			outstr = outstr + (instr [(j + shift) % instr.Length]);
		}
		clone.GetComponent<TextMesh>().text = outstr;
	}
	void Update () {  //do every frame - many times per second
		int i = 1;    //keep track of clone index

		////variant 1 - turn around in a circle
//		foreach(Transform ct in clones) {
//			ct.localEulerAngles = new Vector3 (0, 0, i*1.0F/num*360.0F);
//			i++;
//		}

		////variant 2 - scale up
//		        foreach(Transform ct in clones) {
//		            ct.localScale = new Vector3 (i*0.25F, i*0.25F, 1);
//		            i++;
//		        }

		////variant 3 - positioning
//		        foreach(Transform ct in clones) {
//		            ct.localPosition = new Vector3 (i, i%3, 1);
//		            i++;
//		        }

		////variant 4 - more positioning
//		        foreach(Transform ct in clones) {
//		            ct.localPosition = new Vector3 (i%7*1.5F, i%9*0.4F, 1);
//		            i++;
//		        }

		////variant 5 - animation position
//		        foreach (Transform ct in clones) {
//		            ct.localPosition = new Vector3 ((i % (Time.frameCount*0.01F%5) * 8.0F)-18, i % (Time.frameCount+(i*0.1F)%12) * 0.2F, 1);
//		            i++;
//		        }

		////variant 6 - animation scaling
//		        foreach (Transform ct in clones) {
//		            ct.localScale = new Vector3 ((Time.frameCount+(i*10))*0.01F%5, i % (Time.frameCount+(i*0.3F)%8)*0.25F, 1);
//		            i++;
//		        }

		////variant 7 - animation rotation
//		        foreach (Transform ct in clones) {
//		            ct.localEulerAngles = new Vector3 (0, 0, (Time.frameCount+(i*0.9F))*0.01F*360.0F);
//		            i++;
//		        }

		////variant 8 - more animation rotation
//		        foreach (Transform ct in clones) {
//		            ct.localEulerAngles = new Vector3 (0, 0, (Time.frameCount*(i*-0.0001F))*360.0F);
//		            i++;
//		        }

		//now combine form above and change some values
		        foreach (Transform ct in clones) {
		            ct.localScale = new Vector3 ((Time.frameCount+(i*10))*0.01F%5, i % (Time.frameCount+(i*0.3F)%8)*0.25F, 1);
		            ct.localEulerAngles = new Vector3 (0, 0, (Time.frameCount*(i*-0.0001F))*360.0F);
		            i++;
		        }

		//special - string reordering - can also go in Start to do it once
//		      for (int j = 0; j < num; j++) {
//		          RotateText(clones[j], 1);
//		      }

		//special - colour each text object
		        for (int j = 0; j < num; j++) {
		            //clones[j].GetComponent<TextMesh>().color= new Color(0.0F, j*1.0F/num, 0.0F);  //variants
		            //clones[j].GetComponent<TextMesh>().color= Color.HSVToRGB (j * 1.0F / num, 1.0F, 1.0F);
		            //clones[j].GetComponent<TextMesh>().color= Color.HSVToRGB (1.0F, j * 1.0F / num, 1.0F);
		            clones[j].GetComponent<TextMesh>().color= Color.HSVToRGB (((Time.frameCount+j)*0.05F)%1.0F, 1.0F, 1.0F);  //animation
		            //clones[j].GetComponent<TextMesh>().color= Color.HSVToRGB (Mathf.Sin(Time.frameCount*0.3F+(j*6.1F))%1, Mathf.Sin(Time.frameCount*0.1F+(j*10.01F))%1, 1.0F);
		            //clones[j].GetComponent<TextMesh>().color= Color.HSVToRGB (Mathf.Sin(Time.frameCount*0.1F+(j*6.1F))%1, Mathf.Sin(Time.frameCount*0.1F+(j*10.01F))%1, Mathf.Sin((Time.frameCount*0.02F)+(j*0.4F))*0.5F+0.5F);
		        }

	}
}