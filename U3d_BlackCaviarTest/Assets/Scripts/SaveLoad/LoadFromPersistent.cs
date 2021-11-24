using System;
using System.IO;
using UnityEngine;

public class LoadFromPersistent : ILoader
{
    public void Load(MonoBehaviour owner, string filename, Action<string> success, Action fail)
    {
        try
        {
            var data = File.ReadAllText(Application.persistentDataPath + "/" + filename);
            success?.Invoke(data);
        }
        catch
        {
            fail?.Invoke();
        }
    }
}