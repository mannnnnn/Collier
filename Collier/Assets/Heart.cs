using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heart : MonoBehaviour {

    float timer = 0f;
    public float duration = 0.1f;

    public Sprite[] fullSprites;
    public Sprite[] deadSprites;

    int index = 0;

    Image image;

    public bool full = false;
    public bool poisoned = false;

	// Use this for initialization
	void Start () {
        image = GetComponent<Image>();
	}

	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if (timer > duration)
        {
            timer = 0f;
        }


        if (poisoned){
            image.color = Color.green;
        }
        else if (full)
        {
            image.sprite = fullSprites[index];
             image.color = Color.white;
        }
        else {
            image.sprite = deadSprites[index];
            image.color = Color.white;
        }
    }
}
