using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using TMPro;

public class EnableDisableGameObject : MonoBehaviour
{
    public GameObject targetGameObject;
    public Image timerImage;
    public TextMeshProUGUI amount;
    public int claimedAmount=1;
    public int currentAmount = 500;
    public int baseAmount = 250;

    public float enableDuration = 5f;  // Time in seconds for which the object will be enabled
    public float cycleTime = 10f; // Total time in seconds for one cycle (enable + disable)

    private Vector3 originalScale;
    public float punchScaleMagnitude = 1.2f;  // How much the scale will increase
    public float punchScaleDuration = 0.5f;  // How long the punch scale effect will last
    private Tween scaleTween;

    private Coroutine enableDisableRoutine = null; // this variable will hold the reference to the coroutine
    private Coroutine ongoingRoutine = null;
    private Coroutine timerRoutine = null;

    private void Start()
    {
        if (targetGameObject == null)
        {
            Debug.LogError("Target GameObject is not assigned");
            return;
        }

        if (timerImage == null)
        {
            Debug.LogError("Timer Image is not assigned");
            return;
        }

        originalScale = targetGameObject.transform.localScale;

        enableDisableRoutine = StartCoroutine(EnableDisableRoutine());
        ongoingRoutine = StartCoroutine(EnableDisableRoutine());
        // Punch scale loop
        scaleTween = targetGameObject.transform.DOScale(originalScale * punchScaleMagnitude, punchScaleDuration)
        .SetEase(Ease.OutBounce).SetLoops(-1, LoopType.Yoyo);
    }

    private IEnumerator EnableDisableRoutine()
    {
        while (true)
        {
            scaleTween = targetGameObject.transform.DOScale(originalScale * punchScaleMagnitude, punchScaleDuration)
       .SetEase(Ease.OutBounce).SetLoops(-1, LoopType.Yoyo);
            targetGameObject.SetActive(true);
            timerRoutine = StartCoroutine(UpdateTimerImage());
           
            yield return new WaitForSeconds(enableDuration);
            // Stop the scaling loop when the object is disabled
            
            targetGameObject.transform.localScale = originalScale;
            targetGameObject.SetActive(false);
           
            yield return new WaitForSeconds(cycleTime - enableDuration);
        }
    }

    private IEnumerator UpdateTimerImage()
    {
        curTime = 0;
        while (curTime <= enableDuration)
        {
            curTime += Time.deltaTime;
            timerImage.fillAmount = 1f - (curTime / enableDuration);
            if (curTime >= enableDuration)
            {
                targetGameObject.SetActive(false);
                claimedAmount++;
                currentAmount = claimedAmount * baseAmount;
                amount.text = "+" + currentAmount + "";

            }
            yield return null;
        }
    }
    float curTime;
    public void ResetAndDisableGameObject()
    {
        curTime = enableDuration - 0.1f;
        claimedAmount++;
        currentAmount = claimedAmount * baseAmount;
        amount.text = "+" + currentAmount + "";
        targetGameObject.SetActive(false);

        /*  if (ongoingRoutine != null)
          {
              StopCoroutine(ongoingRoutine);
              ongoingRoutine = null;
          }

          if (timerRoutine != null)
          {
              StopCoroutine(timerRoutine);
              timerRoutine = null;
          }

          // Immediately disable the object and reset the image fill amount
          targetGameObject.SetActive(false);
          timerImage.fillAmount = 1f;

          // Wait for the remaining time in the cycle before enabling the GameObject again
          ongoingRoutine = StartCoroutine(ResetAndEnableRoutine());
          claimedAmount++;
          currentAmount = claimedAmount * baseAmount;
          amount.text = "+"+currentAmount + "";*/

    }
    private IEnumerator ResetAndEnableRoutine()
{
    yield return new WaitForSeconds(cycleTime - enableDuration);
    ongoingRoutine = StartCoroutine(EnableDisableRoutine());
}
   
}
