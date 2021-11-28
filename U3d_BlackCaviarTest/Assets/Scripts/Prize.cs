using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Prize : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private Canvas _canvas = default;
    [SerializeField] private RectTransform _prize = default;
    private Vector3 _startPosition;
    private Vector3 _offset;
    private Coroutine _moveRoutine;
    private RectTransform _basket;
    private Action _onBasket;
    public int Id { get; private set; }

    public void Init(int id, RectTransform basket, Action onBasket, Vector3 position, Vector2 size)
    {
        _startPosition = position;
        _prize.position = position;
        _prize.sizeDelta = size;
        Id = id;
        _basket = basket;
        _onBasket += onBasket;
    }

    private void Start() =>
        _canvas.sortingOrder = Constants.PrizeDefaultSorting;

    public void OnReturn() =>
        _onBasket = null;

    public void OnPointerDown(PointerEventData eventData)
    {
        _canvas.sortingOrder = Constants.PrizeSelectedSorting;
        _offset = eventData.position - (Vector2)_prize.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_moveRoutine != null)
            StopCoroutine(_moveRoutine);

        if (_prize.WorldSpaceRect().Overlaps(_basket.WorldSpaceRect()))
            _moveRoutine = StartCoroutine(MoveTo(_basket.WorldSpaceRect().center, true));
        else
            _moveRoutine = StartCoroutine(MoveTo(_startPosition));
    }

    private IEnumerator MoveTo(Vector2 position, bool invoke = false)
    {
        var time = 0f;

        while (time < Constants.PrizeMoveTime)
        {
            yield return null;
            time += Time.deltaTime;
            var newPos = Vector2.Lerp(_prize.position, position, time);
            _prize.position = newPos;
        }

        _canvas.sortingOrder = Constants.PrizeDefaultSorting;

        if (invoke)
            _onBasket?.Invoke();
    }

    public void OnDrag(PointerEventData eventData) =>
        _prize.position = eventData.position - (Vector2)_offset;
}