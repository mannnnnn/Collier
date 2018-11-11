using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadTown : MonoBehaviour {

    public GameObject trans;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Exit()
    {
        Instantiate(trans).GetComponent<SceneTransition>()
            .Initialize("1_Town");
    }
}
