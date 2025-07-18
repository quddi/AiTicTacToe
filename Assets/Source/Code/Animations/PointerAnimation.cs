using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointerAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private float _scaleDuration = 0.1f;
    [SerializeField] private Vector3 _defaultScale = Vector3.one;
    [SerializeField] private Vector3 _targetScale = new(0.8f, 0.8f, 0.8f);
    [SerializeField] private AnimationCurve _scaleCurve = new();

    [field: SerializeField] public bool IsInteractable { get; set; } = true;
    
    private bool _pointerEntered;
    private bool _pointedDown;
    private float _time;
    private bool _hadToScale;
    
    private bool MustScale => _pointerEntered && _pointedDown && IsInteractable;

    private void Update()
    {
        var shouldScale = MustScale;

        if (shouldScale != _hadToScale)
        {
            _time = 0f;
            _hadToScale = shouldScale;
        }

        if (!(_time < _scaleDuration)) return;
        
        _time += Time.unscaledDeltaTime;
        
        var t = Mathf.Clamp01(_time / _scaleDuration);
        var curveValue = _scaleCurve.Evaluate(t);
        var from = shouldScale ? _defaultScale : _targetScale;
        var to = shouldScale ? _targetScale : _defaultScale;

        transform.localScale = Vector3.LerpUnclamped(from, to, curveValue);
    }

    public void OnPointerEnter(PointerEventData eventData) => _pointerEntered = true;

    public void OnPointerExit(PointerEventData eventData) => _pointerEntered = false;

    public void OnPointerUp(PointerEventData eventData) => _pointedDown = false;

    public void OnPointerDown(PointerEventData eventData) => _pointedDown = true;
}