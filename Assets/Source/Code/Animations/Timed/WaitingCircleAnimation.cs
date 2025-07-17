using System;
using System.Threading;
using UnityEngine;

public class WaitingCircleAnimation : TimedAnimation
{
    [SerializeField] private Vector3 _defaultScale;
    [SerializeField] private Vector3 _targetScale;
    [SerializeField] private AnimationCurve _scaleCurve;
    
    protected override void UpdateAnimation(float t)
    {
        transform.localScale = Vector3.Lerp(_defaultScale, _targetScale, _scaleCurve.Evaluate(t));
    }
}