using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class TimedAsyncAnimation : AsyncAnimation
{
    [SerializeField] private float _duration;
    [SerializeField] private float _delay;
    
    public override async UniTask Execute(CancellationToken token)
    {
        IsExecuting = true;
            
        var time = 0f;

        if (_delay != 0) await UniTask.Delay(TimeSpan.FromSeconds(_delay), cancellationToken: token);
            
        StartDefault();
        await StartAsync();

        while (time < _duration)
        {
            Evaluate(time / _duration);

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