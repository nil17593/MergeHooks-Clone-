using UnityEngine;
using TMPro;


/// <summary>
/// Hook Container class attached on every Hook Container 
/// indicated the mergable Hooks by highlighting
/// changes the level texts to indicate level of the current Hook
/// </summary>
public class HookContainer : MonoBehaviour
{
    #region Public Booleans
    public bool isOccupied;
    #endregion

    #region Public References of Gameobjects
    public TextMeshProUGUI levelText;
    public GameObject levelTextHolder;
    #endregion

    #region Private Properties
    private MeshRenderer meshRenderer;
    private Color defaultColor;
    private Material material;
    #endregion

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        material=GetComponent<MeshRenderer>().material;
        defaultColor = material.color;
    }


    #region Change Colour Methods
    public void ChangeColor()
    {
        material.color = Color.green;
    }

    public void ResetColor()
    {
        material.color = defaultColor;
    }
    #endregion

    public void ActivateDeactivateLevelText(bool status)
    {
        levelTextHolder.SetActive(status);
    }
}
