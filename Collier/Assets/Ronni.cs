using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ronni : MonoBehaviour {

    BoxCollider2D box;
    int direction = 1;

	// Use this for initialization
	void Start () {
        box = GetComponent<BoxCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {
        Player player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Player>();
        float x = transform.position.x;
        if (player != null)
        {
            float left = player.leftWall.GetComponent<BoxCollider2D>().bounds.max.x;
            float right = player.rightWall.GetComponent<BoxCollider2D>().bounds.min.x;
            if (transform.position.x - box.bounds.extents.x < left)
            {
                // if on the left go right
                direction = 1;
            }
            if (transform.position.x + box.bounds.extents.x > right)
            {
                // if on the right go left
                direction = -1;
            }
            x += direction * 2 * Time.deltaTime;
        }
        transform.position = new Vector3(x, transform.position.y, transform.position.y);
	}
}
