using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookBase : MonoBehaviour
{
    public enum HookLevel
    {
        ONE = 10,
        TWO = 20,
        THREE = 30,
        FOUR = 40,
        FIVE = 50,
        SIX,
        SEVEN,
        EIGHT,
        NINE,
        TEN,
        ELEVEN,
    }

    [SerializeField] private HookController hookController;
    [System.NonSerialized]public HookContainer thisHookContainer;
    public HookLevel hookLevel;
    private bool isDragging = false;
    private Vector3 offset;
    private Vector3 initialPos;

    private void Start()
    {
        initialPos = transform.position;
    }
    private void OnMouseDown()
    {
        // Calculate the offset between the mouse position and the object's position
        foreach (HookBase hook in GameManager.Instance.activeHooks)
        {
            if (hook.hookLevel == this.hookLevel)
            {
                if (hook.thisHookContainer != thisHookContainer)
                {
                    hook.thisHookContainer.ChangeMaterialColor();
                }
                //hook.GetComponentInParent<Renderer>().material.color = Color.green;
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
                //hook.GetComponentInParent<Renderer>().material.color = Color.green;
            }
        }
        CheckForHookMerging();
    }

    private void CheckForHookMerging()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1f);

        foreach (var collider in colliders)
        {
            HookBase otherHook = collider.GetComponent<HookBase>();

            if (otherHook != null && otherHook != this && otherHook.hookLevel == this.hookLevel)
            {
                MergeHooks(otherHook);
                break; // You can break the loop after merging the first hook found.
            }
            else
            {
                transform.position = initialPos;
            }
        }
    }

    private void MergeHooks(HookBase otherHook)
    {
        // Implement your logic to merge the hooks here.
        // For example, you can destroy one of the hooks and update the properties of the other.

        // Destroy the other hook (you can customize this logic)
        transform.position = otherHook.transform.position;
        transform.SetParent(otherHook.thisHookContainer.transform);
        thisHookContainer.isOccupied = false;
        otherHook.thisHookContainer.isOccupied = false;
        if (GameManager.Instance.activeHooks.Contains(this))
        {
            GameManager.Instance.activeHooks.Remove(this);
        }
        if (GameManager.Instance.activeHooks.Contains(otherHook))
        {
            GameManager.Instance.activeHooks.Remove(otherHook);
        }
        Destroy(otherHook.gameObject);
        Destroy(gameObject);

        // Update the properties of this hook if needed
        // For example, increase hookLevel or change appearance
        hookLevel += 10;

        // You can update any other properties or behaviors as required for your game.
    }

    //private bool TryToMergeHook(HookBase hook)
    //{

    //}

    private Vector3 GetMouseWorldPos()
    {
        // Get the mouse position in world coordinates
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.point;
        }
        return Vector3.zero; // Return (0,0,0) if the raycast doesn't hit anything
    }

    private void Update()
    {
        if (isDragging)
        {
            // Update the object's position based on the mouse movement
            Vector3 targetPos = GetMouseWorldPos() + offset;
            targetPos.y = 1f;
            transform.position = targetPos;
        }
    }
}
