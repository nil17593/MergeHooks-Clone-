using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<GameObject> hookContainer = new List<GameObject>();
    public List<HookBase> activeHooks = new List<HookBase>();
    public List<HookContainer> hookContainers = new List<HookContainer>();
    public HookBase hook;
    public HookBase levelTwoHook;
    public HookBase levelThreeHook;
    public HookBase[] hooks;

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

    public void SpawnHooks()
    {
        if (hookContainers.Count > 0 && activeHooks.Count < hookContainers.Count)
        {
            HookContainer hookContainer = GetRandomHookContainer();
            HookBase hookBase = Instantiate(hooks[0], hookContainer.transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity, hookContainer.transform);
            hookBase.thisHookContainer = hookContainer;
            hookContainer.isOccupied = true;
            activeHooks.Add(hookBase);
        }
    }

    public void AddMergedHook(HookLevel hookLevel,Vector3 pos,HookContainer parent)
    {
        switch (hookLevel)
        {
            case HookLevel.TWO:
                HookBase hook2 = Instantiate(hooks[1], pos, Quaternion.identity, parent.transform);
                hook2.thisHookContainer = parent;
                activeHooks.Add(hook2);
                parent.isOccupied = true;
                break;

            case HookLevel.THREE:
                HookBase hook3 = Instantiate(hooks[2], pos, Quaternion.identity, parent.transform);
                activeHooks.Add(hook3);
                hook3.thisHookContainer = parent;
                parent.isOccupied = true;
                break;

            case HookLevel.FOUR:
                HookBase hook4 = Instantiate(hooks[3], pos, Quaternion.identity, parent.transform);
                activeHooks.Add(hook4);
                hook4.thisHookContainer = parent;
                parent.isOccupied = true;
                break;

            case HookLevel.FIVE:
                HookBase hook5 = Instantiate(hooks[4], pos, Quaternion.identity, parent.transform);
                activeHooks.Add(hook5);
                hook5.thisHookContainer = parent;
                parent.isOccupied = true;
                break;
        }
    }

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
}
