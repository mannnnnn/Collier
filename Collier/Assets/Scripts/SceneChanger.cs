using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class SceneChanger : MonoBehaviour {
	public string townLocation = "1_Town"; // Make sure the town is build location 1
	public Rigidbody2D player;
	private Vector2 vel;
	private bool openUI = false;

	public GameObject settingsScreen;
	public GameObject creditScreen;

	void Awake()
	{
		settingsScreen.SetActive(false);
		creditScreen.SetActive(false);
	}

	public void Start () 
	{
		if (GameObject.FindWithTag("Player") != null) {
			// I'm gonna assume that there's only ever gonna be one player 
			// If I'm wrong I will fight all of you
			player = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
		} else {
			player = null;
		}
	}

	public void LoadTown () 
	{
		SceneManager.LoadScene(townLocation);
	}

	public void OpenMenu ()
	{
		if(!openUI) {
			vel = player.velocity;
			player.velocity = new Vector2 (0, 0);
			openUI = true;

			if (EventSystem.current.currentSelectedGameObject.name == "Exit") 
			{
				HandleExit();
			}
			else if (EventSystem.current.currentSelectedGameObject.name == "Settings")
			{
				ShowSettings();
			}
			else if (EventSystem.current.currentSelectedGameObject.name == "Credits")
			{
				ShowCredits();
			}
		} 
		else 
		{	

			player.velocity = vel;
			openUI = false;

		}


	}

	private void ToggleMenu () {
		
	}

	private void ShowSettings () {
		if (creditScreen.active) {
			creditScreen.SetActive(true);
		}

		settingsScreen.SetActive(false);
	}
	
	private void ShowCredits () {
		if (settingsScreen.active) {
			settingsScreen.SetActive(true);
		}

		creditScreen.SetActive(true);
	}

	private void HandleExit () 
	{
		if (openUI) 
		{
			OpenMenu();
		} 
		else if(SceneManager.GetActiveScene().name == "1_Town")
		{
			Application.Quit();
		} 
		else 
		{
			SceneManager.LoadScene("1_Town");
		}
	}

}

