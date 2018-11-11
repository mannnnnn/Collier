using System;
using UnityEngine;

public class CameraScroll : MonoBehaviour
{
    Vector3 initial;

    float accMult = 0.2f;
    float speedMult = 0.2f;

    Vector2 acceleration;
    Vector2 speed;

    float levelTop = float.MinValue;
    float levelBottom = float.MaxValue;
    float cameraSize;

    void Start()
    {
        cameraSize = Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y
            - Camera.main.ScreenToWorldPoint(Vector2.zero).y;
        initial = transform.position;
        // find top
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Wall"))
        {
            float value = go.GetComponent<SpriteRenderer>().bounds.max.y;
            if (value > levelTop)
            {
                levelTop = value;
            }
        }
        // find bottom
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Goal"))
        {
            float value = go.GetComponent<SpriteRenderer>().bounds.min.y;
            if (value < levelBottom)
            {
                levelBottom = value;
            }
        }
    }

    void Update()
    {
        Health health = GameObject.FindGameObjectWithTag("Health")?.GetComponent<Health>();
        if (health == null || health.health > 0)
        {
            MoveCameraSmooth();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
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
        float clampedY = Mathf.Clamp(currentPos.y, levelBottom + cameraSize * 0.5f, levelTop - cameraSize * 0.5f);
        transform.position = new Vector3(currentPos.x,
            clampedY, transform.position.z);
    }

    public float GetParallax(SpriteRenderer bg, float offset, float shrink)
    {
        float cameraPos = Mathf.InverseLerp(levelTop - cameraSize * 0.5f, levelBottom + cameraSize * 0.5f, Camera.main.transform.position.y);
        float x = Mathf.Lerp(-offset + shrink, (bg.bounds.extents.y * transform.localScale.y) * 2f - cameraSize - shrink, cameraPos);
        return Camera.main.transform.position.y + x;
    }
}