using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doni : MonoBehaviour {

    BoxCollider2D box;
    int direction = 1;
    float max = 2.5f;
    float min = 1.5f;

	// Use this for initialization
	void Start () {
        float x = Random.Range(0f, 1f);
        transform.localScale = new Vector3(x, x, 1);
    }
	
	// Update is called once per frame
	void Update () {
        Player player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Player>();
        if (player != null)
        {
            float x = transform.localScale.x;
            if (transform.localScale.x > max)
            {
                direction = -1;
            }
            if (transform.localScale.x < min)
            {
                direction = 1;
            }
            x += direction * 2 * Time.deltaTime;
            transform.localScale = new Vector3(x, x, 1);
        }
	}
}
