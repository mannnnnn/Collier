using System;
using UnityEngine;

public class CameraScroll : MonoBehaviour
{
    public Vector3 initial;

    float accMult = 0.5f;
    float speedMult = 0.5f;

    Vector2 acceleration;
    Vector2 speed;

    void Start()
    {
        initial = transform.position;
    }

    void Update()
    {
        MoveCameraSmooth();
    }

    private void MoveCameraSmooth()
    {
        Vector2 target = new Vector2(initial.x,
            GameObject.FindGameObjectWithTag("Player").transform.position.y);
        // follow the specified object
        // to smooth the following, use a differential equation
        // where acceleration makes spd approach targetSpd = k1 * distanceToTravel
        // acceleration is k2 * spdDifference
        Vector2 targetPos = target;
        Vector2 currentPos = transform.position;
        float k1 = 0.1f;
        Vector2 targetSpd = k1 * (targetPos - currentPos);
        float k2 = 0.1f;
        Vector2 acc = k2 * (targetSpd - speed);
        // move the y position of the camera to the calculated value
        speed += acc;
        currentPos += speed;
        transform.position = new Vector3(currentPos.x,
            currentPos.y, transform.position.z);
    }
}