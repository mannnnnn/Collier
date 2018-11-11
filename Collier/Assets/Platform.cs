using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

    BoxCollider2D box;

	// Use this for initialization
	void Start () {
        box = GetComponent<BoxCollider2D>();
    }
	
	// Update is called once per frame
	void Update () {
        box.enabled = transform.position.y
            < GameObject.FindGameObjectWithTag("Player").transform.position.y;
    }
}
