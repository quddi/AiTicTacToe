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
    private List<string> _turnsLoop;
    
    public bool GameEnded { get; private set; }
    public string CurrentTurnTeam { get; private set; }
    public FieldCell[,] Field { get; private set; }
    
    public event Action FieldUpdated;
    public event Action TurnPassed;
    public event Action<int, int> CellUpdated;

    [Inject]
    private void Construct(ITeamsManager teamsManager)
    {
        _teamsManager = teamsManager;
        _turnsLoop = _teamsManager.TurnsLoop;

        CurrentTurnTeam = _turnsLoop[0];
    }
    
    private void Awake()
    {
        Field = new FieldCell[Constants.FieldDimension, Constants.FieldDimension];

        for (var i = 0; i < Constants.FieldDimension; i++)
        {
            for (var j = 0; j < Constants.FieldDimension; j++)
            {
                Field[i, j] = new FieldCell(i, j);
            }
        }
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
        
        if (GameEnded) throw new Exception("Game is ended");
        
        Field[x, y].TeamId = CurrentTurnTeam;

        GameEnded = _gameRules.EstimateGameEnd(Field);
        
        FieldUpdated?.Invoke();
        
        if (!GameEnded) 
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