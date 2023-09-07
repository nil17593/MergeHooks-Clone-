using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookBase : MonoBehaviour
{
    [SerializeField] private HookController hookController;

    private bool isDragging = false;
    private Vector3 offset;
    private Vector3 initialPos;

    private void Start()
    {
        initialPos = transform.position;
    }
    private void OnMouseDown()
    {
        // Calculate the offset between the mouse position and the object's position
        offset = transform.position - GetMouseWorldPos();
        isDragging = true;
    }

    private void OnMouseUp()
    {
        isDragging = false;
        transform.position = initialPos;
    }

    private Vector3 GetMouseWorldPos()
    {
        // Get the mouse position in world coordinates
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.point;
        }
        return Vector3.zero; // Return (0,0,0) if the raycast doesn't hit anything
    }

    private void Update()
    {
        if (isDragging)
        {
            // Update the object's position based on the mouse movement
            Vector3 targetPos = GetMouseWorldPos() + offset;
            targetPos.y = 1f;
            transform.position = targetPos;
        }
    }
}
