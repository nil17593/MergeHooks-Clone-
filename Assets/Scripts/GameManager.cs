using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<GameObject> hookSpawningTransforms = new List<GameObject>();
    public List<HookBase> activeHooks = new List<HookBase>();
    public List<HookContainer> hookContainers = new List<HookContainer>();
    public HookBase hook;

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
    }

    public void SpawnHooks()
    {
        if (hookSpawningTransforms.Count > 0 && activeHooks.Count < hookSpawningTransforms.Count)
        {
            HookContainer hookContainer = GetRandomHookContainer();
            HookBase hookBase = Instantiate(hook, hookContainer.transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity, hookContainer.transform);
            hookBase.thisHookContainer = hookContainer;
            hookContainer.isOccupied = true;
            activeHooks.Add(hookBase);
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
            return containers[0]; // Return the first (randomized) unoccupied HookContainer
        }
        else
        {
            return null; // Handle the case where all HookContainers are occupied
        }

    }

    //public static void Shuffle<T>(this IList<T> list)
    //{
    //    int n = list.Count;
    //    while (n > 1)
    //    {
    //        n--;
    //        int k = Random.Range(0, n + 1);
    //        T value = list[k];
    //        list[k] = list[n];
    //        list[n] = value;
    //    }
    //}
}
