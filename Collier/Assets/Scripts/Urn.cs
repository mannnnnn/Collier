using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Urn : MonoBehaviour
{
    public int level;
    public int stage;
    string key;

    Animator anim;

    public string scene;
    public GameObject trans;
    public Sprite spr;

    // Start is called before the first frame update
    void Start()
    {
        key = $"Level_{level}_{stage}";
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetInteger("State", SaveLoad.levelUnlocked[key]);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (SaveLoad.levelUnlocked[key] < 0)
        {
            return;
        }
        Debug.Log("You tried.");
        if (col.gameObject.tag == "Player" && GameObject.FindGameObjectWithTag("SceneTransition") == null)
        {
            Debug.Log("You tried even harder.");
            GameObject go = Instantiate(trans);
            if (go.GetComponent<SceneTransition>() != null)
            {
                go.GetComponent<SceneTransition>().Initialize(scene);
            }
            if (go.GetComponent<StoryTransition>() != null)
            {
                go.GetComponent<StoryTransition>().Initialize(scene, spr);
            }
        }
    }
}
