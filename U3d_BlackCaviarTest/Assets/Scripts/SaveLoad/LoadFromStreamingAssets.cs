using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class LoadFromStreamingAssets : ILoader
{
    public void Load(MonoBehaviour owner, string filename, Action<string> success, Action fail)
    {
        owner.StartCoroutine(LoadAssets(filename, success, fail));
    }
    
    private IEnumerator LoadAssets(string fileName, Action<string> success, Action fail)
    {
        var link = Path.Combine(Application.streamingAssetsPath, fileName);
        Debug.Log($"link = {link}");

        using (var www = UnityWebRequest.Get(link))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
                fail?.Invoke();
            else
                success?.Invoke(www.downloadHandler.text);
        }
    }
}