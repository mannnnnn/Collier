using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Victory : MonoBehaviour {
    public GameObject trans;
    public int maxScore;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Restart()
    {
        Instantiate(trans).GetComponent<SceneTransition>()
            .Initialize(SceneManager.GetActiveScene().name);
    }

    public void Exit()
    {
        Instantiate(trans).GetComponent<SceneTransition>()
            .Initialize("1_Town");
    }
}
