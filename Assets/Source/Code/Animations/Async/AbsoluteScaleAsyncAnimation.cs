using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AbsoluteScaleAsyncAnimation : TimedAsyncAnimation
{
    [FormerlySerializedAs("_animationCurves")] [SerializeReference] private EasingsVector3 _easingses = new();
    [SerializeField] private List<Transform> _scaledTransforms = new();
    
    protected override void Evaluate(float t)
    {
        foreach (var scaledTransform in _scaledTransforms)
        {
            scaledTransform.localScale = _easingses.Evaluate(t);
        }
    }
}