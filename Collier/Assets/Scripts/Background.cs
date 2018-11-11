using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour {

    SpriteRenderer sr;
    public float offset = 0f;
    public float shrink = 0f;

	// Use this for initialization
	void Start () {
        sr = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void LateUpdate() {
        float y = GameObject.FindGameObjectWithTag("MainCamera")
            .GetComponent<CameraScroll>().GetParallax(sr, offset, shrink);
        transform.position = new Vector3(transform.position.x, y - 1, transform.position.z);
	}
}
