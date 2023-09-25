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
    public bool isThisHookSelected = false;
    #endregion

    private void Start()
    {
        mainCamera = Camera.main;
        initialPos = transform.position;
    }

    private void Update()
    {
        if (!GameManager.Instance.canHandleTouch)
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
                        isThisHookSelected = true;
                        isDragging = true;
                        UIController.Instance.ActivateSellHookPanel();
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
            UIController.Instance.DeactivateHookPanel();
            if (isThisHookSelected)
            {
                isThisHookSelected = false;
                isDragging = false;
                foreach (HookBase hook in GameManager.Instance.activeHooks)
                {
                    if (hook.hookLevel == this.hookLevel)
                    {
                        hook.thisHookContainer.ResetColor();
                    }
                }
                CheckForHookMerging();
            }
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
        //hookController.SetInitialPosition(hookController.transform.position);
        hookController.canThrow = true;
        hookController.isReached = false;
        hookController.canPull = false;
    }

    #region Hook Merging 
    private void CheckForHookMerging()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.4f);

        foreach (var collider in colliders)
        {
            HookBase otherHook = collider.GetComponent<HookBase>();
            HookContainer hittingHookContainer = collider.GetComponent<HookContainer>();


            if (otherHook != null && otherHook != this && otherHook.hookLevel == this.hookLevel)//if the two same level hooks
            {
                Debug.Log("Merged");
                MergeHooks(otherHook);
                break;
            }
            else if (otherHook != null && otherHook != this && otherHook.hookLevel != this.hookLevel)//if we swap the positions of two hooks
            {
                Debug.Log("we swap the positions of two hooks");
                //making the references for the hooks
                Vector3 otherHooksPos = otherHook.thisHookContainer.transform.position + new Vector3(0, 0.8f, 0);
                Vector3 thisHooksPos = thisHookContainer.transform.position + new Vector3(0, 0.8f, 0);
                HookContainer hookContainerForThisHook = otherHook.thisHookContainer;
                HookContainer hookContainerForOtherHook = thisHookContainer;

                //other hook settings
                otherHook.transform.position = thisHooksPos;
                otherHook.thisHookContainer = hookContainerForOtherHook;
                otherHook.transform.SetParent(otherHook.thisHookContainer.transform);
                otherHook.SetCurrentPosition(otherHook.transform.position);
                otherHook.hookController.hookLevel = otherHook.hookLevel;
                otherHook.thisHookContainer.levelText.text = "" + (int)otherHook.hookLevel;
                otherHook.initialPos = otherHook.transform.position;
                //otherHook.hookController.SetInitialPosition(otherHook.hookController.transform.position);
                //this hook settings
                transform.position = otherHooksPos;
                thisHookContainer = hookContainerForThisHook;
                transform.SetParent(hookContainerForThisHook.transform);
                hookController.hookLevel = this.hookLevel;
                thisHookContainer.levelText.text = "" + (int)hookLevel;
                //initialPos = transform.position;
                SetCurrentPosition(transform.position);
                //hookController.SetInitialPosition(hookController.transform.position);
            }
            else if (hittingHookContainer != null && !hittingHookContainer.isOccupied)// if we change the position of the hook
            {
                Debug.Log("we change the position of the hook");
                thisHookContainer.ActivateDeactivateLevelText(false);
                thisHookContainer.isOccupied = false;
                transform.position = hittingHookContainer.transform.position + new Vector3(0, 0.8f, 0);
                transform.SetParent(hittingHookContainer.transform);
                thisHookContainer = hittingHookContainer;
                hookController.hookLevel = hookLevel;
                initialPos = transform.position;
                thisHookContainer.levelText.text = "" + (int)hookLevel;
                hittingHookContainer.isOccupied = true;
                thisHookContainer.ActivateDeactivateLevelText(true);
                //hookController.SetInitialPosition(hookController.transform.position);
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
        thisHookContainer.ActivateDeactivateLevelText(false);

        currentEnumIndex = (int)hookLevel;
        currentEnumIndex = (currentEnumIndex + 1) % System.Enum.GetValues(typeof(HookLevel)).Length;
        HookLevel hook;
        hook = (HookLevel)currentEnumIndex;
        if (((int)hook) <= GameManager.Instance.hooks.Length)
        {
            Vector3 pos = otherHook.thisHookContainer.transform.position + new Vector3(0, 0.8f, 0);
            GameManager.Instance.AddMergedHook(hook, pos, otherHook.thisHookContainer);
        }
        thisHookContainer.isOccupied = false;

        if (GameManager.Instance.activeHooks.Contains(this))
        {
            GameManager.Instance.activeHooks.Remove(this);
        }
        if (GameManager.Instance.activeHooks.Contains(otherHook))
        {
            GameManager.Instance.activeHooks.Remove(otherHook);
        }


        if (FollowCamera.Instance.hooks.Contains(this.transform))
        {
            FollowCamera.Instance.hooks.Remove(this.transform);
        }
        if (FollowCamera.Instance.hooks.Contains(otherHook.transform))
        {
            FollowCamera.Instance.hooks.Remove(otherHook.transform);
        }

        if (GameManager.Instance.hookControllers.Contains(this.hookController))
        {
            GameManager.Instance.hookControllers.Remove(this.hookController);
        }
        if (GameManager.Instance.hookControllers.Contains(otherHook.hookController))
        {
            GameManager.Instance.hookControllers.Remove(otherHook.hookController);
        }

        Destroy(otherHook.gameObject);
        Destroy(gameObject);
    }
    #endregion


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("ShredderArea"))
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (GameManager.Instance.activeHooks.Contains(this))
                {
                    GameManager.Instance.activeHooks.Remove(this);
                }
                if (FollowCamera.Instance.hooks.Contains(this.transform))
                {
                    FollowCamera.Instance.hooks.Remove(this.transform);
                }
                if (GameManager.Instance.hookControllers.Contains(this.hookController))
                {
                    GameManager.Instance.hookControllers.Remove(this.hookController);
                }
                thisHookContainer.levelText.text = "";
                thisHookContainer.isOccupied = false;
                thisHookContainer.ActivateDeactivateLevelText(false);
                foreach (HookBase hook in GameManager.Instance.activeHooks)
                {
                    if (hook.hookLevel == this.hookLevel)
                    {
                        hook.thisHookContainer.ResetColor();
                    }
                }
                Vector3 pos = mainCamera.WorldToScreenPoint(transform.position);
                GameObject cash = Instantiate(UIController.Instance.cashPrefab, pos, UIController.Instance.cashPrefab.transform.rotation, UIController.Instance.targetForCash.transform);

                cash.GetComponent<CashAnimation>().MoveCoin(50);
                //GameManager.Instance.AddCash(50);
                Destroy(gameObject);
            }
        }
    }
}
