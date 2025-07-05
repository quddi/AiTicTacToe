using System;
using System.Collections.Generic;

public interface IResultsManager
{
    IReadOnlyList<GameResultInfo> Results { get; }

    event Action<GameResultInfo> ResultAdded;
    
    void AddResult(GameResultInfo gameResultInfo);
}