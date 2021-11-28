using System;
using System.IO;
using UnityEngine;

public class SaveToPersistent : ISaver
{
    public void Save(string data, Action success)
    {
        var path = Application.persistentDataPath + "/" + Constants.GameDataFileName;
        Debug.Log(path);
        File.WriteAllText(path, data);
        success?.Invoke();
    }
}