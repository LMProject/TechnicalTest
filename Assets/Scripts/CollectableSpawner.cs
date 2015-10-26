using UnityEngine;
using System.Collections;
using DG.Tweening;

/*

CollectableSpawner is responsible for spawning collectibles in the game depending on the blended procedurel and manual patterns.
CollectableSpawner transforms the 2d patterns to 3d rotated positions (detailed at GetSpawnPosition method below)
CollectableSpawner also updates the collectables rotation when they are moving.

How spawning of collectables work?
1) 2d array "grids" parameter is set by given pattern type.

Example 2d array "grids" setup:
|------ 18 -----|

OO2OOOOOO...
OO1OOOOOO...
OO1OOOOOO...
OO1OOOOOO...
OO1OOOOOO...
OO1OOOOOO...
OO1OOOOOO...
OO1OOOOOO...

2) Depending on the 2d array "grids" data, visual of collectors are created from spawner by line by line.
3) Depending on the 2d array "grids" data, created collectors are transformed to the starting position.
4) After all of them are created and after some time interval, 2d array "grids" reset for next pattern.
5) Repeat Step 1-5 after small amount of time

 */
public class CollectableSpawner : MonoBehaviour {

	public GameObject GoldCoinPrefab ;
	public GameObject SlowPrefab ;
	public GameObject SpeedBoostPrefab ;

	public GameObject character ;
	public GameObject worldCylinder ;
	
	public float fRotatePerMin ;
	static float fIntervalBetweenCollectables ;
	float fCollectableSpawnTimer  ; 
	private PatternSystem patternSystem ;
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
		fRotatePerMin = 15.0f;
		fIntervalBetweenCollectables = 0.125f ; 
		fCollectableSpawnTimer = fIntervalBetweenCollectables;
		patternSystem = new PatternSystem();

		patternSystem.SetPattern (PatternType.PT_Line);
	}
 
	public void ResetSpawner ()
	{
		Init ();
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
	Transforms 2d tile spawned collectables to the required rotated position.
	 */
	void SpawnCollectable (int iX, CollectableType collectableType)
	{
		SpawnCollectable (iX * 10 - 90.0f, collectableType);
	}

	void SpawnCollectable (float fAngle, CollectableType collectableType)
	{
		Vector3 vStartPos = GetSpawnPosition (fAngle);
		GameObject collectable = null ;
		if (collectableType == CollectableType.CT_GOLD)
			collectable = (GameObject)GameObject.Instantiate (GoldCoinPrefab, vStartPos, Quaternion.Euler(90, 0, 0));

		if (collectableType == CollectableType.CT_SLOW)
			collectable = (GameObject)GameObject.Instantiate (SlowPrefab, vStartPos, Quaternion.Euler(90, 0, 0));

		if (collectableType == CollectableType.CT_SPEEDBOOST)
			collectable = (GameObject)GameObject.Instantiate (SpeedBoostPrefab, vStartPos, Quaternion.Euler(90, 0, 0));

		GetComponent<CollectableManager>().AddCollectable (collectable, collectableType);
		collectable.GetComponent<CollectableListener>().Register (gameObject);
	}

	public void InitBeforePlay ()
	{
		patternSystem.ResetHardness ();
		StartToCountHardness ();
	}

	public void StartToCountHardness ()
	{
		StopCoroutine ("IncreaseHardnessCoroutine");
		StartCoroutine ("IncreaseHardnessCoroutine",5.0f);
	}
	
	IEnumerator IncreaseHardnessCoroutine (float fTime)
	{ 		
		yield return new WaitForSeconds(fTime); 
		patternSystem.IncreaseHardness ();
		StartToCountHardness ();


	}
 
	/*
		DOTween is used to tween between values over time, which is a smooth interpolation between values.
	 */
	public void SetWorldSpeed (float fSpeed)
	{
		DOTween.Kill (fRotatePerMin, false);
		DOTween.To(()=> fRotatePerMin, x=> fRotatePerMin = x, fSpeed, 0.5f);
	}

	public float GetWorldSpeed ()
	{
		return fRotatePerMin ;
	}

	public void EnableWorldCylinderRotate(bool bEnabled)
	{
		worldCylinder.GetComponent<Rotater>().enabled = bEnabled ;
	}

	IEnumerator NextPatternCoroutine (float fTime)
	{ 		
		yield return new WaitForSeconds(fTime); 

		int iDecisionVal = Random.Range (1,101);
  
		if (iDecisionVal > 66 )
			patternSystem.SetPattern (PatternType.PT_FullLine);
		else if (iDecisionVal > 33 )
		{
			int iRandVal = Random.Range (1,4);
			if (iRandVal == 1)
			{
				patternSystem.SetPattern (PatternType.PT_Line);
			}
			if (iRandVal == 2)
			{
				patternSystem.SetPattern (PatternType.PT_DoubleLine);
			}
			if (iRandVal == 3)
			{
				patternSystem.SetPattern (PatternType.PT_TripleLine);
			}
		}
		else if (iDecisionVal > 0 )
		{
			int iRandVal = Random.Range (1,3);
			if (iRandVal == 1)
			{
				patternSystem.SetPattern (PatternType.PT_X);
			} 
			if (iRandVal == 2)
			{
				patternSystem.SetPattern (PatternType.PT_X_Red);
			} 
		}
	}
	 
	// Update is called once per frame
	void Update () 
	{
		GameManager gameManager = GetComponent<GameManager> ();
		if (gameManager.GetGameState() == GameState.GS_Playing)
		{
			if (!patternSystem.GetPatternComplete())
			{
				fCollectableSpawnTimer -= Time.deltaTime ;
				if (fCollectableSpawnTimer < 0.0001f)
				{
					int iSeq = patternSystem.GetSequenceY ();
					patternSystem.IncrementSequenceY ();
					Grid[,] grids = patternSystem.GetGrids ();
					
					for (int x = 0 ; x < patternSystem.GetMaxX() ; x++)
					{
						if (grids[x,iSeq].collectableType != CollectableType.CT_NONE)
						{
							SpawnCollectable (x, grids[x,iSeq].collectableType);
						}
					}
					fCollectableSpawnTimer = fIntervalBetweenCollectables / (fRotatePerMin * 0.1f) ;	

					if (patternSystem.GetSequenceY () == 0)
						StartCoroutine ("NextPatternCoroutine", patternSystem.GetTimeBetweenPatterns());
				}
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
