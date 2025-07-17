using System;
using DG.Tweening;
using DG.Tweening.Core.Easing;
using UnityEngine;

[Serializable]
public class DOTweenEasing : IEasing
{
    [SerializeField] private Ease _easeType;
    [SerializeField] private float _overshootOrApmlitude;
    [SerializeField] private float _period;
        
    public float Evaluate(float t)
    {
        return EaseManager.Evaluate(_easeType, default, t, 1, _overshootOrApmlitude, _period);
    }
}