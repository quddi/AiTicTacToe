using System;
using UnityEngine;

[Serializable]
public class InverseEasing : IEasing
{
    [SerializeReference] private IEasing _easing;
    
    public float Evaluate(float t) => _easing.Evaluate(1 - t);
}