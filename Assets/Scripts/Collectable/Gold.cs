using UnityEngine;
using System.Collections;

/*

Gold is the derived class from Collectable.
It is one of the collectible type.

 */
public class Gold : Collectable 
{
	public Gold ()
	{
		collectableType = CollectableType.CT_GOLD;
	}
}
