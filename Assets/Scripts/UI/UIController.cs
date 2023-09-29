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

    [SerializeField] private GameObject levelClearPanel;
    [SerializeField] private GameObject getOrLeaveRewardPanel;
    [SerializeField] private RectTransform dontHaveEnoughCoinsPanel;
    [SerializeField] private PickerWheel pickerWheel;
    private int currentSpinWheelRewardAmount;

    public void ShowLevelClearPopup()
    {
        levelClearPanel.SetActive(true);
        pickerWheel.OnSpinStart(() =>
            {
            });

        pickerWheel.OnSpinEnd(wheelPiece =>
        {
            Debug.Log("Lable" + wheelPiece.Label + " , amount:" + wheelPiece.Amount);
            currentSpinWheelRewardAmount = wheelPiece.Amount;
            levelClearPanel.SetActive(false);
            getOrLeaveRewardPanel.SetActive(true);
        });
        pickerWheel.Spin();
    }

    public void ShowDontHaveEnoughCoinsPopUp()
    {
        dontHaveEnoughCoinsPanel.DOAnchorPos(Vector2.zero, .5f).SetEase(Ease.InOutElastic).OnComplete(() =>
        {
            dontHaveEnoughCoinsPanel.DOAnchorPos(new Vector2(-2000, 0), 0.5f).SetDelay(1);
        });
    }
    public void ActivateSellHookPanel()
    {
        sellHookPanel.SetActive(true);
    }

    public void ResetShowLevelClearPopup()
    {
        getOrLeaveRewardPanel.SetActive(false);
    }

    public void ResetSpinWheelReward()
    {
        currentSpinWheelRewardAmount = 0;
    }

    public void DeactivateHookPanel()
    {
        sellHookPanel.SetActive(false);
    }

    public int GetCurrentSpinWheelRewardAmount()
    {
        return currentSpinWheelRewardAmount;
    }
}

