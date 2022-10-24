using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent (typeof (MeshFilter))]
//[RequireComponent (typeof (MeshRenderer))]
public class GridGenerator : MonoBehaviour
{
    //public event System.Action OnWidthChanged;
    //[SerializeField] private int xSize;
    //[SerializeField] private int ySize;
    //[SerializeField] private Mesh mesh;

    [SerializeField] private float width = 6;
    [SerializeField] private float height = 6;
    [SerializeField] private Cylinder cylinderObject;
    // it should be local scale x and z 
    float cylinderRadius;
    float cylinderRadiusX;
    float cylinderRadiusZ;
    float cylinderLength;
    float ratioX = 0.16f; // 1/6 ratio
    float ratioZ = 0.16f;

    //private Vector3[] vertices;


    private void Start()
    {
        cylinderRadius = cylinderObject.transform.localScale.x / 2;
        cylinderLength = cylinderObject.transform.localScale.y;
        //CalculateAndArrangeRadius();
        //Generate();
        GenerateQuad();
    }

    //void Generate() 
    //{
    //    mesh = GetComponent<MeshFilter>().mesh;
    //    mesh = new Mesh();
    //    mesh.name = "Procedural Grid";

    //    vertices = new Vector3[(xSize + 1) * (ySize + 1)];
    //    int index = 0;
    //    for (int y = 0; y <= ySize; y++)
    //    {
    //        for (int x = 0; x <= xSize; x++)
    //        {
    //            vertices[index] = new Vector3(x, y);
    //            index++;
    //        }
    //    }
    //    mesh.vertices = vertices;

    //    int[] triangles = new int[6];
    //    triangles[0] = 0;
    //    triangles[3] = triangles[2] = 1;
    //    triangles[4] = triangles[1] = xSize + 1;
    //    triangles[5] = xSize + 2;
    //}

    //private void OnDrawGizmos()
    //{
    //    if (vertices == null)
    //    {
    //        return;
    //    }

    //    Gizmos.color = Color.black;
    //    for (int i = 0; i < vertices.Length; i++)
    //    {
    //        Gizmos.DrawSphere(vertices[i], 0.1f);
    //    }
    //}

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
        GenerateCylinders();
    }

    void GenerateCylinders()
    {
        string holderName = "Generated Cylinders";

        if (transform.Find(holderName))
        {
            DestroyImmediate(transform.Find(holderName).gameObject);
        }

        Transform cylinderHolder = new GameObject(holderName).transform;
        cylinderHolder.parent = transform;

        //CalculateAndArrangeRadius();
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
                newCylinder.gameObject.layer = 6;
                newCylinder.transform.parent = cylinderHolder;
                index++;
            }
        }
    }

    void CalculateAndArrangeRadius()
    {
        //ratioX = 1 / width;
        //ratioZ = 1 / height;

        //cylinderRadiusX = width * ratioX / 2;
        //cylinderRadiusZ = height * ratioZ / 2;
        //Vector3 localScale = Vector3.up + Vector3.forward * cylinderRadiusZ + Vector3.right * cylinderRadiusX;
        //cylinderObject.transform.localScale = localScale * 2;
        //print(cylinderObject.transform.localScale);

        cylinderRadiusX = (width * ratioX) / 2;
        cylinderRadiusZ = (height * ratioZ) / 2;

        Vector3 localScale = Vector3.up / 2 + Vector3.forward * cylinderRadiusZ + Vector3.right * cylinderRadiusX;
        cylinderObject.transform.localScale = localScale * 2;
        print(cylinderObject.transform.localScale + "x radius = " + cylinderRadiusX + "z radius = " + cylinderRadiusZ);

    }

    public void ChangeWidth(int widthIncreaseAmount) 
    {
        float newWidth = width + widthIncreaseAmount;

        if (newWidth < 6 || newWidth > 12)
        {
            return;
        }
        width += widthIncreaseAmount;
        GenerateQuad();
    }

    public void ChangeHeight(int heightIncreaseAmount) 
    {
        float newHeight = height + heightIncreaseAmount;
        if (newHeight < 6 || newHeight > 14)
        {
            return;
        }
        height += heightIncreaseAmount;
        GenerateQuad();
    }
}
