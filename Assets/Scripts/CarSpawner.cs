using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public GameObject[] carPrefabs; // Array of car prefabs with varying sizes
    public int rows = 4;
    public int columns = 8;
    public float spacing = 1f; // Distance between cars (X and Y axis)
    public float forwardDistance = 1f; // Distance to move forward before instantiating the next row
    public Transform startingPoint; // Specify the starting point in the Unity Inspector

    void Start()
    {
        Vector3 currentPosition = startingPoint.position;

        for (int col = 0; col < columns; col++)
        {
            for (int row = 0; row < rows; row++)
            {
                // Randomly select a car prefab from the array
                GameObject randomCarPrefab = carPrefabs[Random.Range(0, carPrefabs.Length)];

                // Instantiate the random car at the current position
                GameObject newCar = Instantiate(randomCarPrefab, currentPosition, Quaternion.identity);

                // Calculate Z-spacing based on the car's scale in the Z-axis
                float carZScale = newCar.transform.localScale.z;
                float zSpacing = spacing * carZScale;

                // Update the current position for the next car
                currentPosition.z += zSpacing; // Adjust as needed for spacing between rows
            }

            // Move to the next column
            currentPosition.x += spacing; // Adjust as needed for spacing between columns
            currentPosition.z = startingPoint.position.z; // Reset the z-position
        }
    }
}
