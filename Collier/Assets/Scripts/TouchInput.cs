using System;
using UnityEngine;

public class TouchInput : MonoBehaviour
{
    static Swipe swipe;
    Vector2 start;
    Vector2 end;
    bool down = true;
    float timer = 0f;
    public float maxDuration = 1f;

    void Update()
    {
        swipe = null;
        if (Input.GetMouseButtonDown(0))
        {
            down = true;
            start = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButtonUp(0))
        {
            down = false;
            timer = 0f;
            end = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (timer < maxDuration)
            {
                swipe = new Swipe(start, end);
            }
        }
        // keep track of swipe duration
        if (down)
        {
            timer += Time.deltaTime;
        }
        if (swipe != null)
        {
            Debug.Log("it is done.");
        }
    }

    public static Swipe GetSwipe()
    {
        return swipe;
    }
}

public class Swipe
{
    public Vector2 start;
    public Vector2 end;
    public Vector2 direction => (end - start).normalized;

    public Swipe(Vector2 start, Vector2 end)
    {
        this.start = start;
        this.end = end;
    }

    public Swipe Normalized(float length)
    {
        return new Swipe(start, start + (direction * length));
    }
}