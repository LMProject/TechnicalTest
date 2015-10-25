using UnityEngine;
using System.Collections;

/*

Slow is the derived class from Collectable.
It is one of the collectible type.

 */

public class Slow : Collectable {

	public Slow ()
	{
		collectableType = CollectableType.CT_SLOW;
	}
}
