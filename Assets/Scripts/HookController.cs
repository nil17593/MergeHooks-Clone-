using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookController : MonoBehaviour
{
    private LineRenderer lineRenderer;
    [SerializeField] private Transform hookBase;
    public bool canMove;
    public float speed = 5.0f;
    public bool isreversing = false;
    public float timer;
    public float duration;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
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
        if (!isreversing)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(-Vector3.forward * speed * 1f * Time.deltaTime);
        }
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
            isreversing = true;
        }
    }
}
