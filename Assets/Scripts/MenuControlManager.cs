using UnityEngine;
using System.Collections;

public class MenuControlManager : MonoBehaviour {

	public GameObject playButton ;
	public GameObject arrowsImage ;


	// Use this for initialization
	void Start () {
	
	}

	public void PlayButtonPressed ()
	{
		playButton.SetActive (false);
		GetComponent<Controller>().SetGameState (GameState.GS_Playing);
		arrowsImage.SetActive (true);
	}

	public void EnableArrowsImage (bool bEnable)
	{
		arrowsImage.SetActive (bEnable);
	}

	
	// Update is called once per frame
	void Update () {
	
	}
}
