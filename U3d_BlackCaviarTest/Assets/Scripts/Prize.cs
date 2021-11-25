using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Prize : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private Vector3 _startPosition;
    private Vector3 _offset;
    private Coroutine _moveRoutine;
    private RectTransform _basket;
    private Action _onBasket;
    public int Id { get; private set; }

    public void Init(int id, RectTransform basket, Action onBasket)
    {
        _startPosition = transform.position;

        Id = id;
        _basket = basket;
        _onBasket += onBasket;
    }

    public void OnDestroy()
    {
        _onBasket = null;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _offset = eventData.position - (Vector2)transform.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_moveRoutine != null)
            StopCoroutine(_moveRoutine);

        var prize = (transform as RectTransform);
        
        if (GetWorldSpaceRect(prize).Overlaps(GetWorldSpaceRect(_basket)))
            _moveRoutine = StartCoroutine(MoveTo(GetWorldSpaceRect(_basket).center, true));
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
            var newPos = Vector2.Lerp(transform.position, position, time);
            transform.position = newPos;
        }

        if (invoke)
            _onBasket?.Invoke();
    }

    public void OnDrag(PointerEventData eventData) =>
        transform.position = eventData.position - (Vector2)_offset;
    
    private Rect GetWorldSpaceRect(RectTransform rt)
    {
        var r = rt.rect;
        r.center = rt.TransformPoint(r.center);
        r.size = rt.TransformVector(r.size);
        return r;
    }
}