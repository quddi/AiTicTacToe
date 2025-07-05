using System;
using System.Collections.Generic;
using TriInspector;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;

public class GameplayController : MonoBehaviour
{
    [SerializeReference] private IGameRules _gameRules;
    
    private ITeamsManager _teamsManager;
    private IResultsManager _resultsManager;
    private List<string> _turnsLoop;

    public GameResult GameResult { get; private set; }
    public string CurrentTurnTeam { get; private set; }
    public FieldCell[,] Field { get; private set; }
    
    public event Action FieldUpdated;
    public event Action TurnPassed;
    public event Action<int, int> CellUpdated;

    [Inject]
    private void Construct(ITeamsManager teamsManager, IResultsManager resultsManager)
    {
        _resultsManager = resultsManager;
        _teamsManager = teamsManager;
        _turnsLoop = _teamsManager.TurnsLoop;

        Restart();
    }

    public void Restart()
    {
        Field = new FieldCell[Constants.FieldDimension, Constants.FieldDimension];

        for (var i = 0; i < Constants.FieldDimension; i++)
        {
            for (var j = 0; j < Constants.FieldDimension; j++)
            {
                Field[i, j] = new FieldCell(i, j);
            }
        }
        
        CurrentTurnTeam = _turnsLoop[0];
        GameResult = GameResult.None;
        
        FieldUpdated?.Invoke();
        TurnPassed?.Invoke();
    }

    private void Start()
    {
        FieldUpdated?.Invoke();
    }

    private void PassTurn()
    {
        CurrentTurnTeam = _turnsLoop[(_turnsLoop.IndexOf(CurrentTurnTeam) + 1) % _turnsLoop.Count];
        
        TurnPassed?.Invoke();
    }
    
    [Button]
    public void MakeTurn(int x, int y)
    {
        if (!Field[x, y].IsEmpty) throw new Exception($"Cell ({x}, {y}) is not empty");
        
        if (GameResult != GameResult.None) throw new Exception("Game is ended");
        
        Field[x, y].TeamId = CurrentTurnTeam;

        GameResult = _gameRules.EstimateGameEnd(Field);

        if (GameResult != GameResult.None)
        {
            var winnerId = GameResult == GameResult.Win ? CurrentTurnTeam : null;
            
            _resultsManager.AddResult(new GameResultInfo { WinnerId = winnerId, Result = GameResult });
        }
        
        FieldUpdated?.Invoke();
        
        if (GameResult == GameResult.None) 
            PassTurn();
    }

    [Button]
    private void SetCellState(int x, int y, string teamId, bool isWinning)
    {
        Field[x, y].TeamId = teamId;
        Field[x, y].IsWinning = isWinning;
        
        CellUpdated?.Invoke(x, y);
    }
}