using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
{
    enum State
    {
        Begin, MoveDown, Stop, MoveUp, KillMe, Die
    }
    State state = State.Begin;

    float timer = 0f;
    public float spd;

    public GameObject cga;

    GameObject player;

    float startPos;

    GameObject wall;

    float inv = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        wall = transform.Find("Wall").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        inv = Mathf.MoveTowards(inv, 0, 1f * Time.deltaTime);
        if (sword && state != State.KillMe)
        {
            state = State.MoveUp;
            gameObject.tag = "Boss";
            gameObject.layer = LayerMask.NameToLayer("Enemy");
        }
        switch (state)
        {
            case State.Begin:
                // slowly move downwards
                timer += Time.deltaTime;
                if (timer > 6f)
                {
                    state = State.MoveDown;
                    timer = 0f;
                    startPos = transform.position.y;
                }
                transform.position = new Vector3(transform.position.x,
                    transform.position.y + -spd * 0f * Time.deltaTime, transform.position.z);
                break;
            case State.MoveDown:
                // quickly move downwards
                if (!player.GetComponent<Player>().damaged)
                {
                    transform.position = new Vector3(transform.position.x,
                        transform.position.y + -spd * Time.deltaTime, transform.position.z);
                }
                timer += Time.deltaTime;
                if (timer > 1.5f)
                {
                    Instantiate(cga);
                    timer = 0f;
                }
                if (transform.position.y < 3.21f)
                {
                    state = State.Stop;
                    timer = 0f;
                }
                break;
            case State.Stop:
                // pause since the player has the artifact
                timer += Time.deltaTime;
                break;
            case State.MoveUp:
                // speed-walk away
                transform.position = new Vector3(transform.position.x,
                    transform.position.y + spd * 0.75f * Time.deltaTime, transform.position.z);
                if (transform.position.y > startPos)
                {
                    state = State.KillMe;
                }
                gameObject.tag = "Boss";
                gameObject.layer = LayerMask.NameToLayer("Enemy");
                break;
            case State.KillMe:
                // sits at top of map
                if (health <= 0)
                {
                    player.GetComponent<Player>().win = true;
                 GameObject ui = GameObject.FindGameObjectWithTag("UI");
                    ui.GetComponentInChildren<Victory>(true).gameObject.SetActive(true);

                    bool unlockNext = false;
                    for (int i = 1; i <= SaveLoad.LEVELS; i++)
                    {
                        for (int j = 1; j <= SaveLoad.STAGES; j++)
                        {
                            string key = $"Level_{i}_{j}";
                            if (unlockNext)
                            {
                                SaveLoad.levelUnlocked[key] = Math.Max(0, SaveLoad.levelUnlocked[key]);
                                PlayerPrefs.SetInt(key, Math.Max(0, SaveLoad.levelUnlocked[key]));
                                unlockNext = false;
                            }
                            if (key == SceneManager.GetActiveScene().name)
                            {
                                unlockNext = true;
                            }
                        }
                    }
                Destroy(gameObject);
                gameObject.tag = "Boss";
                gameObject.layer = LayerMask.NameToLayer("Enemy");
                break;
                    state = State.Die;
                }
                Color c = GetComponent<SpriteRenderer>().color;
                GetComponent<SpriteRenderer>().color = new Color(c.r, c.g, c.b, health / 3f);
                gameObject.tag = "Boss";
                gameObject.layer = LayerMask.NameToLayer("Enemy");
                break;
            case State.Die:
                /** death animation, freeze player
                player.GetComponent<Player>().win = true;
                 GameObject ui = GameObject.FindGameObjectWithTag("UI");
                    ui.GetComponentInChildren<Victory>(true).gameObject.SetActive(true);

                    bool unlockNext = false;
                    for (int i = 1; i <= SaveLoad.LEVELS; i++)
                    {
                        for (int j = 1; j <= SaveLoad.STAGES; j++)
                        {
                            string key = $"Level_{i}_{j}";
                            if (unlockNext)
                            {
                                SaveLoad.levelUnlocked[key] = Math.Max(0, SaveLoad.levelUnlocked[key]);
                                PlayerPrefs.SetInt(key, Math.Max(0, SaveLoad.levelUnlocked[key]));
                                unlockNext = false;
                            }
                            if (key == SceneManager.GetActiveScene().name)
                            {
                                unlockNext = true;
                            }
                        }
                    }
                Destroy(gameObject);
                gameObject.tag = "Boss";
                gameObject.layer = LayerMask.NameToLayer("Enemy");**/
                break;
        }
    }

    public int health = 3;
    public void Damage()
    {
        if (inv <= 0)
        {
            health--;
            inv = 0.1f;
        }
    }

    bool sword = false;
    public void GotArtifactBadGameThanks()
    {
        Debug.Log("OIAHSDJAOIHDAOHDIHWOPIADHAOPIDWHUOPIADHUAPIDHBUAIHOIWPUAHD");
        sword = true;
    }
}
