using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Actual Hook Controller attached on the hook gameobject 
/// will move forward on button click and break the cars and collect them
/// </summary>
public class HookController : MonoBehaviour
{
    #region Public fields
    public HookLevel hookLevel;
    public bool canMove;
    public float speed = 5.0f;
    public bool isreversing = false;
    public float timer;
    public float duration;
    public bool canPull;
    public bool pullDone;
    public Vector3 initialPosition;
    public int damage;
    #endregion

    #region Private Components
    private LineRenderer lineRenderer;
    private Rigidbody rb;
    #endregion

    [SerializeField] private Transform hookBase;
    int count = 0;

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
            timer += Time.deltaTime;
            if (timer >= duration)
            {
                timer = 0.0f;
                isreversing = false;
            }
        }

        if (canPull)
        {
            rb.velocity = -transform.forward * speed * Time.fixedDeltaTime;
            if (transform.position == hookBase.position)
            {
                canPull = false;
            }
        }
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
