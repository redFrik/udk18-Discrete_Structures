using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lines : MonoBehaviour {
	public float startwidthspeed= 0.01F;
	public float startwidthamount= 4.0F;
	public float endwidthspeed= 0.011F;
	public float endwidthamount= 4.0F;
	public Vector3 rotationspeeds= new Vector3(0.4F, 0.5F, 0.6F);  //new public variable
	public Color startcolor = new Color (0.1F, 0.2F, 0.3F, 0.01F);
	public Color endcolor = new Color (0.9F, 0.8F, 0.7F, 0.01F);
	int num= 500;
	LineRenderer line;
	void Start () {
		line = GetComponent<LineRenderer> ();
		line.positionCount = num;
		line.loop = true;
		line.numCornerVertices = 50;
		line.useWorldSpace= false;  //need to be off for rotation to work
	}
	void Update () {
		transform.localEulerAngles = rotationspeeds * (1.0F*Time.frameCount);  //added this
		line.startWidth = Mathf.Sin(Time.frameCount*startwidthspeed)*startwidthamount;
		line.endWidth = Mathf.Sin(Time.frameCount*endwidthspeed)*endwidthamount;
		line.startColor = startcolor;
		line.endColor = endcolor;
		for (int i = 0; i < num; i++) {
			line.SetPosition (i, new Vector3 ((i % 15) * 2 - 1, (i % 16) * 2 - 1, (i % 17) * 2 - 1));
		}
	}
}
