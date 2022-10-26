using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    //public event System.Action OnSizeChanged;    
    [SerializeField] private float width = 6;
    [SerializeField] private float height = 6;
    [SerializeField] private Cylinder cylinderObject;

    Cylinder[] cylinders;
    //Vector3[] cylinderPoss;
    int cylinderLayerNumber = 6;

    // for all cylinder should child of an Object
    string holderName = "Generated Cylinders";
    Transform cylinderHolder;

    // for aligning the cylinders
    float cylinderRadius;
    float cylinderLength;

    // the number of cylinders will be half so for 3 -- 6 cylinders the width should be no less than 6 and no more than 12 
    // same thing for the height
    Vector2 minMaxWidth = new Vector2(6, 12);
    Vector3 minMaxHeight = new Vector2(6, 14);
    
    private void Start()
    {        
        cylinderRadius = cylinderObject.transform.localScale.x / 2;
        cylinderLength = cylinderObject.transform.localScale.y;
        GenerateAllCylinders();
        GenerateQuad();
    }

    // plane generating
    [ContextMenu ("Regenerate")]
    void GenerateQuad() 
    {
        string holderName = "GeneratedQuad";
        
        if (transform.Find(holderName))
        {
            DestroyImmediate(transform.Find(holderName).gameObject);
        }

        Transform quadHolder = new GameObject(holderName).transform;
        quadHolder.parent = transform;

        MeshRenderer meshRenderer = quadHolder.gameObject.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = new Material(Shader.Find("Standard"));

        MeshFilter meshFilter = quadHolder.gameObject.AddComponent<MeshFilter>();

        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[4]
        {
            // bottom left, bottom right, top left, top right
            new Vector3(-width/2, 0, -height/2),
            new Vector3(width/2, 0, -height/2),
            new Vector3(-width/2, 0, height/2),
            new Vector3(width/2, 0, height/2)
        };
        mesh.vertices = vertices;

        int[] tris = new int[6]
        {
            // lower left triangle
            0, 2, 1,
            // upper right triangle
            2, 3, 1
        };
        mesh.triangles = tris;

        Vector3[] normals = new Vector3[4]
        {
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward
        };
        mesh.normals = normals;

        Vector2[] uv = new Vector2[4]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1)
        };
        mesh.uv = uv;

        meshFilter.mesh = mesh;
        //GenerateCylinders();
        AlignCylinders();
    }

    #region Deprecated
    void GenerateCylinders()
    {
        string holderName = "Generated Cylinders";

        if (transform.Find(holderName))
        {
            DestroyImmediate(transform.Find(holderName).gameObject);
        }

        Transform cylinderHolder = new GameObject(holderName).transform;
        cylinderHolder.parent = transform;

        int horizontalCylinderNumber = (int)(width / 2);
        int verticalCylinderNumber = (int)(height / 2);
        int cylinderCount = horizontalCylinderNumber * verticalCylinderNumber;

        Vector3[] cylinderPositions = new Vector3[cylinderCount];

        //for (int i = 0; i < cylinderCount; i++)
        //{
        //    cylinderPositions[i] = new Vector3(-width / 2 + (i + 1) * cylinderRadius, 0, -height / 2 + (i + 1) * cylinderRadius);
        //    Instantiate(cylinderObject, cylinderPositions[i], Quaternion.identity);
        //}

        int index = 0;
        for (int i = 0; i < horizontalCylinderNumber; i++)
        {
            for (int j = 0; j < verticalCylinderNumber; j++)
            {
                cylinderPositions[index] = new Vector3(-width / 2 + ((i + cylinderRadius * 2) * 2 - 1),
                    cylinderLength,
                    -height / 2 + ((j + cylinderRadius * 2) * 2 - 1));

                Cylinder newCylinder = Instantiate(cylinderObject, cylinderPositions[index], Quaternion.identity);
                newCylinder.gameObject.layer = cylinderLayerNumber;
                newCylinder.transform.parent = cylinderHolder;
                index++;
            }
        }
    }
    #endregion

    void AlignCylinders()
    {
        int horizontalCylinderNumber = (int)(width / 2);
        int verticalCylinderNumber = (int)(height / 2);

        // reset all cylinders 
        for (int i = 0; i < cylinders.Length; i++)
        {
            cylinders[i].ResetColor();
            cylinders[i].gameObject.SetActive(false);
        }

        // adjusting the positions of the appropriate cylinders and setActive true
        int index = 0;
        for (int i = 0; i < horizontalCylinderNumber; i++)
        {
            for (int j = 0; j < verticalCylinderNumber; j++)
            {
                Vector3 currentCylinderPos = new Vector3(-width / 2 + ((i + cylinderRadius * 2) * 2 - 1),
                    cylinderLength,
                    -height / 2 + ((j + cylinderRadius * 2) * 2 - 1));
                cylinders[index].transform.position = currentCylinderPos;
                cylinders[index].gameObject.SetActive(true);
                index++;
            }
        }

    }

    // Generating all cylinders so wont istantiate anymore if on max width and max height
    // will only use the SetActive function when necessary
    void GenerateAllCylinders()
    {
        if (transform.Find(holderName))
        {
            DestroyImmediate(transform.Find(holderName).gameObject);
        }

        int maxCylinderCount = (int)(minMaxWidth.y / 2 * minMaxHeight.y / 2);
        cylinders = new Cylinder[maxCylinderCount];
        int index = 0;
        cylinderHolder = new GameObject(holderName).transform;
        cylinderHolder.parent = transform;

        for (int i = 0; i < minMaxWidth.y / 2; i++)
        {
            for (int j = 0; j < minMaxHeight.y / 2; j++)
            {
                cylinders[index] = Instantiate(cylinderObject, new Vector3(0, 0, 0), Quaternion.identity) as Cylinder;
                cylinders[index].gameObject.layer = cylinderLayerNumber;
                cylinders[index].gameObject.tag = "Cylinder";
                cylinders[index].transform.parent = cylinderHolder;
                cylinders[index].gameObject.SetActive(false);
                index++;
            }
        }
    }

    public void ChangeWidth(int widthIncreaseAmount) 
    {
        float newWidth = width + widthIncreaseAmount;

        if (newWidth < minMaxWidth.x || newWidth > minMaxWidth.y)
        {
            return;
        }

        width += widthIncreaseAmount;

        GenerateQuad();
    }

    public void ChangeHeight(int heightIncreaseAmount) 
    {
        float newHeight = height + heightIncreaseAmount;

        if (newHeight < minMaxHeight.x || newHeight > minMaxHeight.y)
        {
            return;
        }

        height += heightIncreaseAmount;

        GenerateQuad();
    }
}