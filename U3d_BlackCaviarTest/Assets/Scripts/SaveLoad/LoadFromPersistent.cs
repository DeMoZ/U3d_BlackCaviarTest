using System;
using System.IO;
using UnityEngine;

public class LoadFromPersistent : ILoader
{
    public void Load(MonoBehaviour owner, string filename, Action<string> success, Action fail)
    {
        try
        {
            var path = Application.persistentDataPath + "/" + Constants.GameDataFileName;
            Debug.Log(path);
            var data = File.ReadAllText(path);
            success?.Invoke(data);
        }
        catch
        {
            fail?.Invoke();
        }
    }
}