using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformScript : MonoBehaviour
{
    float velX;
    float moveSpeed;
    bool move = true;
    [SerializeField] float right;
    [SerializeField] float left;

    void Start()
    {
        moveSpeed = 2f;
    }

    void Update()
    {
        if (transform.position.x > right)
        {
            move = false;
        }
        if (transform.position.x < left)
        {
            move = true;
        }
    }

    void FixedUpdate()
    {
        if (move)
        {
            transform.position = new Vector2(transform.position.x + moveSpeed * Time.deltaTime, transform.position.y);
        }
        else
        {
            transform.position = new Vector2(transform.position.x - moveSpeed * Time.deltaTime, transform.position.y);
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.name.Equals("Player"))
        {
            col.transform.parent = null;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.name.Equals("Player"))
        {
            col.transform.parent = this.transform;
        }
    }
}
