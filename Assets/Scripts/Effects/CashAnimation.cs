using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class CashAnimation : MonoBehaviour
{
    public int coinValue;
    public float moveDuration = 1.0f;
    public TextMeshProUGUI cashText;
    public GameObject coinPrefab;
    public Ease ease;

    public void MoveCoin(int _coinvalue)
    {
        coinValue = _coinvalue;
        RectTransform rect = GetComponent<RectTransform>();
        cashText.text = coinValue.ToString();
        rect.DOAnchorPos(Vector3.zero, moveDuration).SetEase(ease).OnComplete(() =>
        {
            CollectCoin();
        });
    }

    private void CollectCoin()
    {
        UpdatePlayerScore();
        Destroy(gameObject);
    }

    private void UpdatePlayerScore()
    {
        GameManager.Instance.AddCash(coinValue);
    }
}
