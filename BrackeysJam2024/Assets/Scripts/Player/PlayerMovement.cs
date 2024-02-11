using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float initVelocity;
    [SerializeField] private float maxVelocity;
    [SerializeField] private float acceleration;
    private float xVelocity;
    private float yVelocity;

    [SerializeField] private float sprintRate;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        xVelocity = 0f;
        yVelocity = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        //Horizontal input
        if (Input.GetKey(KeyCode.A))
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                xVelocity = initVelocity * -1f;
            }
            xVelocity -=Mathf.Log(Mathf.Abs(xVelocity))*acceleration*Time.deltaTime;
            if (xVelocity <= maxVelocity * -1f) 
            {
                xVelocity = maxVelocity*-1f;
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                xVelocity = initVelocity * 1f;
            }
            xVelocity += Mathf.Log(Mathf.Abs(xVelocity)) * acceleration * Time.deltaTime;
            if (xVelocity >= maxVelocity * 1f)
            {
                xVelocity = maxVelocity;
            }
        }
        else
        {
            xVelocity = 0f;
        }

        //Vertical Input
        if (Input.GetKey(KeyCode.W))
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                yVelocity = initVelocity * 1f;
            }
            yVelocity += Mathf.Log(Mathf.Abs(yVelocity)) * acceleration * Time.deltaTime;
            if (yVelocity >= maxVelocity * 1f)
            {
                yVelocity = maxVelocity;
            }
        }
        else if (Input.GetKey(KeyCode.S))
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                yVelocity = initVelocity * -1f;
            }
            yVelocity -= Mathf.Log(Mathf.Abs(yVelocity)) * acceleration * Time.deltaTime;
            if (yVelocity <= maxVelocity * -1f)
            {
                yVelocity = maxVelocity*-1f;
            }
        }
        else
        {
            yVelocity = 0f;
        }

        //Debug.Log(rb.velocity);
        rb.velocity = new Vector2(xVelocity, yVelocity)*(Input.GetKey(KeyCode.LeftShift)? sprintRate : 1f);
    }
}
