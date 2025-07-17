using System;
using UnityEngine;

[Serializable]
public class EasingsVector3
{
    [field: SerializeReference] public IEasing X { get; set; } 
    [field: SerializeReference] public IEasing Y { get; set; } 
    [field: SerializeReference] public IEasing Z { get; set; } 
    
    public Vector3 Evaluate(float time) => new(X.Evaluate(time), Y.Evaluate(time), Z.Evaluate(time));
}