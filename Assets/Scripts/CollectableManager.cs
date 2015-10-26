using UnityEngine;
using System.Collections; 

/*

CollectableManager is the host for collectables that is created with CollectableSpawner.
CollectableManager manages the collectables in an ArrayList.
CollectableManager is able to add, remove collectible to the list.
CollectableManager also decide what to do if a collectable is collected by a player. 

 */

public class CollectableManager : MonoBehaviour {

	ArrayList collectables = null;
	public GameObject Destroyer = null ;

	// Use this for initialization
	void Start () {
	
	}

	public void Init ()
	{
		collectables = new ArrayList ();
		Destroyer.GetComponent<DestroyerListener>().Register (gameObject);
	}

	public ArrayList GetCollectables ()
	{
		return collectables ;
	}

	public void ClearCollectables ()
	{
		foreach (GameObject collectable in collectables) 
		{
			GameObject.Destroy (collectable);
		}

		collectables.Clear ();
	}

	public void WasteCollectableGo (GameObject collectableGo)
	{
		GameObject.Destroy (collectableGo);
		collectables.Remove (collectableGo);
	}

	IEnumerator SpeedBoostCoroutine (float fTime)
	{ 		
		yield return new WaitForSeconds(fTime); 
		GetComponent<CollectableSpawner>().SetWorldSpeed (15.0f);
	}

	IEnumerator SlowCoroutine (float fTime)
	{ 		
		yield return new WaitForSeconds(fTime); 
		GetComponent<CollectableSpawner>().SetWorldSpeed (15.0f);
	}
		
	void CollisionWithCollectable (GameObject collidedCollectible)
	{
		Collectable collectable = collidedCollectible.GetComponent<Collectable>();
		switch (collectable.GetCollectableType()) 
		{
		case CollectableType.CT_GOLD:

			GetComponent<GameManager>().AddSunIncrease (2.0f);

			break;

		case CollectableType.CT_SPEEDBOOST:

			float worldSpeed = GetComponent<CollectableSpawner>().GetWorldSpeed ();
			if (worldSpeed < 3.0f)
				GetComponent<CollectableSpawner>().SetWorldSpeed (15.0f);
			else
				GetComponent<CollectableSpawner>().SetWorldSpeed (22.0f);
			
			StopCoroutine ("SlowCoroutine");
			StopCoroutine ("SpeedBoostCoroutine");
			StartCoroutine ("SpeedBoostCoroutine", 1.0f);

			break ;
		case CollectableType.CT_SLOW:

			GetComponent<CollectableSpawner>().SetWorldSpeed (2.0f);
			StopCoroutine ("SlowCoroutine");
			StopCoroutine ("SpeedBoostCoroutine");
			StartCoroutine ("SlowCoroutine", 1.0f);
				
			break ;
		default:
			break;
		}
		collectables.Remove (collidedCollectible);
		GameObject.Destroy (collidedCollectible);
	}

	public void AddCollectable (GameObject collectableGo, CollectableType collectableType)
	{  
		collectables.Add (collectableGo); 
	} 
		
	// Update is called once per frame
	void Update () {
	
	}
}
