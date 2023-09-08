using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookBase : MonoBehaviour
{
    //public enum HookLevel
    //{
    //    ONE = 10,
    //    TWO = 20,
    //    THREE = 30,
    //    FOUR = 40,
    //    FIVE = 50,
    //    SIX,
    //    SEVEN,
    //    EIGHT,
    //    NINE,
    //    TEN,
    //    ELEVEN,
    //}

    [SerializeField] private HookController hookController;
    [System.NonSerialized]public HookContainer thisHookContainer;
    public HookLevel hookLevel;
    private bool isDragging = false;
    private Vector3 offset;
    private Vector3 initialPos;
    private int currentEnumIndex = 0;

    private void Start()
    {
        initialPos = transform.position;
    }
    private void OnMouseDown()
    {
        foreach (HookBase hook in GameManager.Instance.activeHooks)
        {
            if (hook.hookLevel == this.hookLevel)
            {
                if (hook.thisHookContainer != thisHookContainer)
                {
                    hook.thisHookContainer.ChangeColor();
                }
            }
        }
        offset = transform.position - GetMouseWorldPos();
        isDragging = true;
    }

    private void OnMouseUp()
    {
        isDragging = false;
        foreach (HookBase hook in GameManager.Instance.activeHooks)
        {
            if (hook.hookLevel == this.hookLevel)
            {
                if (hook.thisHookContainer != thisHookContainer)
                {
                    hook.thisHookContainer.ResetColor();
                }
            }
        }
        CheckForHookMerging();
    }

    private void CheckForHookMerging()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.2f);

        foreach (var collider in colliders)
        {
            HookBase otherHook = collider.GetComponent<HookBase>();
            HookContainer hookContainer = collider.GetComponent<HookContainer>();

            if(hookContainer!=null && !hookContainer.isOccupied)
            {
                thisHookContainer.isOccupied = true;
                transform.position = hookContainer.transform.position;
            }

            if (otherHook != null && otherHook != this && otherHook.hookLevel == this.hookLevel)
            {
                MergeHooks(otherHook);
                break;
            }
            else if (otherHook == null)
            {
                transform.position = initialPos;
            }
            else if (otherHook != null && otherHook != this && otherHook.hookLevel != this.hookLevel)
            {
                Vector3 otherHooksPos = otherHook.transform.position;
                Vector3 thisHooksPos = transform.position;
                otherHook.transform.position = thisHooksPos;
                otherHook.thisHookContainer = this.thisHookContainer;
                transform.position = otherHooksPos;
                this.thisHookContainer = otherHook.thisHookContainer;
                otherHook.thisHookContainer.isOccupied = true;
            }
        }
    }

    private void MergeHooks(HookBase otherHook)
    {
        thisHookContainer.isOccupied = false;

        if (GameManager.Instance.activeHooks.Contains(this))
        {
            GameManager.Instance.activeHooks.Remove(this);
        }
        if (GameManager.Instance.activeHooks.Contains(otherHook))
        {
            GameManager.Instance.activeHooks.Remove(otherHook);
        }

        
        currentEnumIndex = (int)hookLevel;
        currentEnumIndex = (currentEnumIndex + 1) % System.Enum.GetValues(typeof(HookLevel)).Length;
        HookLevel hook;
        hook = (HookLevel)currentEnumIndex;
        if (((int)hook) < GameManager.Instance.hooks.Length)
        {
            GameManager.Instance.AddMergedHook(hook, otherHook.transform.position, otherHook.thisHookContainer);
        }
        Destroy(otherHook.gameObject);
        Destroy(gameObject);
    }

    private Vector3 GetMouseWorldPos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.point;
        }
        return Vector3.zero;
    }

    private void Update()
    {
        if (isDragging)
        {
            Vector3 targetPos = GetMouseWorldPos() + offset;
            targetPos.y = 1f;
            transform.position = targetPos;
        }
    }
}
