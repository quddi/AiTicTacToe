using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AbsoluteScaleAsyncAnimation : TimedAsyncAnimation
{
    [field: SerializeReference] public EasingsVector3 Easings { get; set; } = new();
    [field: SerializeField] public List<Transform> ScaledTransforms { get; set; }= new();
    
    protected override void Evaluate(float t)
    {
        foreach (var scaledTransform in ScaledTransforms)
        {
            scaledTransform.localScale = Easings.Evaluate(t);
        }
    }
}