using System;
using System.IO;
using UnityEngine;

public class SaveToStreamingAssets : ISaver
{
    public void Save(string data, Action success)
    {
        File.WriteAllText(Application.persistentDataPath + "/"+Constants.GameDataFileName, data);
        success?.Invoke();
    }
}