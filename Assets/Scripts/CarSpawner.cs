using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Car spawner instantiates the cars dynamically just we have to give the start point
/// </summary>
public class CarSpawner : Singleton<CarSpawner>
{
    [SerializeField] private List<GameObject> carPrefabs;
    [SerializeField] private int numRows = 8;
    [SerializeField] private int numColumns = 5;
    [SerializeField] private float xSpacing = 1.0f;
    [SerializeField] private Transform startingPoint;
    [SerializeField] private float rowSpacing = 1.0f;
    [SerializeField] private List<GameObject> giftPrefabs;
    private float[] maxZSizeInColumn;
    private List<GameObject> carsOnGrid = new List<GameObject>();

    void Start()
    {
        SpawnCars();
    }

    // Helper function to get the size of a game object along the Z-axis
    float GetZSizeOfObject(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
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

        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numColumns; col++)
            {
                int prefabIndex = Random.Range(0, carPrefabs.Count);
                GameObject car = Instantiate(carPrefabs[prefabIndex], new Vector3(currentX, 0.8f, currentZ), Quaternion.identity);
                float zSize = GetZSizeOfObject(car);
                maxZSizeInColumn[col] = Mathf.Max(zSize, maxZSizeInColumn[col]);
                currentX += xSpacing;
                carsOnGrid.Add(car);
            }

            float maxZSize = Mathf.Max(maxZSizeInColumn);
            currentX = startingPoint.position.x;
            currentZ += maxZSize + xSpacing;
        }

        float additionalRowStartZ = currentZ + rowSpacing;

        for (int col = 0; col < numColumns; col++)
        {
            int prefabIndex = col % giftPrefabs.Count;

            GameObject instantiatedPrefab = Instantiate(giftPrefabs[prefabIndex], new Vector3(currentX, 0.8f, additionalRowStartZ), Quaternion.identity);

            currentX += xSpacing;
        }
    }

    public void ResetGame()
    {
        foreach(GameObject car in carsOnGrid)
        {
            carsOnGrid.Remove(car);
            Destroy(car);
        }

        carsOnGrid.Clear();

        SpawnCars();
    }
}

