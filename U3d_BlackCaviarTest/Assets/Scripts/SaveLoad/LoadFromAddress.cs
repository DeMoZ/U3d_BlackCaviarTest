using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class LoadFromAddress : ILoader
{
    public void Load(MonoBehaviour owner, string filename, Action<string> success, Action fail)
    {
        owner.StartCoroutine(IELoad(filename, success, fail));
    }
    
    private IEnumerator IELoad(string fileName, Action<string> onComplete, Action fail)
    {
        var link = Path.Combine(Application.streamingAssetsPath, fileName);

        if (!link.Contains("http") && !link.Contains("file"))
            link = "file://" + link;

        Debug.Log($"link = {link}");

        using (var www = UnityWebRequest.Get(link))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
                fail?.Invoke();
            else
                onComplete?.Invoke(www.downloadHandler.text);
        }
    }
}