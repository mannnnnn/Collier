using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainAttack : MonoBehaviour
{
    // displays warning line, then creates attack

    // warning line duration
    public float warningDuration = 1f;
    float timer = 0f;

    // two points of the attack
    public Vector2 pointA;
    public Vector2 pointB;

    // line renderer reference
    LineRenderer line;
    AnimationCurve lineCurve;

    // sprite renderer reference
    SpriteRenderer sprite;

    public GameObject chain;

    void Start()
    {
    }

    // must be called after creation
    public void Initialize(Vector2 pointA, Vector2 pointB)
    {
        this.pointA = pointA;
        this.pointB = pointB;

        // set up line renderer
        line = gameObject.GetComponent<LineRenderer>();
        line.numCapVertices = 5;
        line.numCornerVertices = 5;

        // set initial line to be the warning line
        lineCurve = new AnimationCurve();
        lineCurve.AddKey(0f, 1f);
        lineCurve.AddKey(1f, 1f);
        // give initial line settings
        line.positionCount = 2;
        line.SetPositions(new Vector3[2] { pointA, pointB });
        line.widthCurve = lineCurve;
        line.widthMultiplier = 0.05f;
        line.startColor = new Color(1f, 1f, 1f, 0.5f);
        line.endColor = new Color(1f, 1f, 1f, 0.5f);
        // set up sprite renderer for the flashing exclamation point by moving it to the midpoint
        sprite = gameObject.GetComponent<SpriteRenderer>();
        transform.position = (pointA + pointB) * 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        // increment timer until it reaches warningDuration
        timer += Time.deltaTime;
        // flash the color of the warning symbol
        sprite.color = Color.Lerp(Color.red, Color.yellow, Random.Range(0f, 1f));
        // if timer exceeds warning time, start chain attack
        if (timer >= warningDuration)
        {
            timer = 0f;
            // deactivate the warning sprite
            sprite.enabled = false;
            // spawn chain with pos, deltaX, deltaY, duration
            GameObject go = Instantiate(chain);
            go.GetComponent<Chain>().Initialize(pointA, pointB);
            Destroy(gameObject);
        }
    }
}
