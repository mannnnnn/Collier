using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Chain : MonoBehaviour
{
    public Vector2 goal;
    float speed = 25f;
    bool damage = false;
    public GameObject square;
    BoxCollider2D col;

    void Start()
    {
        col = GetComponent<BoxCollider2D>();
    }

    public void Initialize(Vector2 a, Vector2 b)
    {
        goal = (a + b) * 0.5f;
        transform.position = new Vector3(a.x, a.y, transform.position.z);
        transform.eulerAngles = new Vector3(0f, 0f, Mathf.Atan2(b.y - a.y, b.x - a.x) * 180f / Mathf.PI);
    }

    void Update()
    {
        Vector2 pos = Vector2.MoveTowards(transform.position, goal, speed * Time.deltaTime);
        transform.position = new Vector3(pos.x, pos.y, transform.position.z);
        damage = ((Vector2)transform.position - goal).magnitude > 0.01f;
        if (damage)
        {
            gameObject.tag = "Hazard";
            gameObject.layer = LayerMask.NameToLayer("Enemy");
        }
        else
        {
            gameObject.tag = "Platform";
            gameObject.layer = LayerMask.NameToLayer("Wall");
        }
        col.isTrigger = damage;
    }
}