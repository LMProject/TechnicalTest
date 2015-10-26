using UnityEngine;
using System.Collections;

/*

There are 3 types of collectable in the game.
CT_GOLD increases the sun position so give you more time to play.
CT_SPEEDBOOST increase the speed of the world for short time interval. 
CT_SLOW decreases the speed of the world for short time interval so it is a debuff. Better to avoid of it.

 */

public enum CollectableType
{
	CT_NONE = 0,
	CT_GOLD = 1,
	CT_SPEEDBOOST = 2,
	CT_SLOW = 3,
}

/*

Collectable class is the base class for collectable items in the game. 
There are 3 types as mentioned above.

 */

public class Collectable : MonoBehaviour {

	protected CollectableType collectableType ;
	public Collectable ()
	{

	}

	public CollectableType GetCollectableType ()
	{
		return collectableType;
	}

	void Start () 
	{
		
	}

	void Update ()
	{

	}
}
