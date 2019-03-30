using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class SceneChanger : MonoBehaviour {
	public string townLocation = "1_Town"; // Make sure the town is build location 1
	public Rigidbody2D player;
	private Vector2 vel;
	public bool openUI = false;

    public GameObject trans;

	public GameObject settingsScreen;
	public GameObject creditScreen;

	void Awake()
	{
        if (settingsScreen != null)
        {
            settingsScreen.SetActive(false);
        }
        if (creditScreen != null)
        {
            creditScreen.SetActive(false);
        }
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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

	public void LoadTown () 
	{
        Instantiate(trans).GetComponent<SceneTransition>()
            .Initialize("1_Town");
    }
	
	public void ShowCredits () {
        creditScreen.SetActive(true);
        openUI = true;
	}

	public void HandleExit () 
	{
        if (creditScreen.activeSelf)
        {
            creditScreen.SetActive(false);
            openUI = false;
        }
		else if (Player.InLevel())
		{
			Application.Quit();
		} 
		else 
		{
			SceneManager.LoadScene("1_Town");
		}
	}

}

