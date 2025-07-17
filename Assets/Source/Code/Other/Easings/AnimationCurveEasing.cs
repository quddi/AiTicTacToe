using System;
using UnityEngine;

[Serializable]
public class AnimationCurveEasing : IEasing
{
    [SerializeField] private AnimationCurve _curve;
    
    public float Evaluate(float t) => _curve.Evaluate(t);
}