using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainGroupAttack : MonoBehaviour
{
    public GameObject chainAttack;
    int attackCount = 5;
    float delay = 0.1f;
    float index = 0;
    float ypos;
    float deviation = 2f;
    int attacks = 0;

    void Start()
    {
        // get player position
        ypos = GameObject.FindGameObjectWithTag("Player").transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (attacks < attackCount && index > delay)
        {
            int side = (2 * Random.Range(0, 2)) - 1;
            float yStart = Random.Range(ypos - deviation, ypos + deviation);
            float yEnd = Random.Range(ypos - deviation, ypos + deviation);
            float xStart = GameObject.FindGameObjectWithTag("Goal").transform.position.x + (6f * side);
            float xEnd = GameObject.FindGameObjectWithTag("Goal").transform.position.x + (6f * -side);
            GameObject go = Instantiate(chainAttack);
            go.GetComponent<ChainAttack>().Initialize(new Vector2(xStart, yStart), new Vector2(xEnd, yEnd));
            index = 0;
            attacks++;
        }
        if (attacks > attackCount)
        {
            Destroy(gameObject);
        }
        index += Time.deltaTime;
        
    }
}
