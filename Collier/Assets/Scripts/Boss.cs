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
    public float spd;

    public GameObject cga;

    GameObject player;

    float startPos;

    GameObject wall;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        wall = transform.Find("Wall").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Begin:
                // slowly move downwards
                timer += Time.deltaTime;
                if (timer > 3f)
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
                if (timer > 5f)
                {
                    state = State.MoveUp;
                    gameObject.tag = "Boss";
                    gameObject.layer = LayerMask.NameToLayer("Enemy");
                }
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
                break;
            case State.KillMe:
                // sits at top of map
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

    public void Damage()
    {

    }
}
