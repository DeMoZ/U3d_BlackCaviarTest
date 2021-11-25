using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Prize : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private Vector2 _startPosition;
    private Vector2 _offset;
    private Coroutine _moveRoutine;
    private RectTransform _basket;
    private int _id;
    private Action<int> _onMoveToBasket;

    public void Init(int id, Vector2 startPosition, RectTransform basket)
    {
        _id = id;
        _basket = basket;
        _startPosition = startPosition;
        transform.position = _startPosition;
        //_onMoveToBasket += onMoveToBasket;
    }

    public void OnDestroy()
    {
        _onMoveToBasket = null;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _offset = eventData.position - (Vector2)transform.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_moveRoutine != null)
            StopCoroutine(_moveRoutine);

        /*if ((transform as RectTransform).rect.Overlaps(_basket.rect))
        {
            //_onMoveToBasket?.Invoke(_id);   
        }
        else
        {
            _moveRoutine = StartCoroutine(MoveTo(_startPosition));
        }*/
        
        _moveRoutine = StartCoroutine(MoveTo(_startPosition));
    }

    private IEnumerator MoveTo(Vector2 position)
    {
        var time = 0f;

        while (time < Constants.PrizeMoveTime)
        {
            yield return null;
            time += Time.deltaTime;
            var newPos = Vector2.Lerp(transform.position, position, time);
            transform.position = newPos;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position - _offset;
    }
}