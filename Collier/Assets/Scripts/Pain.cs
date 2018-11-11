using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pain : MonoBehaviour {

    float timer = 0f;
    public float duration = 0.1f;

    public void Initialize(GameObject g)
    {
        // face the player
        float z = Vector2.SignedAngle(Vector2.right,
            GameObject.FindGameObjectWithTag("Player").transform.position - g.transform.position);
        transform.eulerAngles = new Vector3(0, 0, z);
    }

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if (timer > duration)
        {
            Destroy(gameObject);
        }

    }
}
