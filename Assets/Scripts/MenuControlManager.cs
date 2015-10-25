using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/*

MenuControlManager handles all ui items.
Update method updates the text ui with score value in game within gameManager.

 */

public class MenuControlManager : MonoBehaviour {

	public GameObject playButton ;
	public GameObject arrowsImage ;
	public GameObject scoreText ;
	public GameObject gameOverText ;
	public GameObject replayButton ;

	// Use this for initialization
	void Start () {
	
	}

	public void PlayButtonPressed ()
	{
		playButton.SetActive (false);
		GetComponent<GameManager>().SetGameState (GameState.GS_Playing);
		arrowsImage.SetActive (true);
	}

	public void ReplayButtonPressed ()
	{
		GetComponent<CollectableManager>().ClearCollectables();
		GetComponent<GameManager>().Reset ();
		GetComponent<CollectableSpawner>().ResetSpawner ();
		arrowsImage.SetActive (true);
		replayButton.SetActive (false);
		gameOverText.SetActive (false);
		GetComponent<GameManager>().SetGameState (GameState.GS_Playing);

	}

	public void EnableArrowsImage (bool bEnable)
	{
		arrowsImage.SetActive (bEnable);
	}

	void Init ()
	{
		GetComponent<GameManager>().SetScore (0.0f);
	}

	public void ActivateGameOverText (bool bActivate)
	{
		gameOverText.SetActive (bActivate);
	}

	public void ActivateReplayButton (bool bActivate)
	{
		replayButton.SetActive (bActivate);
		
	}

	
	// Update is called once per frame
	void Update () {

		GameManager gameManager = GetComponent<GameManager>();
		GameState gameState = gameManager.GetGameState ();
		if (gameState == GameState.GS_Playing)
		{
			gameManager.AddScore (60.0f * Time.deltaTime * 1.0f);
			float fScore = gameManager.GetScore ();
			int iScoreRounded = (int)Mathf.Round(fScore);
			scoreText.GetComponent<Text>().text = iScoreRounded + " m";

		}
	
	}
}
