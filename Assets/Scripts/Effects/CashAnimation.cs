using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CashAnimation : MonoBehaviour
{
    private void OnEnable()
    {
        transform.DOMoveY(2f, 1f).OnComplete(() =>
        {
            Destroy(gameObject, .5f);
        });       
    }
}
