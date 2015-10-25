using UnityEngine;
using System.Collections;

/*

Simple class that can be attached to any game object enable it to rotate around Y axis.
Well, this is also possible with attached ridigbody and Hinge Joint to a GO 
with configuring the Hinge Joint component's Motor value.


 */
public class Rotater : MonoBehaviour {

	public float fRotatePerMin = 10.0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(0,6.0f * fRotatePerMin * Time.deltaTime,0);
	}
}
