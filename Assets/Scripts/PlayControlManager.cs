﻿using UnityEngine;
using System.Collections;

/*

PlayControlManager gets the input from keyboard and manages the swinging character.
It apply perpendicular force vector to the swinging character, able him/her to swing.

 */

public class PlayControlManager : MonoBehaviour {
	
	private float fKeyForce = 90.361f ;
	public GameObject character;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		GameState gameState = GetComponent<GameManager>().GetGameState ();
		
		if (gameState == GameState.GS_Playing)
		{ 
			if (Input.GetKey(KeyCode.LeftArrow))
			{
				GetComponent<MenuControlManager>().EnableArrowsImage (false);

				float fAngleZ = character.transform.localEulerAngles.z ;
				Vector2 vForce = new Vector2 (-fKeyForce * Time.deltaTime , 0.0f);
				
				Vector2 vPerpDir = Quaternion.AngleAxis(fAngleZ, Vector3.forward) * vForce;
				character.GetComponent<Rigidbody>().AddForce (new Vector3 (vPerpDir.x, vPerpDir.y, 0.0f));
			}
			
			if (Input.GetKey(KeyCode.RightArrow))
			{
				GetComponent<MenuControlManager>().EnableArrowsImage (false);

				float fAngleZ = character.transform.localEulerAngles.z ;
				Vector2 vForce = new Vector2 (fKeyForce * Time.deltaTime, 0.0f);
				
				Vector2 vPerpDir = Quaternion.AngleAxis(fAngleZ, Vector3.forward) * vForce;
				character.GetComponent<Rigidbody>().AddForce (new Vector3 (vPerpDir.x, vPerpDir.y, 0.0f));
			}
		}
	}
	
}
