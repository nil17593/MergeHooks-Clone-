using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Gamemanager class handles spawning of Hooks and holds the list of HookContainers
/// </summary>
public class GameManager : Singleton<GameManager>
{
    #region public lists and arrays
    public List<HookBase> activeHooks = new List<HookBase>();
    public List<HookContainer> hookContainers = new List<HookContainer>();
    public HookBase[] hooks;
    public List<HookController> hookControllers = new List<HookController>();
    public List<CarController> carControllers = new List<CarController>();
    public GameObject buttonPanel;
    #endregion
    public enum GameState { None, Merging, Throwing, Pulling, End };
    public GameState presentGameState;

    #region Public booleans
    [Header("BOOLS")]
    public bool canProgress;
    public bool onlyOnce;
    public bool canHandleTouch;
    public bool isHooksMoving;
    public bool hasCoroutineStarted = false;
    #endregion


    #region UI
    [SerializeField] private TextMeshProUGUI gameCash;
    #endregion


    private void Start()
    {
        gameCash.text = PlayerPrefs.GetInt("Cash").ToString();
        canHandleTouch = true;
        presentGameState = GameState.Merging;
    }

    private void Update()
    {
        if (presentGameState == GameState.Merging)
        {
            canHandleTouch = true;
            buttonPanel.SetActive(true);
        }
        else if (presentGameState == GameState.Throwing)
        {
            canHandleTouch = false;
            buttonPanel.SetActive(false);
        }
        else if (presentGameState == GameState.Pulling)
        {
            if (!hasCoroutineStarted)
            {
                StartCoroutine(PullThecars());
                hasCoroutineStarted = true;
                canHandleTouch = false;
            }
        }
    }

    #region UI Methods

    public void AddCash(int amount)
    {
        int cash = PlayerPrefs.GetInt("Cash");
        cash += amount;
        PlayerPrefs.SetInt("Cash", cash);
        gameCash.text = cash.ToString();
    }

    #endregion


    #region Spawning of Hooks 
    //Spawn hooks on every button click for now there will be always level1 hook will instantiate
    public void SpawnHooks()
    {
        if (hookContainers.Count > 0 && activeHooks.Count < hookContainers.Count)
        {
            HookContainer hookContainer = GetRandomHookContainer();
            HookBase hookBase = Instantiate(hooks[0], hookContainer.transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity, hookContainer.transform);
            hookBase.thisHookContainer = hookContainer;
            hookContainer.isOccupied = true;
            hookContainer.ActivateDeactivateLevelText(true);
            hookContainer.levelText.text = ""+ ((int)hookBase.hookLevel);
            hookBase.SetHookControllerLevel(HookLevel.ONE);
            activeHooks.Add(hookBase);
            hookControllers.Add(hookBase.hookController);
            FollowCamera.Instance.hooks.Add(hookBase.hookController.transform);
        }
    }

    //return random Hook Container to spawn the hooks on it
    public HookContainer GetRandomHookContainer()
    {
        List<HookContainer> containers = new List<HookContainer>();
        containers.AddRange(hookContainers.FindAll(o => !o.isOccupied));

        // Shuffle the list using Fisher-Yates shuffle algorithm
        System.Random rng = new System.Random();
        int n = containers.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            HookContainer value = containers[k];
            containers[k] = containers[n];
            containers[n] = value;
        }

        if (containers.Count > 0)
        {
            return containers[0];
        }
        else
        {
            return null;
        }
    }
    #endregion


    #region Switch case for Instantiate Merged Hooks
    //switch case to instantiate merged Hooks it will take level of hook, position and HookContainer
    public void AddMergedHook(HookLevel hookLevel,Vector3 pos,HookContainer parent)
    {
        switch (hookLevel)
        {
            case HookLevel.TWO:
                HookBase hook2 = Instantiate(hooks[1], pos, Quaternion.identity, parent.transform);
                hook2.thisHookContainer = parent;
                parent.levelText.text = "" + ((int)hook2.hookLevel);
                activeHooks.Add(hook2);
                parent.isOccupied = true;
                hook2.SetHookControllerLevel(HookLevel.TWO);
                hookControllers.Add(hook2.hookController);
                break;

            case HookLevel.THREE:
                HookBase hook3 = Instantiate(hooks[2], pos, Quaternion.identity, parent.transform);
                activeHooks.Add(hook3);
                hook3.thisHookContainer = parent;
                parent.levelText.text = "" + ((int)hook3.hookLevel);
                parent.isOccupied = true;
                hook3.SetHookControllerLevel(HookLevel.THREE);
                hookControllers.Add(hook3.hookController);
                break;

            case HookLevel.FOUR:
                HookBase hook4 = Instantiate(hooks[3], pos, Quaternion.identity, parent.transform);
                activeHooks.Add(hook4);
                hook4.thisHookContainer = parent;
                parent.levelText.text = "" + ((int)hook4.hookLevel);
                parent.isOccupied = true;
                hook4.SetHookControllerLevel(HookLevel.FOUR);
                hookControllers.Add(hook4.hookController);
                break;

            case HookLevel.FIVE:
                HookBase hook5 = Instantiate(hooks[4], pos, Quaternion.identity, parent.transform);
                activeHooks.Add(hook5);
                hook5.thisHookContainer = parent;
                parent.levelText.text = "" + ((int)hook5.hookLevel);
                parent.isOccupied = true;
                hook5.SetHookControllerLevel(HookLevel.FIVE);
                hookControllers.Add(hook5.hookController);
                break;

            case HookLevel.SIX:
                HookBase hook6 = Instantiate(hooks[5], pos, Quaternion.identity, parent.transform);
                activeHooks.Add(hook6);
                hook6.thisHookContainer = parent;
                parent.levelText.text = "" + ((int)hook6.hookLevel);
                parent.isOccupied = true;
                hook6.SetHookControllerLevel(HookLevel.SIX);
                hookControllers.Add(hook6.hookController);
                break;

            case HookLevel.SEVEN:
                HookBase hook7 = Instantiate(hooks[6], pos, Quaternion.identity, parent.transform);
                activeHooks.Add(hook7);
                hook7.thisHookContainer = parent;
                parent.levelText.text = "" + ((int)hook7.hookLevel);
                parent.isOccupied = true;
                hook7.SetHookControllerLevel(HookLevel.SEVEN);
                hookControllers.Add(hook7.hookController);
                break;

            case HookLevel.EIGHT:
                HookBase hook8 = Instantiate(hooks[7], pos, Quaternion.identity, parent.transform);
                activeHooks.Add(hook8);
                hook8.thisHookContainer = parent;
                parent.levelText.text = "" + ((int)hook8.hookLevel);
                parent.isOccupied = true;
                hook8.SetHookControllerLevel(HookLevel.EIGHT);
                hookControllers.Add(hook8.hookController);
                break;

            case HookLevel.NINE:
                HookBase hook9 = Instantiate(hooks[8], pos, Quaternion.identity, parent.transform);
                activeHooks.Add(hook9);
                hook9.thisHookContainer = parent;
                parent.levelText.text = "" + ((int)hook9.hookLevel);
                parent.isOccupied = true;
                hook9.SetHookControllerLevel(HookLevel.NINE);
                hookControllers.Add(hook9.hookController);
                break;

            case HookLevel.TEN:
                HookBase hook10 = Instantiate(hooks[9], pos, Quaternion.identity, parent.transform);
                activeHooks.Add(hook10);
                hook10.thisHookContainer = parent;
                parent.levelText.text = "" + ((int)hook10.hookLevel);
                parent.isOccupied = true;
                hook10.SetHookControllerLevel(HookLevel.TEN);
                hookControllers.Add(hook10.hookController);
                break;

            case HookLevel.ELEVEN:
                HookBase hook11 = Instantiate(hooks[10], pos, Quaternion.identity, parent.transform);
                activeHooks.Add(hook11);
                hook11.thisHookContainer = parent;
                parent.levelText.text = "" + ((int)hook11.hookLevel);
                parent.isOccupied = true;
                hook11.SetHookControllerLevel(HookLevel.ELEVEN);
                hookControllers.Add(hook11.hookController);
                break;

            default:
                hookLevel = HookLevel.None;
                break;
        }
    }
    #endregion


    //public void CheckFirstHook()
    //{
    //    if (hookControllers.Count <= 0)
    //        return;
    //    for(int i = 0; i < hookControllers.Count-1; i++)
    //    {
    //        if (hookControllers[i].count > hookControllers[i + 1].count)
    //        {
    //            FollowCamera.Instance.target = hookControllers[i].transform;
    //        }
    //    }
    //}
    public bool CanStartTopull()
    {
        foreach (HookController hook in hookControllers)
        {
            if (!hook.isReached)
            {
                return false;
            }
        }
        return true;
    }

    IEnumerator PullThecars()
    {
        yield return new WaitForSeconds(1f);
        hasCoroutineStarted = false;
        foreach (CarController car in carControllers)
        {
            car.canPull = true;          
        }
       
        foreach (HookController hook in hookControllers)
        {
            hook.canPull = true;
        }
        //presentGameState = GameState.Merging;
        if (carControllers.Count <= 0)
        {
            Debug.Log("Ho gaya");
            presentGameState = GameState.Merging;
            yield break;
        }
    }

    public void OnThrowTheHooksButtonPressed()
    {
        if (hookControllers.Count <= 0)
            return;
        buttonPanel.SetActive(false);
        presentGameState = GameState.Throwing;
        isHooksMoving = true;
        foreach(HookBase hookBase in activeHooks)
        {
            hookBase.ThrowTheHooks();        
        }
    }
}
