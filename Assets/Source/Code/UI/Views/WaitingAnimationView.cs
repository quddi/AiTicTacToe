using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class WaitingAnimationView : MonoBehaviour
{
    [SerializeField] private AsyncAnimation _appearAnimation;
    [SerializeField] private AsyncAnimation _disappearAnimation;
    
    private CancellationTokenSource _tokenSource;

    private bool _appeared;

    private void Awake()
    {
        _appeared = true;
        
        Disappear();
    }

    public void Appear()
    {
        if (_appeared) return;
        
        _appeared = true;
        
        ValidateToken();

        _appearAnimation.Execute(_tokenSource.Token).Forget();
    }

    public void Disappear()
    {
        if (!_appeared) return;
        
        _appeared = false;
        
        ValidateToken();

        _disappearAnimation.Execute(_tokenSource.Token).Forget();
    }

    private void ValidateToken()
    {
        _tokenSource?.Cancel();
        _tokenSource?.Dispose();

        _tokenSource = new();
    }
}