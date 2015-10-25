using UnityEngine;
using System.Collections;

/*

CollectableListener class is responsible for listening events coming from Collectable GO directly
If registered, it callbacks to collision event with sending "CollisionWithCollectable" event.

 */

public class CollectableListener : MonoBehaviour {

	GameObject callbackObject ;
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

		if (other.gameObject.GetComponent<Collectable>())
		{
			return ;
		}

		if (other.gameObject.GetComponent<DestroyerListener>() == null)
			callbackObject.SendMessage ("CollisionWithCollectable", gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
