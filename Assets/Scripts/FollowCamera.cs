using UnityEngine;

public class FollowCamera : Singleton<FollowCamera>
{
    public Transform target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float smoothFactor;
    private Transform playerLastPos;


    void FollowPlayer()
    {
        if (target != null)
        {
            float x = Mathf.Clamp(0, 0f, 0f);
            Vector3 targetPosition = target.position + offset + new Vector3(x, target.position.y, target.position.z);
            Debug.Log("pos: " + target.position);
            Vector3 smoothedposition = Vector3.Lerp(transform.position, targetPosition, smoothFactor * Time.deltaTime);
            transform.position = targetPosition;
        }
    }

    private void LateUpdate()
    {
        GameManager.Instance.CheckFirstHook();
        FollowPlayer();
    }
}
