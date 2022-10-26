using System;
using System.IO;
using UnityEngine;

public class TextFileManager : MonoBehaviour
{
    // The Only task is writing given info to textfile
    // This class could be static with JSonHelper

    StreamWriter sW;
    string path;
    string filePath;

    void Start()
    {
        filePath = Application.dataPath;
        print(filePath);
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }
        path = (filePath + @"\CylinderJSonInfo.txt");
        if (!File.Exists(path))
        {
            sW = File.CreateText(path);
            sW.Close();
        }
    }

    public void AddInfoToFile(String info)
    {
        try
        {
            if (!File.Exists(path))
            {
                sW = File.CreateText(path);
                sW.Close();
            }
            sW = File.AppendText(path);
            sW.WriteLine(info);
        }

        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        finally
        {
            sW.Close();
        }
    }
}
