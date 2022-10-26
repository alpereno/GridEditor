using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonHelper: MonoBehaviour
{
    TextFileManager tfm;
    private void Start()
    {
        tfm = FindObjectOfType<TextFileManager>();
    }

    // -External- creating Json String and input an array
    public string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }

    public void CreateJsonFile()
    {
        GameObject[] cylinders = GameObject.FindGameObjectsWithTag("Cylinder");
        CylinderInfo[] cylinderInfoArray = new CylinderInfo[cylinders.Length];

        for (int i = 0; i < cylinders.Length; i++)
        {
            GameObject cylinder = cylinders[i];
            cylinderInfoArray[i] = new CylinderInfo();
            cylinderInfoArray[i].Color = cylinder.GetComponent<Renderer>().material.color.ToString();
            cylinderInfoArray[i].Coordinate = new Coordinate(cylinder.transform.position.x, cylinder.transform.position.y, cylinder.transform.position.z);
        }

        string jsonStr = ToJson(cylinderInfoArray, true);

        tfm.AddInfoToFile(jsonStr);
    }
}
