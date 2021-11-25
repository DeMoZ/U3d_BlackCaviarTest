using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BoardCell : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Button _button = default;
    
    private Cell _cell;
    public bool HasPrise { get; set; }

    /*public void OnClick(Action onClick)
    {
        _button.onClick.AddListener(()=> { onClick?.Invoke();});
    }*/
    public void Init(Cell cell)
    {
        _cell = cell;
        
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(()=>OnClick());
        
        DecorateCell();
    }

    
    private void OnDestroy() => 
        _button.onClick.RemoveAllListeners();

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log($"pointerDown");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log($"pointerUp");
    }

    private void OnClick()
    {
        if (_cell.Depth > 0)
            UpdateCell();

    }

    private void UpdateCell()
    {
        _cell.Depth--;
        DecorateCell();
    }
    private void DecorateCell()
    {
        GetComponentInChildren<Text>().text = _cell.Depth.ToString();
    }


}