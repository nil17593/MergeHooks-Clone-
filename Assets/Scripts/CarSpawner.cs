using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Car spawner instantiates the cars dynamically just we have to give the start point
/// </summary>
public class CarSpawner : Singleton<CarSpawner>
{
    [SerializeField] private List<CarController> carPrefabs;
    [SerializeField] private int numRows = 8;
    [SerializeField] private int numColumns = 5;
    [SerializeField] private float xSpacing = 1.0f;
    [SerializeField] private Transform startingPoint;
    [SerializeField] private float rowSpacing = 1.0f;
    [SerializeField] private List<GameObject> giftPrefabs;
    private float[] maxZSizeInColumn;
    public List<CarController> carsOnGrid = new List<CarController>();
    public static event Action<bool> onPulledCars;
    public List<GameObject> giftBoxes = new List<GameObject>();
    public bool[,] occupiedPositions;// = new bool[numRows, numColumns];

    void Start()
    {
        occupiedPositions = new bool[numRows, numColumns];
        SpawnCars();
    }

    // Helper function to get the size of a game object along the Z-axis
    float GetZSizeOfObject(CarController obj)
    {
        Renderer renderer = obj.gameObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            return renderer.bounds.size.z;
        }
        return 0f;
    }

    public void SpawnCars()
    {
        maxZSizeInColumn = new float[numColumns];
        float currentX = startingPoint.position.x;
        float currentZ = startingPoint.position.z;

        //bool[,] occupiedPositions = new bool[numRows, numColumns]; // Initialize a grid to track occupied positions.
        int gridSize = numRows * numColumns;
        int num = UnityEngine.Random.Range(0, 100);
        int numOfCarsToSpawn = Mathf.FloorToInt(gridSize * num / 100);
        //Debug.Log("NUM===  " + numOfCarsToSpawn);
        int count = 0;
        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numColumns; col++)
            {
                int prefabIndex = UnityEngine.Random.Range(0, carPrefabs.Count);
                Vector3 spawnPosition = new Vector3(currentX, 0.8f, currentZ);

                // Check if the position is occupied.
                if (!occupiedPositions[row, col])
                {

                    CarController car = Instantiate(carPrefabs[prefabIndex], spawnPosition, Quaternion.identity);
                    float zSize = GetZSizeOfObject(car);
                    maxZSizeInColumn[col] = Mathf.Max(zSize, maxZSizeInColumn[col]);
                    carsOnGrid.Add(car);

                    // Mark the position as occupied.
                    occupiedPositions[row, col] = true;
                    car.SetPosition(row, col);
                }

                currentX += xSpacing;
            }

            float maxZSize = Mathf.Max(maxZSizeInColumn);
            currentX = startingPoint.position.x;
            currentZ += maxZSize + xSpacing;
        }

        float additionalRowStartZ = currentZ + rowSpacing;
    }



    public void ResetGame()
    {
        //List<GameObject> carsToDestroy = new List<GameObject>();

        //foreach (GameObject car in carsOnGrid)
        //{
        //    carsToDestroy.Add(car);
        //}

        //foreach (GameObject car in carsToDestroy)
        //{
        //    carsOnGrid.Remove(car);
        //    Destroy(car);
        //}
        //List<GameObject> giftsToDestroy = new List<GameObject>();

        //foreach (GameObject giftBox in giftBoxes)
        //{
        //    giftsToDestroy.Add(giftBox);
        //}

        //foreach (GameObject giftBox in giftsToDestroy)
        //{
        //    giftBoxes.Remove(giftBox);
        //    Destroy(giftBox);
        //}
        //carsOnGrid.Clear();
       // giftBoxes.Clear();
        SpawnCars();
    }


    public void OnCarsPulled()
    {
        if (onPulledCars != null)
        {
            onPulledCars.Invoke(true);
        }
    }
}

