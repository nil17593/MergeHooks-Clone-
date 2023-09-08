using UnityEngine;
using TMPro;

public class HookContainer : MonoBehaviour
{
    public bool isOccupied;


    private MeshRenderer meshRenderer;
    private Color defaultColor;
    private Material material;
    private HookBase hookBase;
    public TextMeshProUGUI levelText;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        material=GetComponent<MeshRenderer>().material;
        defaultColor = material.color;
    }

    public void ChangeColor()
    {
        material.color = Color.green;
    }

    public void ResetColor()
    {
        material.color = defaultColor;
    }
}
