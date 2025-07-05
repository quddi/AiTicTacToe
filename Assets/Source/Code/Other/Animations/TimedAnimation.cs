using System;
using UnityEngine;

public abstract class TimedAnimation : MonoBehaviour
{
    [SerializeField] private float _duration;
    [SerializeField] private float _time;
    
    private void Update()
    {
        if (_time == 0) AnimationRestart();
        
        _time += Time.deltaTime;
        
        UpdateAnimation(_time / _duration);

        if (_time >= _duration)
        {
            _time = 0;
        }
    }
    
    protected virtual void AnimationRestart() { }
    
    protected abstract void UpdateAnimation(float t);
}