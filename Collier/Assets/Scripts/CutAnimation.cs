using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutAnimation : MonoBehaviour
{
    public enum State
    {
        CUT, RETRACT
    }
    // current state
    State state = State.CUT;
    float timer = 0f;

    // cut speed
    public float cutSpd = 4f;
    public float cutWidth = 0.4f;

    // two points of the cut
    public Vector2 pointA;
    public Vector2 pointB;

    // line renderer reference
    LineRenderer line;
    AnimationCurve lineCurve;
    AnimationCurve cutCurve;

    public Vector2 position;

    void Start()
    {
        if (line == null)
        {
            Initialize(pointA, pointB);
        }
    }

    // must be called after creation
    public void Initialize(Vector2 pointA, Vector2 pointB)
    {
        this.pointA = pointA;
        this.pointB = pointB;
        position = pointA;

        // set up line renderer
        line = gameObject.GetComponent<LineRenderer>();
        line.numCapVertices = 5;
        line.numCornerVertices = 5;

        // curve to be used later for the cut animation
        cutCurve = new AnimationCurve();
        cutCurve.AddKey(0f, 0f);
        cutCurve.AddKey(0.5f, 1f);
        cutCurve.AddKey(1f, 0f);

        // set up line renderer curve
        line.widthMultiplier = cutWidth;
        line.widthCurve = cutCurve;
        line.startColor = new Color(1f, 1f, 1f, 1f);
        line.endColor = new Color(1f, 1f, 1f, 1f);
        line.positionCount = 1;
        line.SetPositions(new Vector3[1] { pointA });

        // move to the midpoint
        transform.position = (pointA + pointB) * 0.5f;

        // state is warning at first
        state = State.CUT;
    }

    // Update is called once per frame
    void Update()
    {
        // draw from pointA to middle point on cut
        // then draw from middle point to pointB on retract
        Vector2 point = Vector2.Lerp(pointA, pointB, timer);
        // set position
        if (state == State.CUT)
        {
            position = point;
        }
        else
        {
            position = pointB;
        }
        // change line renderer points
        Vector3[] positions = new Vector3[7];
        for (int i = 0; i < positions.Length; i++)
        {
            if (state == State.CUT)
            {
                positions[i] = Vector2.Lerp(pointA, point, (float)i / (positions.Length - 1));
            }
            else
            {
                positions[i] = Vector2.Lerp(point, pointB, (float)i / (positions.Length - 1));
            }
        }
        line.positionCount = positions.Length;
        line.SetPositions(positions);
        // increment timer
        timer += cutSpd * Time.deltaTime / (pointB - pointA).magnitude;
        // finish when timer is 1
        if (timer > 1f)
        {
            timer = 0f;
            // if finished cutting, then retract
            if (state == State.CUT)
            {
                state = State.RETRACT;
            }
            // if finished retracting, we're done
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
