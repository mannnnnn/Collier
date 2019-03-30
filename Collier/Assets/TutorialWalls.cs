using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialWalls : MonoBehaviour
{
     public GameObject trans;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollision2DEnter(Collision2D collision)
    {
        Debug.Log("Hey!");
        Instantiate(trans).GetComponent<SceneTransition>()
            .Initialize("1_Town");
    }
}
