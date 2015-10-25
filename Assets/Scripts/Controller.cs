using UnityEngine;
using System.Collections;

/*

Controller is the upper class of all Manager classes.
All Manager classes are initialized here when necessary.
This method is set from Edit->Project Settings->Script Execution Order-> as -100 to be 
sure it is executed first of all Managers.

 */
public class Controller : MonoBehaviour {
  
	//init order
	void Awake ()
	{ 
		GetComponent<GameManager>().Init();
		GetComponent<CollectableManager>().Init();
		GetComponent<CollectableSpawner>().Init();
	}

	// Use this for initialization
	void Start () {
	
	}
  
	// Update is called once per frame
	void Update () {
	
	}
}
