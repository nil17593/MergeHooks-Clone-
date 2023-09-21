using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CashAnimation : MonoBehaviour
{
    private void OnEnable()
    {
        transform.DOMoveY(4f, .2f).OnComplete(() =>
        {
            if (GameManager.Instance.carControllers.Count <= 0)
            {
                Debug.Log("HO GAYA");
                CarSpawner.Instance.ResetGame();
                GameManager.Instance.presentGameState = GameManager.GameState.Merging;
            }
        });
        Destroy(gameObject, 2f);
    }


}
