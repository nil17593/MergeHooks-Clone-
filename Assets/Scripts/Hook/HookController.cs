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
    public HookState HookState;
    [SerializeField] private float speed = 5.0f;
    
    [SerializeField] private float timer;
    [SerializeField] private float duration;
    [SerializeField] private Vector3 initialPosition;
    public int damage;
    public int health;
    #endregion

    #region booleans
    public bool canThrow;
    public bool canPull;
    public bool pullDone;
    public bool canMove;
    public bool isreversing = false;
    #endregion

    #region Private Components
    private LineRenderer lineRenderer;
    private Rigidbody rb;
    #endregion

    [SerializeField] private Transform hookBase;
    
    public int count = 0;

    private void Awake()
    {
        health = 100;
        lineRenderer = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        SetDamageBasedOnHookLevel(hookLevel);
        initialPosition = transform.position;
    }


    public void SetDamageBasedOnHookLevel(HookLevel hookLevel)
    {
        switch (hookLevel)
        {
            case HookLevel.ONE:
                damage = 10;
                break;
            case HookLevel.TWO:
                damage = 20;
                break;
            case HookLevel.THREE:
                damage = 30;
                break;
            case HookLevel.FOUR:
                damage = 40;
                break;

            case HookLevel.FIVE:
                damage = 50;
                break;

            case HookLevel.SIX:
                damage = 60;
                break;

            case HookLevel.SEVEN:
                damage = 70;
                break;

            case HookLevel.EIGHT:
                damage = 80;
                break;

            case HookLevel.NINE:
                damage = 90;
                break;

            case HookLevel.TEN:
                damage = 100;
                break;

            case HookLevel.ELEVEN:
                damage = 110;
                break;
            default:
                damage = 5;
                break;
        }
    }
    private void Update()
    {
        if (!canThrow)
            return;
        if (isreversing && canThrow)
        {
            timer += Time.deltaTime;
            if (timer >= duration)
            {
                timer = 0.0f;
                isreversing = false;
            }
        }

        //if (canPull)
        //{
        //    rb.velocity = -transform.forward * speed * Time.fixedDeltaTime;
        //    if (transform.position == hookBase.position)
        //    {
        //        canPull = false;
        //    }
        //}
    }

    private void FixedUpdate()
    {
        if (!canThrow)
            return;
        if (!canPull && canThrow)
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
        if (!canThrow)
            return;
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

    private void OnCollisionEnter(Collision collider)
    {
        CarController carController = collider.gameObject.GetComponent<CarController>();
        if (carController != null)
        {
            if (carController.health > 0)
            {
                carController.TakeDamage(damage);
                health -= 10;
            }
            if (carController.health > 0)
            {
                isreversing = true;
            }
            else
            {
                count += 1;
            }

            if (health <= 0)
            {
                canPull=true;
                GameManager.Instance.hookControllers.Remove(this);
                //foreach (HookController hook in GameManager.Instance.movingHooks)
                //{
                //    if (hook.canPull != true)
                //    {

                //    }
                //}
            }
        }
    }
}
