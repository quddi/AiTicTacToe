using System;
using UnityEngine;

[Serializable]
public class ConstantEasing : IEasing
{
    [SerializeField] private float _value;
    
    public float Evaluate(float t) => _value;
}