using System;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] private Text _text = default;
    [SerializeField] private Button _button = default;

    public void Init(Action onClick) => 
        _button.onClick.AddListener(()=>onClick?.Invoke());

    public void SetText(string text) =>
        _text.text = text;
    
    private void OnDestroy() => 
        _button.onClick.RemoveAllListeners();
}