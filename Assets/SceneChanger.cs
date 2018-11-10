using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour {
	public int townLocation = 1; // Make sure the town is build location 1

	public void LoadTown () 
	{
		
		SceneManager.LoadScene(townLocation);
	}

}

