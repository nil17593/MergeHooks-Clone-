using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Transform> hookSpawningTransforms;
    public GameObject hook;


    public void SpawnHooks()
    {
        if (hookSpawningTransforms.Count > 0)
        {
            int randomPosNumber = Random.Range(0, hookSpawningTransforms.Count);
            Instantiate(hook, hookSpawningTransforms[randomPosNumber].position+ new Vector3(0,0.8f,0), Quaternion.identity, hookSpawningTransforms[randomPosNumber]);
            hookSpawningTransforms.RemoveAt(randomPosNumber);
        }
    }
}
