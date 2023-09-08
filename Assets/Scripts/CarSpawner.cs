using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Car spawner instantiates the cars dynamically just we have to give the start point
/// </summary>
public class CarSpawner : MonoBehaviour
{
    public GameObject[] carPrefabs; // Array of car prefabs with varying sizes
    public int rows = 4;
    public int columns = 8;
    public float spacing = 1f; // Distance between cars (X axis)
    public float forwardDistance = 1f; // Distance to move forward before instantiating the next row
    public Transform startingPoint; // Specify the starting point in the Unity Inspector

    void Start()
    {
        SpawnCars();
    }

    //spawning of different type of cars
    void SpawnCars()
    {
        Vector3 currentPosition = startingPoint.position;

        for (int col = 0; col < columns; col++)
        {
            for (int row = 0; row < rows; row++)
            {
                GameObject randomCarPrefab = carPrefabs[Random.Range(0, carPrefabs.Length)];

                GameObject newCar = Instantiate(randomCarPrefab, currentPosition, Quaternion.identity);

                float carZScale = newCar.transform.localScale.z;
                float zSpacing = spacing * carZScale;

                currentPosition.z += zSpacing;
            }
            currentPosition.x += spacing;
            currentPosition.z = startingPoint.position.z;
        }
    }
}

