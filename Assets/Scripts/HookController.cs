using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookController : MonoBehaviour
{
    public HookLevel hookLevel;
    private LineRenderer lineRenderer;
    [SerializeField] private Transform hookBase;
    public bool canMove;
    public float speed = 5.0f;
    public bool isreversing = false;
    public float timer;
    public float duration;
    private Rigidbody rb;
    int count = 0;

    public bool canPull;
    public bool pullDone;

    public Vector3 initialPosition;
    public int damage;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        if (isreversing)
        {
            // Increment the timer while the boolean is true
            timer += Time.deltaTime;

            // Check if the timer has exceeded the specified duration
            if (timer >= duration)
            {
                // Reset the timer and set the boolean to false
                timer = 0.0f;
                isreversing = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            canMove = true;
        }
        else
        {
            canMove = false;
        }

        if (canPull)
        {
            rb.velocity = -transform.forward * speed * Time.fixedDeltaTime;
            if (transform.position == hookBase.position)
            {
                canPull = false;
            }
        }
        //if (!isreversing)
        //{
        //    transform.Translate(Vector3.forward * speed * Time.deltaTime);
        //}
        //else
        //{
        //    transform.Translate(-Vector3.forward * speed * 2f * Time.deltaTime);
        //}
    }

    private void FixedUpdate()
    {
        if (!canPull && pullDone)
        {
            if (!isreversing)
            {
                rb.velocity = transform.forward * speed * Time.fixedDeltaTime;
            }
            else
            {
                rb.velocity = -transform.forward * speed * Time.fixedDeltaTime;
            }
        }
        else
        {

        }
        //if (!isreversing)
        //{
        //    rb.velocity = new Vector3(0, 0, speed * Time.fixedDeltaTime);
        //}
        //else
        //{
        //    rb.velocity = new Vector3(0, 0, -speed * Time.fixedDeltaTime);
        //}
    }


    private void LateUpdate()
    {
        DrawRopeForHook();
    }

    void DrawRopeForHook()
    {
        lineRenderer.SetPosition(1, transform.position);
        lineRenderer.SetPosition(0, hookBase.position);
    }

    public void OnTrowButtonPressed()
    {
        canMove = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            //isreversing = true;
            other.gameObject.transform.SetParent(transform);
            count += 1;
            if (count >= 4)
            {
                canPull = true;
            }
        }
    }
}
