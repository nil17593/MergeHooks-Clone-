using UnityEngine;
using System.Collections.Generic;

public class FollowCamera : Singleton<FollowCamera>
{
    public List<Transform> hooks = new List<Transform>();
    public float followSpeed = 5.0f; // Adjust this to control camera follow speed
    public float viewportThreshold = 0.8f; // The threshold when the camera should start following

    private Transform target;
    private Camera mainCamera;
    private Vector3 initialPos;

    private void Start()
    {
        initialPos = transform.position;
        mainCamera = Camera.main;
        target = null; // Initialize the target to null
    }

    private void LateUpdate()
    {
        if (GameManager.Instance.presentGameState == GameManager.GameState.Throwing)
        {
            // Find the moving hook that is farthest ahead
            Transform movingHook = null;
            float farthestZ = float.MinValue;

            foreach (HookController hookController in GameManager.Instance.hookControllers)
            {
                if (!hookController.isReached)
                {
                    float zPosition = hookController.transform.position.z;
                    if (zPosition > farthestZ)
                    {
                        movingHook = hookController.transform;
                        farthestZ = zPosition;
                    }
                }
            }

            if (movingHook != null)
            {
                // Calculate the threshold distance based on the camera's viewport
                float thresholdDistance = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, viewportThreshold, movingHook.position.z - mainCamera.transform.position.z)).z;

                // Only move the camera when the moving hook goes outside of the viewport
                if (movingHook.position.z > thresholdDistance)
                {
                    // Set the moving hook as the target
                    target = movingHook;
                }
            }

            // Follow the target (moving hook)
            if (target != null)
            {
                Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y, target.position.z);
                transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
            }
        }
        else if (GameManager.Instance.presentGameState == GameManager.GameState.Pulling)
        {
            ReturnToBase();
        }
    }

    public void ReturnToBase()
    {
        Vector3 newPosition = Vector3.MoveTowards(transform.position, initialPos, 25f * Time.deltaTime);
        transform.position = newPosition;
    }
}
