using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindParent : MonoBehaviour {

    public Vector2 direction;

	// Use this for initialization
	void Start () {
        // scale emission to size
        ParticleSystem ps = GetComponentInChildren<ParticleSystem>();
        var emission = ps.emission;
        emission.rateOverTime = transform.lossyScale.x * transform.lossyScale.y;
        // rotate particle system
        var angle = Vector2.SignedAngle(Vector2.right, direction);
        ps.gameObject.transform.Rotate(Vector3.forward, angle, Space.World);
        // set child to have same direction
        BoxCollider2D collider = GetComponentInChildren<BoxCollider2D>();
        //collider.gameObject.GetComponent<WindCollider>().direction = direction;
    }
}
