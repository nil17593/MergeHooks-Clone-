using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Threading.Tasks;
using System;

public class UIController : Singleton<UIController>
{
    public GameObject cashPrefab;
    public GameObject targetForCash;
    private Vector3 initialPos;
    private Quaternion initialRotation;
    public RectTransform coin;
    public GameObject sellHookPanel;

    [SerializeField] private RectTransform levelClearPanel;



    public void ShowLevelClearPopup()
    {
        levelClearPanel.DOAnchorPos(Vector3.zero, .5f).SetEase(Ease.InOutElastic).OnComplete(() =>
        {
            levelClearPanel.DOAnchorPos(new Vector2(-2000, 0), 0.5f).SetDelay(1);
        });
    }
        public void ActivateSellHookPanel()
    {
        sellHookPanel.SetActive(true);
    }

    public void DeactivateHookPanel()
    {
        sellHookPanel.SetActive(false);
    }
}

