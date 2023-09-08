using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Gamemanager class handles spawning of Hooks and holds the list of HookContainers
/// 
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    #region public lists and arrays
    public List<HookBase> activeHooks = new List<HookBase>();
    public List<HookContainer> hookContainers = new List<HookContainer>();
    public HookBase[] hooks;
    #endregion

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
;    }

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
            activeHooks.Add(hookBase);
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
                break;

            case HookLevel.THREE:
                HookBase hook3 = Instantiate(hooks[2], pos, Quaternion.identity, parent.transform);
                activeHooks.Add(hook3);
                hook3.thisHookContainer = parent;
                parent.levelText.text = "" + ((int)hook3.hookLevel);
                parent.isOccupied = true;
                break;

            case HookLevel.FOUR:
                HookBase hook4 = Instantiate(hooks[3], pos, Quaternion.identity, parent.transform);
                activeHooks.Add(hook4);
                hook4.thisHookContainer = parent;
                parent.levelText.text = "" + ((int)hook4.hookLevel);
                parent.isOccupied = true;
                break;

            case HookLevel.FIVE:
                HookBase hook5 = Instantiate(hooks[4], pos, Quaternion.identity, parent.transform);
                activeHooks.Add(hook5);
                hook5.thisHookContainer = parent;
                parent.levelText.text = "" + ((int)hook5.hookLevel);
                parent.isOccupied = true;
                break;

            case HookLevel.SIX:
                HookBase hook6 = Instantiate(hooks[5], pos, Quaternion.identity, parent.transform);
                activeHooks.Add(hook6);
                hook6.thisHookContainer = parent;
                parent.levelText.text = "" + ((int)hook6.hookLevel);
                parent.isOccupied = true;
                break;

            case HookLevel.SEVEN:
                HookBase hook7 = Instantiate(hooks[6], pos, Quaternion.identity, parent.transform);
                activeHooks.Add(hook7);
                hook7.thisHookContainer = parent;
                parent.levelText.text = "" + ((int)hook7.hookLevel);
                parent.isOccupied = true;
                break;

            case HookLevel.EIGHT:
                HookBase hook8 = Instantiate(hooks[7], pos, Quaternion.identity, parent.transform);
                activeHooks.Add(hook8);
                hook8.thisHookContainer = parent;
                parent.levelText.text = "" + ((int)hook8.hookLevel);
                parent.isOccupied = true;
                break;

            case HookLevel.NINE:
                HookBase hook9 = Instantiate(hooks[8], pos, Quaternion.identity, parent.transform);
                activeHooks.Add(hook9);
                hook9.thisHookContainer = parent;
                parent.levelText.text = "" + ((int)hook9.hookLevel);
                parent.isOccupied = true;
                break;

            case HookLevel.TEN:
                HookBase hook10 = Instantiate(hooks[9], pos, Quaternion.identity, parent.transform);
                activeHooks.Add(hook10);
                hook10.thisHookContainer = parent;
                parent.levelText.text = "" + ((int)hook10.hookLevel);
                parent.isOccupied = true;
                break;

            case HookLevel.ELEVEN:
                HookBase hook11 = Instantiate(hooks[10], pos, Quaternion.identity, parent.transform);
                activeHooks.Add(hook11);
                hook11.thisHookContainer = parent;
                parent.levelText.text = "" + ((int)hook11.hookLevel);
                parent.isOccupied = true;
                break;
        }
    }
    #endregion
}
