using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour {
	public string townLocation = "1_Town"; // Make sure the town is build location 1

	public void LoadTown () 
	{
		SceneManager.LoadScene(townLocation);
	}

	public void ShowSettings () 
	{
		// TODO: Preserve collier's Position and Velowockitty

	}

	public void LoadCredits ()
	{
		// TODO
	}

	public void ExitGame () 
	{
		Application.Quit();
	}

}

