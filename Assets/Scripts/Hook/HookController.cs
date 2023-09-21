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
    public bool isReached;
    public bool pullDone;
    public bool canMove;
    public bool isreversing = false;
    public bool canPull = false;
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
        if (!canThrow || GameManager.Instance.presentGameState == GameManager.GameState.Merging)
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
        if (!canThrow || GameManager.Instance.presentGameState==GameManager.GameState.Merging)
            return;

        if (!isReached && canThrow && health > 0)
        {
            if (!isreversing)
            {
                rb.velocity = speed * Time.fixedDeltaTime * transform.forward;
            }
            else
            {
                rb.AddForce(1.5f * speed * Time.deltaTime * -transform.forward, ForceMode.Force);// = -transform.forward * speed * Time.fixedDeltaTime;
            }
        }
        if (canPull)
        {
            ReturnToBase();
        }
    }

    private void LateUpdate()
    {
        if (!canThrow || GameManager.Instance.presentGameState == GameManager.GameState.Merging)
            return;
        DrawRopeForHook();
    }

    void DrawRopeForHook()
    {
        lineRenderer.SetPosition(1, transform.position);
        lineRenderer.SetPosition(0, hookBase.position);
    }

    //public void OnTrowButtonPressed()
    //{
    //    initialPos = transform.position;
    //    Debug.Log("pos" + initialPos);
    //    canMove = true;
    //}

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

            if (health <= 0)
            {
                isReached=true;
                if (GameManager.Instance.CanStartTopull())
                {
                    GameManager.Instance.presentGameState = GameManager.GameState.Pulling;
                }
                
                //foreach (HookController hook in GameManager.Instance.movingHooks)
                //{
                //    if (hook.canPull != true)
                //    {

                //    }
                //}
            }
        }

        else if (collider.gameObject.CompareTag("Gift"))
        {
            isReached = true;
            GameManager.Instance.isThisLevelCleared = true;
            if (GameManager.Instance.CanStartTopull())
            {
                GameManager.Instance.presentGameState = GameManager.GameState.Pulling;
            }
        }
    }

    //GameManager.Instance.isThisLevelCleared = true;
    //        if (GameManager.Instance.CanStartTopull())
    //        {
    //            GameManager.Instance.presentGameState = GameManager.GameState.Pulling;
    //            int currentLevel = GameManager.Instance.GetCurrentLevel();
    //int nextLevel = currentLevel + 1;
    //GameManager.Instance.SaveCurrentLevel(nextLevel);

    public void ResetGame()
    {
        health = 100;
    }

    //public void SetInitialPosition(Vector3 pos)
    //{
    //    initialPos = pos;
    //}


    public void ReturnToBase()
    {
        Vector3 newPosition = Vector3.MoveTowards(transform.position, hookBase.transform.position + new Vector3(0, 0, 0.7f), 20f * Time.deltaTime);
        transform.position = newPosition;
        ResetGame();
    }
}
