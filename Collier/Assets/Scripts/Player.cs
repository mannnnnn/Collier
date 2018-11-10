using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public enum State
    {
        FALL, WALL, CUT, STAND, HURT
    }
    State state = State.STAND;

    public float maxCutLength = 8;
    public float minCutLength = 3;

    BoxCollider2D box;

    public GameObject cut;
    CutAnimation currentCut;
    Vector2 cutTarget;

    Rigidbody2D rb;

	// Use this for initialization
	void Start () {
        box = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
        Vector2 pos = transform.position;
		switch (state)
        {
            case State.FALL:
                // go towards a wall
                // until we touch a wall

                break;
            case State.WALL:
                // animation down wall
                // check for cuts
                if (TouchInput.GetSwipe() != null)
                {
                    TryCut(TouchInput.GetSwipe().direction);
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
                break;
            case State.STAND:
                // can only swipe one way
                if (TouchInput.GetSwipe() != null)
                {
                    if (TouchInput.GetSwipe().direction.x > 0)
                    {
                        TryCut(TouchInput.GetSwipe().direction);
                    }
                }
                break;
            case State.HURT:
                break;
        }
	}

    bool TryCut(Vector2 direction)
    {
        // we check a bit beyond where we want to land
        float dist = maxCutLength + box.bounds.extents.x;
        Vector2? raycast = Raycast((Vector2)transform.position +
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
        else if ((raycast - transform.position).Value.magnitude > minCutLength)
        {
            float freeDist = Mathf.MoveTowards((raycast.Value - (Vector2)transform.position).magnitude,
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
    public Vector2? Raycast(Vector2 target)
    {
        Vector2 pos = transform.position;
        RaycastHit2D[] hits = Physics2D.RaycastAll(pos, (target - pos).normalized,
            (target - pos).magnitude, LayerMask.GetMask("Wall"));
        if (hits.Length == 0)
        {
            return null;
        }
        // return closest hit
        float minDist = (target - pos).magnitude;
        Vector2 min = target;
        foreach (RaycastHit2D hit in hits)
        {
            if ((hit.point - pos).magnitude < minDist)
            {
                minDist = (hit.point - pos).magnitude;
                min = hit.point;
            }
        }
        return min;
    }
}
