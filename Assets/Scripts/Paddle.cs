using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEditor;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Rigidbody rb;
    public GameObject manager;
    private Vector3 move;
    private float speed;
    private float x_max;

    public Vector3 Move
    {
        get { return move; }
        set { move = value; }
    }

    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    public float X_max
    {
        get { return x_max; }
        set { x_max = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Move = new Vector3();
        Speed = 15;
        X_max = 5;
    }

    // Update is called once per frame
    void Update()
    {
        Speed = 15;
        Move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        X_max = 7.5f + rb.position.z;

        if (rb.position.z < 0)
        {
            rb.position = new Vector3(rb.position.x, 1, 0);
        }

        if (rb.position.z > 25)
        {
            rb.position = new Vector3(rb.position.x, 1, 25);
        }

        if (rb.position.x < -X_max)
        {
            rb.position = new Vector3(-X_max, 1, rb.position.z);
        }

        if (rb.position.x > X_max)
        {
            rb.position = new Vector3(X_max, 1, rb.position.z);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            Speed = 30;
        }

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    rb.AddForce(Vector3.up * Speed, ForceMode.Impulse);
        //}
    }

    private void FixedUpdate()
    {
        rb.AddForce(Move * Speed);
    }
}
