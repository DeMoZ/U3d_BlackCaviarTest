using System;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    [SerializeField] private Button _button = default;
    [SerializeField] private Text _scoops = default;
    [SerializeField] private Text _prizes = default;
    [SerializeField] private RectTransform _basket = default;
    public RectTransform Basket => _basket;

    public void SetScoops(int amount) =>
        _scoops.text = amount.ToString();

    public void SetPrize(string amount) =>
        _prizes.text = amount;

    public void Init(Action onClick) => 
        _button.onClick.AddListener(()=>onClick?.Invoke());

    private void OnDestroy() => 
        _button.onClick.RemoveAllListeners();
}