using UnityEngine;

[System.Serializable]
public class CylinderInfo
{
    public string Color;
    public Coordinate Coordinate;
}

[System.Serializable]
public class Coordinate
{
    public float X;
    public float Y;
    public float Z; 

    public Coordinate(float x, float y, float z) 
    {
        this.X = x;
        this.Y = y;
        this.Z = z;
    }
}
