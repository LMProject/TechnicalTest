using UnityEngine;
using System.Collections;

/*

CollectableSpawner is responsible for spawning collectibles in the game depending on the blended procedurel and manual patterns.
CollectableSpawner transforms the 2d patterns to 3d rotated positions (detailed at GetSpawnPosition method below)
CollectableSpawner also updates the collectables rotation when they are moving.

 */
public class CollectableSpawner : MonoBehaviour {

	public GameObject GoldCoinPrefab ;
	public GameObject SlowPrefab ;
	public GameObject SpeedBoostPrefab ;

	public GameObject character ;
	public GameObject worldCylinder ;
	
	public float fRotatePerMin ;
	static float fIntervalBetweenCollectables ;
	float fIntervalBetweenPatterns ; // will be used with patterns

	float fCollectableSpawnTimer = fIntervalBetweenCollectables ; 
	// Use this for initialization

	float fCenterPivotY ; // y param. of the pivot that is used for swinging
	float fBottomCharacterY ; // y param. of the bottom of the character.

	void Start () 
	{	
		fCenterPivotY = character.GetComponent<HingeJoint>().anchor.y + character.transform.localPosition.y ;
		fBottomCharacterY = character.transform.localPosition.y - character.transform.localScale.y ;
	}

	public void Init ()
	{
		fRotatePerMin = 10.0f;
		fIntervalBetweenCollectables = 0.25f ;
		fIntervalBetweenPatterns = 3.0f ;
		fCollectableSpawnTimer = fIntervalBetweenCollectables ;
	}

	/*
		 Collectable positions are first created around the swinging character and then they are carried 
		 to the desired position by transformation depending on the patterns. So that character can collect them.
		 GetSpawnPosition method gets the collectable's new position rotated around swinging character with given angle parameter. 

		Ex: 
		   angle = 30		   
		   pivot
			 *
			  \
			   \
			    \
			     \
				  \
				   O collectable gold


			OR
			
			angle = 90
			pivot
			 * - - - - - - O collectable gold

	 */
	Vector3 GetSpawnPosition (float fAngle)
	{
		Vector3 vBottom = new Vector3 (0,fBottomCharacterY,0);
		Vector3 vPivot = new Vector3 (0,fCenterPivotY,0);
		Vector3 vAngle = new Vector3 (0,0,fAngle);
		
		Vector3 vStartPos = VectorHelper.RotatePointAroundPivot (vBottom,vPivot, vAngle  );
		vStartPos = VectorHelper.RotatePointAroundPivot (vStartPos, worldCylinder.transform.localPosition, new Vector3(180,0,0) );
		return vStartPos ;
	}
	 /*
	Transforms 2d tile spawned collectables to the required angle.
	 */
	void SpawnCollectable (uint uiX, uint uiY)
	{


	}

	void SpawnCollectable (float fAngle)
	{
		Vector3 vStartPos = GetSpawnPosition (fAngle);
		GameObject collectable = (GameObject)GameObject.Instantiate (GoldCoinPrefab, vStartPos, Quaternion.Euler(90, 0, 0));
		GetComponent<CollectableManager>().AddCollectable (collectable, CollectableType.CT_GOLD);
		collectable.GetComponent<CollectableListener>().Register (gameObject);
	}

	/*
	Under Construction method, will be updated, no use currently.
	 */
	void CreatePattern ()
	{
		int[,] grids = new int[18,18]; 
		
		for (int i = 0 ; i < 18 ; i++)
		{
			for (int k = 0; k < 18; k++) {
				grids[i,k] = 0 ;
			}
		}

		int iStartX = Random.Range (0, 18);
		int iStartY = Random.Range (0, 18);
	}

	public void ResetSpawner ()
	{
		Init ();
	}
 
	// Update is called once per frame
	void Update () 
	{
		GameManager gameManager = GetComponent<GameManager> ();
		if (gameManager.GetGameState() == GameState.GS_Playing)
		{
			fCollectableSpawnTimer -= Time.deltaTime ;
			if (fCollectableSpawnTimer < 0.0001f)
			{
				SpawnCollectable (45.0f);
				fCollectableSpawnTimer = fIntervalBetweenCollectables ;
			}
			
			ArrayList collectableGos = GetComponent<CollectableManager>().GetCollectables();
			
			foreach (GameObject collectableGo in collectableGos) 
			{
				collectableGo.transform.RotateAround(worldCylinder.transform.localPosition, Vector3.right, -6.0f * fRotatePerMin * Time.deltaTime);
				collectableGo.transform.localEulerAngles = new Vector3 (90,0,0);
				
				
			}
		}


		 
	}
}
