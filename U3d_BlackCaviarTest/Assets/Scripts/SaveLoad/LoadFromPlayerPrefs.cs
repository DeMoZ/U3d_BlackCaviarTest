using System;
using UnityEngine;

public class LoadFromPlayerPrefs : ILoader
{
    public void Load(MonoBehaviour owner, string filename, Action<string> success, Action fail)
    {
        try
        {
            var data = PlayerPrefs.GetString(filename);
            success?.Invoke(data);
        }
        catch
        {
            fail?.Invoke();
        }
    }
}