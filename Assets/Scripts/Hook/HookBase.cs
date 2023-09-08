using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// HookBase class attached on the Base of the hook 
/// Act as a base of the Hook and holds the references of the Hook Container 
/// handles the Merging , swapping of the hooks
/// </summary>
public class HookBase : MonoBehaviour
{
    #region public references
    [System.NonSerialized] public HookContainer thisHookContainer;
    public HookLevel hookLevel;
    #endregion

    #region Serialized fields
    public HookController hookController;
    #endregion

    #region private properties
    private bool isDragging = false;
    private Vector3 offset;
    private Vector3 initialPos;
    private int currentEnumIndex = 0;
    private Camera mainCamera;
    private RaycastHit hitInfo;
    #endregion

    private void Start()
    {
        mainCamera = Camera.main;
        initialPos = transform.position;
    }

    private void Update()
    {
        if (GameManager.Instance.isHooksMoving)
            return;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hitInfo))
        {
            // Check if the GameObject has a collider
            if (hitInfo.collider != null)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (hitInfo.collider.gameObject == gameObject)
                    {
                        isDragging = true;
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
                    }
                }
            }
        }

        // Release the selected object when the left mouse button is released
        if (Input.GetMouseButtonUp(0))
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

        // Move the selected object when the left mouse button is held down
        if (isDragging)
        {
            Vector3 targetPos = GetMouseWorldPos() + offset;
            targetPos.y = 1f;
            transform.position = targetPos;
        }
    }

    //returns the mouse position
    private Vector3 GetMouseWorldPos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.point;
        }
        return Vector3.zero;
    }


    //Sets the current position of Hook in case when we fail to merge or swap the hook will take its original place
    public void SetCurrentPosition(Vector3 pos)
    {
        initialPos = pos;
    }
    public void SetHookControllerLevel(HookLevel _hookLevel)
    {
        hookController.hookLevel = _hookLevel;
    }

    public void ThrowTheHooks()
    {
        hookController.canThrow = true;
    }

    #region Hook Merging 
    private void CheckForHookMerging()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.2f);

        foreach (var collider in colliders)
        {
            HookBase otherHook = collider.GetComponent<HookBase>();
            HookContainer hittingHookContainer = collider.GetComponent<HookContainer>();


            if (otherHook != null && otherHook != this && otherHook.hookLevel == this.hookLevel)//if the two same level hooks
            {
                MergeHooks(otherHook);
                break;
            }
            else if (otherHook != null && otherHook != this && otherHook.hookLevel != this.hookLevel)//if we swap the positions of two hooks
            {
                //making the references for the hooks
                Vector3 otherHooksPos = otherHook.thisHookContainer.transform.position + new Vector3(0, 0.8f, 0);
                Vector3 thisHooksPos = thisHookContainer.transform.position + new Vector3(0, 0.8f, 0);
                HookContainer hookContainerForThisHook = otherHook.thisHookContainer;
                HookContainer hookContainerForOtherHook = thisHookContainer;

                //other hook settings
                otherHook.transform.position = thisHooksPos;
                otherHook.thisHookContainer = hookContainerForOtherHook;
                otherHook.transform.SetParent(hookContainerForOtherHook.transform);
                otherHook.SetCurrentPosition(otherHook.transform.position);
                otherHook.thisHookContainer.levelText.text = "" + (int)otherHook.hookLevel;
                otherHook.hookController.hookLevel = otherHook.hookLevel;

                //this hook settings
                transform.position = otherHooksPos;
                thisHookContainer = hookContainerForThisHook;
                transform.SetParent(hookContainerForThisHook.transform);
                thisHookContainer.levelText.text = "" + (int)hookLevel;
                hookController.hookLevel = hookLevel;
                initialPos = transform.position;
            }
            else if (hittingHookContainer != null && !hittingHookContainer.isOccupied)// if we change the position of the hook
            {
                thisHookContainer.ActivateDeactivateLevelText(false);
                hittingHookContainer.isOccupied = true;
                thisHookContainer.isOccupied = false;
                thisHookContainer = hittingHookContainer;
                transform.position = hittingHookContainer.transform.position + new Vector3(0, 0.8f, 0);
                transform.SetParent(hittingHookContainer.transform);
                initialPos = transform.position;
                thisHookContainer.levelText.text = "" + (int)hookLevel;
                hookController.hookLevel = hookLevel;
                thisHookContainer.ActivateDeactivateLevelText(true);
            }
            else // if the hook is not collide with anything
            {
                transform.position = initialPos;
            }
        }
    }

    //Merge the same level of hooks to create combined bigger hook
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
        if (((int)hook) <= GameManager.Instance.hooks.Length)
        {
            GameManager.Instance.AddMergedHook(hook, otherHook.transform.position, otherHook.thisHookContainer);
        }

        thisHookContainer.ActivateDeactivateLevelText(false);
        Destroy(otherHook.gameObject);
        Destroy(gameObject);
    }
    #endregion
}
