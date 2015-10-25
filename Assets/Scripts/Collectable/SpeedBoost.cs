using UnityEngine;
using System.Collections;


/*

SpeedBoost is the derived class from Collectable.
It is one of the collectible type.

 */

public class SpeedBoost : Collectable {

	public SpeedBoost ()
	{
		collectableType = CollectableType.CT_SPEEDBOOST;
	}

}
