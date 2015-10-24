using UnityEngine;
using System.Collections;

public enum GameState 
{
	GS_PlayMenu = 1,
	GS_Playing = 2,
	GS_FailedMenu = 3,
}

public class GameManager 
{
	private GameState gameState ;

	public GameManager ()
	{
		gameState = GameState.GS_PlayMenu;
	}

	public GameState GetGameState ()
	{
		return gameState;
	}

	public void SetGameState (GameState gameState)
	{
		this.gameState = gameState ;
	}

}
