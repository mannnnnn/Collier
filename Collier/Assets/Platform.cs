using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

    BoxCollider2D box;

	// Use this for initialization
	void Start () {
         /*box = GetComponent<BoxCollider2D>();*/
    }
	
	// Update is called once per frame
	void Update () {
        /*I've got a bad feeling about this code
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            box.enabled = transform.position.y < player.transform.position.y;
        }
         */
    }
}
