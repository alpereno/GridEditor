using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brush : MonoBehaviour
{
    [SerializeField] private LayerMask cylinderMask;
    Camera viewCamera;    
    float maxDistance = 100f;
    public Color brushColor;
    public Color defaultBrushColor;

    private void Start()
    {        
        viewCamera = Camera.main;
        defaultBrushColor = new Color(.2f, .2f, .2f, 1);
        brushColor = defaultBrushColor;
    }

    private void Update()
    {
        MouseClick();
    }

    void MouseClick() 
    {
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Input.GetMouseButton(0))
        {
            if (Physics.Raycast(ray, out hit, maxDistance, cylinderMask, QueryTriggerInteraction.Collide))
            {
                Cylinder hitObjectV2 = hit.collider.GetComponent<Cylinder>();
                if (hitObjectV2 != null)
                {
                    hitObjectV2.ChangeColor(brushColor);
                }
            }
        }            
    }

    public void SetBrushColorToDefault()
    {
        brushColor = defaultBrushColor;
    }

    public void SetBrushColor(Color newColor) 
    {
        brushColor = newColor;
    }
}
