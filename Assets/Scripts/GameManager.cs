using UnityEngine;
using System.Collections;

/*

GameState enumeration consist of 3 types.
GS_PlayMenu; stating that player is at the main menu.
GS_Playing; stating that game is started and player is playing with swinging character.
GS_FailedMenu; stating that game is over and Replay Menu is shown to the user.

 */
public enum GameState 
{
	GS_PlayMenu = 1,
	GS_Playing = 2,
	GS_FailedMenu = 3,
}

/*

Game class is consist of Game attributes as game state and score of current player.
gameState; host of the current game state
fScore; player current score
 */
public class Game
{
	public GameState gameState ;
	public float fScore ;
}

/*

SunParams class is consist of sun parameters.
fSunSpeed; speed of the sun, downward.
fSunIncrease; is increased by collecting gold collectable.
fSunMax; sun maximum reach value in Y
fSunMin; sun minimum value, below that means game is over

 */

public class SunParams
{
	public float fSunSpeed ;
	public float fSunIncrease ;
	public Vector3 vSunResetPosition ;
	public float fSunMax ;
	public float fSunMin ; 
}
/*

Game manager manages the game and sunParams variables.
Handles the sun position in its Update method.

 */
public class GameManager : MonoBehaviour
{
	Game game ; 
	SunParams sunParams ;
	public GameObject sunObject ; 
 
	void Start ()
	{
		sunParams.vSunResetPosition = sunObject.transform.localPosition ;
	}

	public void Init ()
	{
		game = new Game ();
		sunParams = new SunParams ();

		game.gameState = GameState.GS_PlayMenu;
		game.fScore = 0.0f ;
		sunParams.fSunIncrease = 0.0f ;
		sunParams.fSunSpeed = -60.0f * 0.04f ;
		sunParams.fSunMax = 16.3f ;
		sunParams.fSunMin = -2.5f ;

		Application.targetFrameRate = 60;
	}

	public void Reset ()
	{
		ResetScore ();
		ResetSunPosition ();
		game = null ;
		sunParams = null ;
		Init ();
	}
 
	public GameState GetGameState ()
	{
		return game.gameState;
	}

	public void SetGameState (GameState gameState)
	{
		game.gameState = gameState ;
	}

	public void AddScore (float fScore)
	{
		game.fScore += fScore ;
	}

	public void SetScore (float fScore)
	{
		game.fScore = fScore ;
	}

	public float GetScore ()
	{
		return game.fScore ;
	}

	public void ResetScore ()
	{
		game.fScore = 0 ;
	}

	public void ResetSunPosition ()
	{
		sunObject.transform.localPosition = sunParams.vSunResetPosition;
	}

	public void AddSunIncrease (float fSunIncreaseAdd)
	{
		sunParams.fSunIncrease += fSunIncreaseAdd;
	}

	public void GameOver ()
	{
		GetComponent<MenuControlManager>().ActivateGameOverText (true);
		GetComponent<MenuControlManager>().ActivateReplayButton (true);

	}

	void Update ()
	{
		if (game.gameState == GameState.GS_Playing)
		{
			sunParams.fSunIncrease += sunParams.fSunSpeed * Time.deltaTime * 2.0f ;
			if (sunParams.fSunIncrease < 0.0f)
				sunParams.fSunIncrease = 0.0f ;

			Vector3 vPos = sunObject.transform.localPosition ;
			vPos.y += sunParams.fSunSpeed * Time.deltaTime + sunParams.fSunIncrease * Time.deltaTime ;

			if (vPos.y < sunParams.fSunMin)
			{
				GameOver ();
				SetGameState (GameState.GS_FailedMenu);
				return ;
			}
			if (vPos.y > sunParams.fSunMax)
			{
				vPos.y = sunParams.fSunMax ;
				sunParams.fSunIncrease = 0.0f ;
			}
			sunObject.transform.localPosition = vPos;
		}
	}

}
