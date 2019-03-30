using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Urn : MonoBehaviour
{
    public int level;
    public int stage;
    string key;
    public int animation;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        key = $"Level_{level}_{stage}";
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animation = SaveLoad.levelUnlocked[key];
        anim.SetInteger("State", animation);
    }
}
