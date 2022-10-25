using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private Color[] colors;
    [SerializeField] private RawImage nextColorImage;
    [SerializeField] private Button colorButton;
    
    GridGenerator gridGenerator;
    Brush brush;

    int currentColorIndex = 0;
    int nextColorIndex = 1;

    private void Start()
    {
        gridGenerator = FindObjectOfType<GridGenerator>();
        brush = FindObjectOfType<Brush>();
        colorButton.GetComponent<Image>().color = colors[currentColorIndex];
        nextColorImage.color = colors[nextColorIndex];
    }

    public void ChangeWidth(int width)
    {
        gridGenerator.ChangeWidth(width);
    }

    public void ChangeHeight(int height)
    {
        gridGenerator.ChangeHeight(height);
    }

    public void SetBrushColorToDefault()
    {
        brush.SetBrushColorToDefault();
    }

    public void SetBrushColor()
    {
        brush.SetBrushColor(colors[currentColorIndex]);
        nextColorImage.color = colors[nextColorIndex];
        colorButton.GetComponent<Image>().color = colors[currentColorIndex];
        currentColorIndex = (currentColorIndex + 1) % colors.Length;
        nextColorIndex = (nextColorIndex + 1) % colors.Length;
    }
}
