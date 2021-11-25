using System;
using UnityEngine;
using UnityEngine.UI;

public class BoardCell : MonoBehaviour
{
    [SerializeField] private Button _button = default;
    private event Action _onButtonClick;
    private void Start()
    {
        _button.onClick.AddListener(OnButtonClick);
    }

    private void OnDestroy() => 
        _button.onClick.RemoveAllListeners();

    public void OnClick(Action onButtonClick) => 
        _onButtonClick += onButtonClick;

    public void UpdateCell(int depth)
    {
        GetComponentInChildren<Text>().text = depth.ToString();
    }

    private void OnButtonClick() => 
        _onButtonClick?.Invoke();
}