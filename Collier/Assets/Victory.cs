using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Victory : MonoBehaviour {
    public GameObject trans;
    public int maxScore;

    float timer = 0f;
    float duration = 0.75f;
    int index = 0;

    public Sprite star1;
    public Sprite star2;
    public Sprite star3;

    int health;
    int coins;

    // Use this for initialization
    void Start () {
        Health h = GameObject.FindGameObjectWithTag("Health").GetComponent<Health>();
        Coins c = GameObject.FindGameObjectWithTag("Coins").GetComponent<Coins>();
        health = h.health;
        coins = c.coins;
        h.gameObject.SetActive(false);
        c.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if (timer > duration)
        {
            timer = 0f;
            index++;
            switch (index)
            {
                case 1:
                    transform.Find("Star1").GetComponent<Image>().sprite = star1;
                    break;
                case 2:
                    if (health + coins > Mathf.CeilToInt(maxScore / 2f))
                    {
                        transform.Find("Star2").GetComponent<Image>().sprite = star2;
                    }
                    break;
                case 3:
                    if (health + coins >= maxScore)
                    {
                        transform.Find("Star3").GetComponent<Image>().sprite = star3;
                    }
                    break;
            }
        }
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
