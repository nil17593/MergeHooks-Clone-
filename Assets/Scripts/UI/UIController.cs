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
}
