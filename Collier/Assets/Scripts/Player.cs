using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public enum State
    {
        FALL = 0, WALL = 1, CUT = 2, STAND = 3, HURT = 4
    }
    State state = State.STAND;

    public float maxCutLength = 8;
    public float minCutLength = 3;

    BoxCollider2D box;

    public GameObject cut;
    CutAnimation currentCut;
    Vector2 cutTarget;

    Rigidbody2D rb;

    public GameObject leftWall;
    public GameObject rightWall;

    public float driftSpeed = 3;

    Animator anim;
    SpriteRenderer sr;

    float damageTimer = 0f;
    float damageDuration = 1f;

    public GameObject pain;

	// Use this for initialization
	void Start () {
        box = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update() {
        Vector2 pos = transform.position;
        anim.SetInteger("State", (int)state);
        if (GetSide() < 0)
        {
            sr.flipX = false;
        }
        else
        {
            sr.flipX = true;
        }
        RaycastHit2D? hit = Raycast((Vector2)transform.position
            + Vector2.down * box.bounds.extents.y * 0.5f, "Hazard");
        if (hit != null)
        {
            Damage(hit.Value);
        }
        // if not in contact with obstacle, decrement invincibilty timer
        else
        {
            damageTimer = Mathf.MoveTowards(damageTimer, 0, Time.deltaTime);
        }
        switch (state)
        {
            case State.FALL:
                // go towards a wall
                if (GetSide() < 0)
                {
                    rb.position = new Vector2(rb.position.x - (driftSpeed * Time.deltaTime), rb.position.y);
                }
                else
                {
                    rb.position = new Vector2(rb.position.x + (driftSpeed * Time.deltaTime), rb.position.y);
                }
                // until we touch a wall or hit the ground
                if (Grounded())
                {
                    state = State.STAND;
                }
                if (Walled())
                {
                    state = State.WALL;
                }
                break;
            case State.WALL:
                // animation down wall
                // check for cuts
                if (TouchInput.GetSwipe() != null)
                {
                    TryCut(TouchInput.GetSwipe().direction);
                }
                if (!Walled())
                {
                    state = State.FALL;
                }
                if (Grounded())
                {
                    state = State.STAND;
                }
                break;
            case State.CUT:
                // play animation, then fall
                if (currentCut == null)
                {
                    rb.velocity = Vector2.zero;
                    transform.position = new Vector3(cutTarget.x, cutTarget.y, transform.position.z);
                    state = State.FALL;
                }
                // if currently in cut animation, check for obstacles
                else
                {
                    RaycastHit2D? cutHit = Raycast(currentCut.position, "Hazard");
                    if (cutHit != null)
                    {
                        transform.position = new Vector3(currentCut.position.x,
                            currentCut.position.y, transform.position.z);
                        Damage(cutHit.Value);
                        transform.position = new Vector3(currentCut.position.x,
                            currentCut.position.y, transform.position.z);
                    }
                }
                break;
            case State.STAND:
                // can only swipe one way
                if (TouchInput.GetSwipe() != null)
                {
                    TryCut(TouchInput.GetSwipe().direction);
                }
                if (!Grounded())
                {
                    state = State.FALL;
                }
                break;
            case State.HURT:
                if (damageTimer == 0)
                {
                    state = State.FALL;
                }
                if (Grounded())
                {
                    state = State.STAND;
                }
                break;
        }
	}

    void Damage(RaycastHit2D hit)
    {
        if (damageTimer == 0)
        {
            damageTimer = damageDuration;
            // set player to move away from obstacle
            rb.velocity = new Vector2(5f * -GetSide(),
                5f * Mathf.Max(Mathf.Sign(transform.position.y * hit.collider.transform.position.y), 0));
            state = State.HURT;
            // sparks to indicate pain
            Instantiate(pain, transform.position + Vector3.back, transform.rotation)
                .GetComponent<Pain>().Initialize(hit.collider.gameObject);
        }
    }

    // -1 for left, 1 for right
    int GetSide()
    {
        float left = leftWall.GetComponent<BoxCollider2D>().bounds.max.x;
        float right = rightWall.GetComponent<BoxCollider2D>().bounds.min.x;
        float leftDist = Mathf.Abs(transform.position.x - left);
        float rightDist = Mathf.Abs(transform.position.x - right);
        if (leftDist < rightDist)
        {
            return -1;
        }
        return 1;
    }

    bool TryCut(Vector2 direction)
    {
        // we check a bit beyond where we want to land
        float dist = maxCutLength + box.bounds.extents.x;
        RaycastHit2D? raycast = Raycast((Vector2)transform.position +
            direction * dist);
        // if no collisions, go to target position
        if (raycast == null)
        {
            Vector2 end = (Vector2)transform.position +
                direction * maxCutLength;
            Cut(transform.position, end);
            return true;
        }
        // otherwise if far enough away, go to it
        else if ((raycast.Value.point - (Vector2)transform.position).magnitude > minCutLength)
        {
            float freeDist = Mathf.MoveTowards((raycast.Value.point - (Vector2)transform.position).magnitude,
                0, box.bounds.extents.x);
            Vector2 end = (Vector2)transform.position + direction * freeDist;
            Cut(transform.position, end);
            return true;
        }
        return false;
    }

    // start the cut animation
    void Cut(Vector2 start, Vector2 end)
    {
        state = State.CUT;
        rb.velocity = Vector2.zero;
        cutTarget = end;
        currentCut = Instantiate(cut).GetComponent<CutAnimation>();
        currentCut.Initialize(start, end);
    }

    // find closest physics collision with player when attempting to go to target
    public RaycastHit2D? Raycast(Vector2 target, string layer = "Wall")
    {
        Vector2 pos = transform.position;
        RaycastHit2D[] hits = Physics2D.RaycastAll(pos, (target - pos).normalized,
            (target - pos).magnitude, LayerMask.GetMask(layer));
        // return closest hit
        float minDist = (target - pos).magnitude;
        RaycastHit2D? min = null;
        foreach (RaycastHit2D hit in hits)
        {
            if ((hit.point - pos).magnitude < minDist)
            {
                minDist = (hit.point - pos).magnitude;
                min = hit;
            }
        }
        return min;
    }

    bool Grounded()
    {
        return Raycast((Vector2)transform.position
            + Vector2.down * box.bounds.extents.y * 1.8f) != null;
    }
    bool Walled()
    {
        return Raycast((Vector2)transform.position + Vector2.left * box.bounds.extents.x * 1.3f) != null ||
            Raycast((Vector2)transform.position + Vector2.right * box.bounds.extents.x * 1.3f) != null;
    }
}