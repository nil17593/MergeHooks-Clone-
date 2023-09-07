using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeHooks : MonoBehaviour
{
    private Transform selectedObject;
    private Vector3 initialPosition;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Raycast to detect if we clicked on a GameObject
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Check if the clicked GameObject is one we want to merge
                if (hit.collider.CompareTag("Player"))
                {
                    selectedObject = hit.collider.transform;
                    initialPosition = selectedObject.position;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (selectedObject != null)
            {
                // Raycast to detect if we released the mouse over another GameObject
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    // Check if the released GameObject is also mergeable
                    if (hit.collider.CompareTag("Player") && hit.collider.transform != selectedObject)
                    {
                        // Merge the objects by scaling the selectedObject
                        selectedObject.localScale += hit.collider.transform.localScale;
                        Destroy(hit.collider.gameObject); // Destroy the merged object

                        // Reset the selected object's position
                        selectedObject.position = initialPosition;
                    }
                }

                // Deselect the object
                selectedObject = null;
            }
        }

        if (selectedObject != null)
        {
            // Move the selectedObject while the mouse button is held
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            selectedObject.position = new Vector3(mousePos.x, mousePos.y, selectedObject.position.z);
        }
    }
}
