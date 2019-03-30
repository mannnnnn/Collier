using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    enum State
    {
        Begin, MoveDown, Stop, MoveUp, KillMe, Die
    }
    State state = State.Begin;

    float timer = 0f;
    public float spd = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Begin:
                // slowly move downwards
                if (timer < 4f)
                {
                    timer += Time.deltaTime;
                }
                transform.position = new Vector3(transform.position.x,
                    transform.position.y + -spd * 0.5f * Time.deltaTime, transform.position.z);
                break;
            case State.MoveDown:
                // quickly move downwards
                transform.position = new Vector3(transform.position.x,
                    transform.position.y + -spd * Time.deltaTime, transform.position.z);
                break;
            case State.Stop:
                // pause since the player has the artifact
                break;
            case State.MoveUp:
                // speed-walk away
                transform.position = new Vector3(transform.position.x,
                    transform.position.y + spd * 0.75f * Time.deltaTime, transform.position.z);
                break;
            case State.KillMe:
                // sits at top of map
                transform.position = new Vector3(transform.position.x,
                    transform.position.y + spd * 0.75f * Time.deltaTime, transform.position.z);
                break;
            case State.Die:
                // death animation, freeze player
                if (timer > 3f)
                {
                    // trigger victory
                }
                break;
        }
    }
}
