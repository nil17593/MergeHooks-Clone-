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
    public static event Action OnCarsPulled;
    public List<GameObject> giftBoxes = new List<GameObject>();
    public bool[,] occupiedPositions;// = new bool[numRows, numColumns];
    public int carRandomSpawningRate;
    public bool isGiftBoxInstantiatedOnce = false;

    void Start()
    {
        occupiedPositions = new bool[numRows, numColumns];
        SpawnCars();
    }

    public void TriggerCarsPulledEvent()
    {
        OnCarsPulled?.Invoke();
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

    private void OnEnable()
    {
        OnCarsPulled += ResetGame;
    }

    private void OnDestroy()
    {
        OnCarsPulled -= ResetGame;
    }

    public void SpawnCars()
    {
        maxZSizeInColumn = new float[numColumns];
        float currentX = startingPoint.position.x;
        float currentZ = startingPoint.position.z;

        if (!GameManager.Instance.isThisLevelCleared)
        {
            for (int row = 0; row < numRows; row++)
            {
                for (int col = 0; col < numColumns; col++)
                {           
                    // Check if the position is occupied.
                    if (!occupiedPositions[row, col])
                    {
                        int prefabIndex = UnityEngine.Random.Range(0, carPrefabs.Count);
                        Vector3 spawnPosition = new Vector3(currentX, 0.5f, currentZ);
                        int randomPercentage = UnityEngine.Random.Range(0, 101);
                        if (randomPercentage > carRandomSpawningRate)
                        {
                            CarController car = Instantiate(carPrefabs[prefabIndex], spawnPosition, Quaternion.identity);
                            float zSize = GetZSizeOfObject(car);
                            maxZSizeInColumn[col] = Mathf.Max(zSize, maxZSizeInColumn[col]);
                            carsOnGrid.Add(car);

                            // Mark the position as occupied.
                            occupiedPositions[row, col] = true;
                            car.SetPosition(row, col);
                        }
                    }
                    currentX += xSpacing;
                }

                float maxZSize = Mathf.Max(maxZSizeInColumn);
                currentX = startingPoint.position.x;
                currentZ += maxZSize + xSpacing;
            }
            float additionalRowStartZ = currentZ + rowSpacing;
            if (!isGiftBoxInstantiatedOnce)
            {
                isGiftBoxInstantiatedOnce = true;
                for (int col = 0; col < numColumns; col++)
                {
                    int prefabIndex = col % giftPrefabs.Count;

                    GameObject giftBox = Instantiate(giftPrefabs[prefabIndex], new Vector3(currentX, 0.8f, additionalRowStartZ), Quaternion.identity);
                    giftBoxes.Add(giftBox);
                    currentX += xSpacing;
                }
            }
        }
        else
        {
            for (int row = 0; row < numRows; row++)
            {
                for (int col = 0; col < numColumns; col++)
                {
                    // Check if the position is occupied.
                    //if (!occupiedPositions[row, col])
                    //{
                        int randomPercentage = UnityEngine.Random.Range(0, 101);
                    if (randomPercentage > carRandomSpawningRate)
                    {
                        int prefabIndex = UnityEngine.Random.Range(0, carPrefabs.Count);
                        Vector3 spawnPosition = new Vector3(currentX, 0.5f, currentZ);
                        CarController car = Instantiate(carPrefabs[prefabIndex], spawnPosition, Quaternion.identity);
                        float zSize = GetZSizeOfObject(car);
                        maxZSizeInColumn[col] = Mathf.Max(zSize, maxZSizeInColumn[col]);
                        carsOnGrid.Add(car);

                        // Mark the position as occupied.
                        occupiedPositions[row, col] = true;
                        car.SetPosition(row, col);
                    }
                    //}
                    currentX += xSpacing;
                }

                float maxZSize = Mathf.Max(maxZSizeInColumn);
                currentX = startingPoint.position.x;
                currentZ += maxZSize + xSpacing;
            }
            float additionalRowStartZ = currentZ + rowSpacing;
            for (int col = 0; col < numColumns; col++)
            {
                int prefabIndex = col % giftPrefabs.Count;

                GameObject giftBox = Instantiate(giftPrefabs[prefabIndex], new Vector3(currentX, 0.8f, additionalRowStartZ), Quaternion.identity);
                giftBoxes.Add(giftBox);
                currentX += xSpacing;
            }
        }

    }

    public void ResetGame()
    {
        Invoke(nameof(OnAllCarsPulled), 2f);
    }

    void OnAllCarsPulled()
    {
        if (GameManager.Instance.isThisLevelCleared)
        {
            List<CarController> carsToDestroy = new List<CarController>();

            foreach (CarController car in carsOnGrid)
            {
                carsToDestroy.Add(car);
            }

            foreach (CarController car in carsToDestroy)
            {
                carsOnGrid.Remove(car);
                Destroy(car);
            }
            List<GameObject> giftsToDestroy = new List<GameObject>();

            foreach (GameObject giftBox in giftBoxes)
            {
                giftsToDestroy.Add(giftBox);
            }

            foreach (GameObject giftBox in giftsToDestroy)
            {
                giftBoxes.Remove(giftBox);
                Destroy(giftBox);
            }
            carsOnGrid.Clear();
            //Debug.Log("ASDD");
            giftBoxes.Clear();
        }
        SpawnCars();
        GameManager.Instance.presentGameState = GameManager.GameState.Merging;
    }

    int CalculateNumberOfCarsToSpawn(int gridSize, int randomPercentage)
    {
        return Mathf.FloorToInt(gridSize * randomPercentage / 100);
    }
}

