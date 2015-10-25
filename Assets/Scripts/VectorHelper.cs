using UnityEngine;
using System.Collections;

/*

VectorHelper is a static class that helps to come over some well known algebric problem. 
Currently it is called "VectorHelper". In the future, when its populated with other Helper methods, 
name of the class can be updated with a more common one.


 */
public static class VectorHelper {

	public static Vector3 RotatePointAroundPivot(Vector3 vPoint, Vector3 vPivot, Vector3 vAngles)
	{
		Vector3 vDir = vPoint - vPivot;
		vDir = Quaternion.Euler(vAngles) * vDir; 
		vPoint = vDir + vPivot; 
		return vPoint; 
	}
}
