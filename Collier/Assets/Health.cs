using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {

    public int health = 4;
    public bool poisoned = false;
    Heart[] hearts;
    public float timer = 0f;
    void Start()
    {
        hearts = GetComponentsInChildren<Heart>();
    }

    void Update()
    {
        if (poisoned){
          timer += 0.01f;
          Debug.Log("here " + timer);
          if (timer > 2f){
            health--;
            timer = 0f;
          }
        }
        else{
          timer = 0f;
        }
        for (int i = 0; i < hearts.Length; i++)
        {
            if(poisoned){
                if (i == health-1){
                  Debug.Log(" i and health here " + health);
                  hearts[i].poisoned = true;
                }
            }
            hearts[i].full = i < health;
        }
    }
}
