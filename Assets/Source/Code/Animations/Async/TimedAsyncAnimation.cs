using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class TimedAsyncAnimation : AsyncAnimation
{
    [field: SerializeField] public float Duration { get; set; }
    [field: SerializeField] public float Delay { get; set; }
    
    public override async UniTask Execute(CancellationToken token)
    {
        IsExecuting = true;
            
        var time = 0f;

        if (Delay != 0) await UniTask.Delay(TimeSpan.FromSeconds(Delay), cancellationToken: token);
            
        StartDefault();
        await StartAsync();

        while (time < Duration)
        {
            Evaluate(time / Duration);

            await UniTask.DelayFrame(1, cancellationToken: token);

            if (token.IsCancellationRequested)
            {
                break;
            }
                
            time += Time.deltaTime;
        }
            
        Evaluate(1);
        End();
            
        IsExecuting = false;
    }

    protected abstract void Evaluate(float t);
    
    protected virtual void StartDefault() { }
    
    protected virtual UniTask StartAsync() { return UniTask.CompletedTask; }
    
    protected virtual void End() { }
}