using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {

	GameManager gameManager = null;

	void Awake ()
	{
		gameManager = new GameManager ();
	}

	// Use this for initialization
	void Start () {
	
	}

	public void SetGameState (GameState gameState)
	{
		gameManager.SetGameState (gameState);
	}

	public GameState GetGameState ()
	{
		return gameManager.GetGameState();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
