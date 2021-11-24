using System;
using UnityEngine;

public interface ILoader
{
    void Load(MonoBehaviour owner, string filename, Action<string> success, Action fail);
}