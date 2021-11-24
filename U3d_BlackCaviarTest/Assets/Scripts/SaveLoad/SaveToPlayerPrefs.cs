using System;
using UnityEngine;

public class SaveToPlayerPrefs : ISaver
{
    public void Save(string data, Action success)
    {
        PlayerPrefs.SetString(Constants.GameDataFileName,data);
        success?.Invoke();
    }
}