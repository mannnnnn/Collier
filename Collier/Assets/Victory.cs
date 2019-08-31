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
                    SaveLoad.levelUnlocked[SceneManager.GetActiveScene().name] = index;
                    PlayerPrefs.SetInt(SceneManager.GetActiveScene().name, index);
                    break;
                case 2:
                    if (health + coins > Mathf.CeilToInt(maxScore / 2f))
                    {
                        transform.Find("Star2").GetComponent<Image>().sprite = star2;
                        SaveLoad.levelUnlocked[SceneManager.GetActiveScene().name] = index;
                        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name, index);
                    }
                    break;
                case 3:
                    if (health + coins >= maxScore)
                    {
                        transform.Find("Star3").GetComponent<Image>().sprite = star3;
                        SaveLoad.levelUnlocked[SceneManager.GetActiveScene().name] = index;
                        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name, index);
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
	
	public void NextLevel()
    {
		PlayerPrefs.SetInt("coins",coins);
		if(PlayerPrefs.GetInt("stage")==1){
			PlayerPrefs.SetInt("stage", 2);
			Instantiate(trans).GetComponent<SceneTransition>()
            .Initialize("Level_" + PlayerPrefs.GetInt("level") +"_2");
		}else{
			PlayerPrefs.SetInt("stage", 1);
			PlayerPrefs.SetInt("level", PlayerPrefs.GetInt("level") + 1);
			Instantiate(trans).GetComponent<SceneTransition>()
            .Initialize("Level_" + PlayerPrefs.GetInt("level") +"_1");
		}

    }
}
