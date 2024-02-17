using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;

    [SerializeField] private float initVelocity;
    [SerializeField] private float maxVelocity;
    [SerializeField] private float acceleration;
    private float xVelocity;
    private float yVelocity;

    [SerializeField] private float sprintRate;
    [SerializeField] private float sprintStaminaDepleteRate;
    [HideInInspector] public bool isSprinting;

    private bool isCobweb;
    [SerializeField] private float cobwebSlowRate;

    private StaminaManager playerStaminaManager;

    private Rigidbody2D rb;

    [SerializeField] private Animator _animator;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerStaminaManager = GetComponent<StaminaManager>();

        xVelocity = 0f;
        yVelocity = 0f;

        isCobweb = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.canPlayerControl == false)
        {
            rb.velocity = Vector2.zero;
            _animator.SetBool("Walk", false);

			return; 
        }

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

        if (Input.GetKey(KeyCode.LeftShift) && playerStaminaManager.stamina>0.5f)
        {
            isSprinting = true;
            playerStaminaManager.changeStamina(-1f*sprintStaminaDepleteRate*Time.deltaTime);
        }
        else
        {
            isSprinting = false;
        }

        //Debug.Log(rb.velocity);
        rb.velocity = new Vector2(xVelocity, yVelocity) * (isSprinting ? sprintRate:1f) * (isCobweb?cobwebSlowRate:1f);

        _animator.SetBool("Walk", rb.velocity != Vector2.zero);

	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Cobweb"))
        {
            isCobweb = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Cobweb"))
        {
            isCobweb = false;
        }
    }
}
