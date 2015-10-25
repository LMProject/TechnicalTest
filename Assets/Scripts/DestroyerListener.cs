using UnityEngine;
using System.Collections;

/*

DestroyListener class is listening trigger events from missed collectables.
If trigger happens DestroyListener sends WasteCollectableGo message to the callback so
callback removes missed collectables from the system. In other words; if a player misses a collectable it needs to be removed.
Destroyer GO in the game is collecting them.

Another solution would be to check if collectable is behind the swinging character then destroy it.

 */


public class DestroyerListener : MonoBehaviour {

	GameObject callbackObject;
	// Use this for initialization
	void Start () {
	
	}

	public void Register (GameObject callbackObject)
	{
		this.callbackObject = callbackObject ;
	}

	void OnTriggerEnter(Collider other)
	{
		if (callbackObject == null)
		{
			Debug.LogError ("Callback Object is not set!");
			return ;
		}

		Collectable collectable = other.gameObject.GetComponent<Collectable>();
		if (collectable == null)
		{
			return ;
		}

		callbackObject.SendMessage ("WasteCollectableGo", other.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
