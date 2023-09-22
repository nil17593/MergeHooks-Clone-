using UnityEngine;
using System.Collections.Generic;

public class FollowCamera : Singleton<FollowCamera>
{
    public List<Transform> hooks = new List<Transform>();
    public float followSpeed = 5.0f; // Adjust this to control camera follow speed
    public float viewportThreshold = 0.8f; // The threshold when camera should start following

    private Transform target;
    private Camera mainCamera;
    private Vector3 initialPos;
    private void Start()
    {
        initialPos = transform.position;
        mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        if (GameManager.Instance.presentGameState == GameManager.GameState.Throwing)
        {
            // Find the closest GameObject to the camera's current position
            Transform closestObject = GameManager.Instance.hookControllers[0].transform;
            float closestDistance = Mathf.Abs(transform.position.z - closestObject.position.z);

            foreach (HookController go in GameManager.Instance.hookControllers)
            {
                float distance = Mathf.Abs(transform.position.z - go.transform.position.z);
                if (distance < closestDistance)
                {
                    closestObject = go.transform;
                    closestDistance = distance;
                }
            }

            // Calculate the threshold distance based on the camera's viewport
            float thresholdDistance = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, viewportThreshold, closestObject.position.z - mainCamera.transform.position.z)).z;

            // Only move the camera when the target object goes outside of the viewport
            if (closestObject.position.z > thresholdDistance)
            {
                // Smoothly move the CameraController to the target's position
                Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y, closestObject.position.z);
                transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
            }
        }
        if(GameManager.Instance.presentGameState == GameManager.GameState.Pulling)
        {
            ReturnToBase();
        }
    }

    public void ReturnToBase()
    {
        Vector3 newPosition = Vector3.MoveTowards(transform.position, initialPos, 20f * Time.deltaTime);
        transform.position = newPosition;
    }


    //public List<Transform> gameObjects = new List<Transform>();
    //public float followSpeed = 5.0f; // Adjust this to control camera follow speed

    //private Transform target; // The target to follow

    //private void Start()
    //{
    //    // Initially, follow the first GameObject
    //    //target = gameObjects[0];
    //}

    //private void Update()
    //{

    //}
}
