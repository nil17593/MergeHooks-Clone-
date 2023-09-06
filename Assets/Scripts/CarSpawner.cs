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
        //GenerateBoard();
        SpawnCars();
    }

    void SpawnCars()
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

/*
public void GenerateBoard()
{
    //BoardSize boardSize = GamePlayUI.Instance.GetBoardSize();

    int rowSize = rows;
    int columnSize = columns;

    // Fetched the size of block that should be used.
    float blockSize = 1.5f;// = GamePlayUI.Instance.currentGameMode == GameMode.Level ? gamePlaySettings.globalLevelModeBlockSize : GamePlayUI.Instance.currentModeSettings.blockSize;
                           // Fetched the space between blocks that should be used.
    float blockSpace = 1.5f;// GamePlayUI.Instance.currentGameMode == GameMode.Level ? gamePlaySettings.globalLevelModeBlockSpace : GamePlayUI.Instance.currentModeSettings.blockSpace;

    //Set background and design Grid Size and design type
    //gridBackground.GetComponent<RectTransform>().sizeDelta = new Vector2((rowSize * blockSize) + ((rowSize - 1) * blockSpace) + gridBackgroundOffset, (columnSize * blockSize) + ((columnSize - 1) * blockSpace) + gridBackgroundOffset);
    //gridDesign.GetComponent<RectTransform>().sizeDelta = new Vector2((rowSize * blockSize) + ((rowSize - 1) * blockSpace) + gridDesignOffset, (columnSize * blockSize) + ((columnSize - 1) * blockSpace) + gridDesignOffset);
    //gridBorder.GetComponent<RectTransform>().sizeDelta = new Vector2((rowSize * blockSize) + ((rowSize - 1) * blockSpace) + gridBorderOffset, (columnSize * blockSize) + ((columnSize - 1) * blockSpace) + gridBorderOffset);
    //gridDesign.GetComponent<Image>().sprite = ThemeManager.Instance.GetBlockSpriteWithTag(rowSize.ToString());

    // Starting points represents point from where block shape grid should start inside block shape.
    float startPointX = GetStartPointX(blockSize, columnSize);
    float startPointY = GetStartPointZ(blockSize, rowSize);

    // Will keep updating with iterations.
    float currentPositionX = startPointX;
    float currentPositionY = startPointY;

    //List<Block> blockRow = new List<Block>();

    for (int column = 0; column < columnSize; column++)
    {
        // Spawn a block instance and prepares it.
        GameObject randomCarPrefab = carPrefabs[Random.Range(0, carPrefabs.Length)];

        // Instantiate the random car at the current position
        GameObject newCar = Instantiate(randomCarPrefab, Vector3.zero, Quaternion.identity);
        newCar.transform.localPosition = new Vector3(currentPositionX, 0.5f, currentPositionY);
        currentPositionX += (blockSize + blockSpace);

        // Sets blocks logical position inside grid and its default sprite.
    }
    currentPositionX = startPointX;
    currentPositionY -= (blockSize + blockSpace);

}

public float GetStartPointX(float blockSize, int rowSize)
{
    float blockSpace = 1.5f;// GamePlayUI.Instance.currentGameMode == GameMode.Level ? gamePlaySettings.globalLevelModeBlockSpace : GamePlayUI.Instance.currentModeSettings.blockSpace;
    float totalWidth = (blockSize * rowSize) + ((rowSize - 1) * blockSpace);
    return -((totalWidth / 2) - (blockSize / 2));
}

public float GetStartPointZ(float blockSize, int columnSize)
{
    float blockSpace = 1.5f;// GamePlayUI.Instance.currentGameMode == GameMode.Level ? gamePlaySettings.globalLevelModeBlockSpace : GamePlayUI.Instance.currentModeSettings.blockSpace;
    float totalHeight = (blockSize * columnSize) + ((columnSize - 1) * blockSpace);
    return ((totalHeight / 2) - (blockSize / 2));
}
*/
