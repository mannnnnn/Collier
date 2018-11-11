using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {

    public int health = 4;
    Heart[] hearts;

    void Start()
    {
        hearts = GetComponentsInChildren<Heart>();
    }

    void Update()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].full = i < health;
        }
    }
}
