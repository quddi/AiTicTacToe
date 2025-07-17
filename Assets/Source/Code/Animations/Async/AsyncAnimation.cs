using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class AsyncAnimation : MonoBehaviour
{
    public bool IsExecuting { get; protected set; }
    
    public abstract UniTask Execute(CancellationToken token);
    
    public virtual UniTask Execute() => Execute(CancellationToken.None);
}