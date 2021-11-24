using System;
using UnityEngine;

public class LoadFromResources : ILoader
{
    public void Load(MonoBehaviour owner, string filename, Action<string> success, Action fail)
    {
        var textAsset = Resources.Load<TextAsset>(filename);
        success?.Invoke(textAsset.text);    
    }
}