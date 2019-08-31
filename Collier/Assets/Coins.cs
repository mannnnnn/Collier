using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coins : MonoBehaviour {

    Text text;
    public int coins = 0;
    public int tempCoins = 0;

    public Sprite[] sprites;

    float timer = 0f;
    public float duration = 0.1f;

    int index = 0;

    Image image;

    // Use this for initialization
    void Start () {
        text = GetComponentInChildren<Text>();
        image = GetComponentInChildren<Image>();
		coins = PlayerPrefs.GetInt("coins");
    }
	
	// Update is called once per frame
	void Update () {
        text.text = coins.ToString();
        timer += Time.deltaTime;
        if (timer > duration)
        {
            timer = 0f;
            index = (index + 1) % sprites.Length;
        }
        image.sprite = sprites[index];
    }
}
