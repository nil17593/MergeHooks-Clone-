using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookBase : MonoBehaviour
{
    [SerializeField] private HookController hookController;
    [System.NonSerialized] public HookContainer thisHookContainer;

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

    public void SetCurrentPosition(Vector3 pos)
    {
        initialPos = pos;
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


            if (otherHook != null && otherHook != this && otherHook.hookLevel == this.hookLevel)
            {
                MergeHooks(otherHook);
                break;
            }
            else if (otherHook != null && otherHook != this && otherHook.hookLevel != this.hookLevel)
            {

                Vector3 otherHooksPos = otherHook.thisHookContainer.transform.position + new Vector3(0, 0.8f, 0);
                Vector3 thisHooksPos = thisHookContainer.transform.position + new Vector3(0, 0.8f, 0);
                HookContainer hookContainerForThisHook = otherHook.thisHookContainer;
                HookContainer hookContainerForOtherHook = thisHookContainer;

                otherHook.transform.position = thisHooksPos;
                otherHook.thisHookContainer = hookContainerForOtherHook;
                otherHook.transform.SetParent(hookContainerForOtherHook.transform);
                otherHook.SetCurrentPosition(otherHook.transform.position);

                transform.position = otherHooksPos;
                thisHookContainer = hookContainerForThisHook;
                transform.SetParent(hookContainerForThisHook.transform);

                initialPos = transform.position;
            }
            else if (hookContainer != null && !hookContainer.isOccupied)
            {
                hookContainer.isOccupied = true;
                thisHookContainer.isOccupied = false;
                thisHookContainer = hookContainer;
                transform.position = hookContainer.transform.position + new Vector3(0, 0.8f, 0);
                transform.SetParent(hookContainer.transform);
                initialPos = transform.position;
            }
            else
            {
                Debug.Log("ESEDD");
                transform.position = initialPos;
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
