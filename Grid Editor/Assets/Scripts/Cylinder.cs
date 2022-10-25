using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cylinder : MonoBehaviour
{
    Material material;
    Color defaultColor;
    Color color;

    private void Awake()
    {
        material = gameObject.GetComponent<Renderer>().material;
        defaultColor = material.color;
        color = defaultColor;        
    }

    public void ChangeColor(Color newColor)
    {
        color = newColor;
        material.color = color;
    }

    public void ResetColor()
    {
        ChangeColor(defaultColor);
    }
}
