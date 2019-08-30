using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {

    public int health = 4;
    public bool poisoned = false;
    Heart[] hearts;
    public float timer = 0f;

    private int poisonedHealthStart = -1;
    //track the health when the player was poisoned
    //If the player hits an obstacle or anything that reduces that health, reset it and move the poison down.

    void Start()
    {
        hearts = GetComponentsInChildren<Heart>();
    }

    void Update()
    {

      if(health < poisonedHealthStart){
        hearts[health].poisoned = false; //update state of dead heart 
        hearts[health].full = false;
        hearts[health].poisoned = true; //update state of new poisoned heart
      }

        if (poisoned){
          timer += 0.01f;
          if (timer > 2f){
            health--;
            timer = 0f;
            poisoned = false; //Poison changed to only remove 1 heart max
            hearts[health].poisoned = false; //update state of dead heart 
            hearts[health].full = false;
          }
        }
        else{
          timer = 0f;
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            if(poisoned){
                if (i == health-1){
                  hearts[i].poisoned = true;
                } 
                else{
                  hearts[i].poisoned = false;
                  hearts[i].full = i < health;
                }
            } else {
            hearts[i].full = i < health;
            hearts[i].poisoned = false;
            }
        }
    }
}
