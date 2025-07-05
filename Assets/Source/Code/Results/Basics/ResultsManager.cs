using System;
using System.Collections.Generic;

public class ResultsManager : IResultsManager
{
    private readonly List<GameResultInfo> _results = new();
    
    public IReadOnlyList<GameResultInfo> Results => _results;
    
    public event Action<GameResultInfo> ResultAdded;

    public void AddResult(GameResultInfo gameResultInfo)
    {
        _results.Add(gameResultInfo);
        
        ResultAdded?.Invoke(gameResultInfo);
    }
}