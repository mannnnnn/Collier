using System;
using UnityEngine;

public class CameraScroll : MonoBehaviour
{
    Vector3 initial;

    float accMult = 0.2f;
    float speedMult = 0.2f;

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
        Vector2 targetPos = target;
        Vector2 currentPos = transform.position;
        Vector2 targetSpd = speedMult * (targetPos - currentPos);
        Vector2 acc = accMult * (targetSpd - speed);
        // move the position of the camera to the calculated value
        speed += acc;
        currentPos += speed;
        transform.position = new Vector3(currentPos.x,
            currentPos.y, transform.position.z);
    }
}