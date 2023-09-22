using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

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
    public bool isThisLevelCleared = false;
    #endregion
    public static event Action StartCarPulling;
    
    #region Playerprefs
    private const string LevelKey = "CurrentLevel";
    #endregion

    #region UI
    [SerializeField] private TextMeshProUGUI gameCashText;
    [SerializeField] private TextMeshProUGUI levelText;
    #endregion


    private void Start()
    {
        int currentLevel = GetCurrentLevel();
        levelText.text = "LEVEL " + currentLevel;
        gameCashText.text = PlayerPrefs.GetInt("Cash").ToString();
        canHandleTouch = true;
        presentGameState = GameState.Merging;
    }

    private void Update()
    {
        if (presentGameState == GameState.Merging)
        {
            canHandleTouch = true;
            if (!buttonPanel.activeSelf)
            {
                buttonPanel.SetActive(true);
            }
        }
        else if (presentGameState == GameState.Throwing)
        {
            canHandleTouch = false;
            if (buttonPanel.activeSelf)
            {
                buttonPanel.SetActive(false);
            }
        }
        else if (presentGameState == GameState.Pulling)
        {
           

            //if (carControllers.Count <= 0)
            //{
                //if (!hasCoroutineStarted)
                //{
                    hasCoroutineStarted = true;
                    //StartCoroutine(PullThecars());
                    //int currentLevel = GetCurrentLevel();
                    //levelText.text = "LEVEL " + currentLevel;

                //}
            ///}
        }
    }

    public void SaveCurrentLevel(int levelNumber)
    {
        PlayerPrefs.SetInt(LevelKey, levelNumber);
        PlayerPrefs.Save();
    }

    public int GetCurrentLevel()
    {
        return PlayerPrefs.GetInt(LevelKey, 1);
    }

    #region UI Methods

    public void AddCash(int amount)
    {
        int cash = PlayerPrefs.GetInt("Cash");
        cash += amount;
        PlayerPrefs.SetInt("Cash", cash);
        gameCashText.text = cash.ToString();
    }

    public void DeductCoins(int amount)
    {
        int cash = PlayerPrefs.GetInt("Cash");
        cash -= amount;
        PlayerPrefs.SetInt("Cash", cash);
        gameCashText.text = cash.ToString();
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
            hookBase.SetHookControllerLevel(HookLevel.ONE);
            hookContainer.levelText.text = ""+ ((int)hookBase.hookLevel);
            activeHooks.Add(hookBase);
            DeductCoins(50);
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
                activeHooks.Add(hook2);
                parent.isOccupied = true;
                hook2.SetHookControllerLevel(HookLevel.TWO);
                hookControllers.Add(hook2.hookController);
                parent.levelText.text = "" + ((int)hook2.hookLevel);
                break;

            case HookLevel.THREE:
                HookBase hook3 = Instantiate(hooks[2], pos, Quaternion.identity, parent.transform);
                activeHooks.Add(hook3);
                hook3.thisHookContainer = parent;
                parent.isOccupied = true;
                hook3.SetHookControllerLevel(HookLevel.THREE);
                hookControllers.Add(hook3.hookController);
                parent.levelText.text = "" + ((int)hook3.hookLevel);
                break;

            case HookLevel.FOUR:
                HookBase hook4 = Instantiate(hooks[3], pos, Quaternion.identity, parent.transform);
                activeHooks.Add(hook4);
                hook4.thisHookContainer = parent;
                parent.isOccupied = true;
                hook4.SetHookControllerLevel(HookLevel.FOUR);
                hookControllers.Add(hook4.hookController);
                parent.levelText.text = "" + ((int)hook4.hookLevel);
                break;

            case HookLevel.FIVE:
                HookBase hook5 = Instantiate(hooks[4], pos, Quaternion.identity, parent.transform);
                activeHooks.Add(hook5);
                hook5.thisHookContainer = parent;
                parent.isOccupied = true;
                hook5.SetHookControllerLevel(HookLevel.FIVE);
                hookControllers.Add(hook5.hookController);
                parent.levelText.text = "" + ((int)hook5.hookLevel);
                break;

            case HookLevel.SIX:
                HookBase hook6 = Instantiate(hooks[5], pos, Quaternion.identity, parent.transform);
                activeHooks.Add(hook6);
                hook6.thisHookContainer = parent;
                parent.isOccupied = true;
                hook6.SetHookControllerLevel(HookLevel.SIX);
                hookControllers.Add(hook6.hookController);
                parent.levelText.text = "" + ((int)hook6.hookLevel);
                break;

            case HookLevel.SEVEN:
                HookBase hook7 = Instantiate(hooks[6], pos, Quaternion.identity, parent.transform);
                activeHooks.Add(hook7);
                hook7.thisHookContainer = parent;
                parent.isOccupied = true;
                hook7.SetHookControllerLevel(HookLevel.SEVEN);
                hookControllers.Add(hook7.hookController);
                parent.levelText.text = "" + ((int)hook7.hookLevel);
                break;

            case HookLevel.EIGHT:
                HookBase hook8 = Instantiate(hooks[7], pos, Quaternion.identity, parent.transform);
                activeHooks.Add(hook8);
                hook8.thisHookContainer = parent;
                parent.isOccupied = true;
                hook8.SetHookControllerLevel(HookLevel.EIGHT);
                hookControllers.Add(hook8.hookController);
                parent.levelText.text = "" + ((int)hook8.hookLevel);
                break;

            case HookLevel.NINE:
                HookBase hook9 = Instantiate(hooks[8], pos, Quaternion.identity, parent.transform);
                activeHooks.Add(hook9);
                hook9.thisHookContainer = parent;
                parent.isOccupied = true;
                hook9.SetHookControllerLevel(HookLevel.NINE);
                hookControllers.Add(hook9.hookController);
                parent.levelText.text = "" + ((int)hook9.hookLevel);
                break;

            case HookLevel.TEN:
                HookBase hook10 = Instantiate(hooks[9], pos, Quaternion.identity, parent.transform);
                activeHooks.Add(hook10);
                hook10.thisHookContainer = parent;
                parent.isOccupied = true;
                hook10.SetHookControllerLevel(HookLevel.TEN);
                hookControllers.Add(hook10.hookController);
                parent.levelText.text = "" + ((int)hook10.hookLevel);
                break;

            case HookLevel.ELEVEN:
                HookBase hook11 = Instantiate(hooks[10], pos, Quaternion.identity, parent.transform);
                activeHooks.Add(hook11);
                hook11.thisHookContainer = parent;
                parent.isOccupied = true;
                hook11.SetHookControllerLevel(HookLevel.ELEVEN);
                hookControllers.Add(hook11.hookController);
                parent.levelText.text = "" + ((int)hook11.hookLevel);
                break;

            default:
                hookLevel = HookLevel.None;
                break;
        }
    }
    #endregion

    #region Events
    private void OnEnable()
    {
        StartCarPulling += PullThecars;
    }

    private void OnDestroy()
    {
        StartCarPulling -= PullThecars;
    }

    public bool CanStartTopull()
    {
        foreach (HookController hook in hookControllers)
        {
            if (!hook.isReached)
            {
                return false;
            }
        }
        TriggerPullEvent();
        return true;
    }

    public static void TriggerPullEvent()
    {
        StartCarPulling?.Invoke();
    }

    void PullThecars()
    {
        Invoke(nameof(StartPullingCars), 1f);
        //yield return new WaitForSeconds(1f);
        
        //CarSpawner.Instance.ResetGame();
        //hasCoroutineStarted = false;
        //CarSpawner.Instance.SpawnCars();
        //presentGameState = GameState.Merging;

        //if (carControllers.Count <= 0)
        //{
        //    StartCoroutine(SpawnNewCars());
        //}
    }

    void StartPullingCars()
    {
        presentGameState = GameState.Pulling;

        foreach (CarController car in carControllers)
        {
            car.canPull = true;
        }

        foreach (HookController hook in hookControllers)
        {
            hook.canPull = true;
        }
    }

    public IEnumerator SpawnNewCars()
    {
        yield return new WaitForSeconds(2f);

        //if (carControllers.Count <= 0)
        //{
            CarSpawner.Instance.ResetGame();
            GameManager.Instance.presentGameState = GameManager.GameState.Merging;
        //}
    }
    #endregion

    public void OnThrowTheHooksButtonPressed()
    {
        if (presentGameState == GameState.Merging)
        {
            if (hookControllers.Count <= 0)
                return;
            buttonPanel.SetActive(false);
            presentGameState = GameState.Throwing;
            //isHooksMoving = true;
            foreach (HookBase hookBase in activeHooks)
            {
                hookBase.ThrowTheHooks();
            }
        }
    }
}
