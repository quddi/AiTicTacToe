using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TriInspector;
using UnityEngine;

public class FieldView : MonoBehaviour
{

    [SerializeField] private GameplayController _gameplayController;
    [SerializeField] private List<FieldCellView> _cellViews = new();

    private bool _isAiTurn;
    private FieldCellView[,] _field;
    
    private void Awake()
    {
        _field = new FieldCellView[Constants.FieldDimension, Constants.FieldDimension];

        for (var i = 0; i < _field.GetLength(0); i++)
        {
            for (var j = 0; j < _field.GetLength(1); j++)
            {
                _field[i, j] = _cellViews[i * _field.GetLength(0) + j];
            }
        }
    }

    private void Start()
    {
        SetAiTurn(false);
    }

    private void FieldUpdatedHandler()
    {
        for (int i = 0; i < _field.GetLength(0); i++)
        {
            for (int j = 0; j < _field.GetLength(1); j++)
            {
                UpdateCellView(i, j);
            }
        }
    }

    private void UpdateCellView(int x, int y)
    {
        var cell = _gameplayController.Field[x, y];
        var cellView = _field[x, y];
        
        cellView.UpdateState(cell);
    }

    private void SetAiTurn(bool isTurn)
    {
        _isAiTurn = isTurn;
    }

    private void CellUpdatedHandler(int x, int y)
    {
        UpdateCellView(x, y);
    }

    private async void CellViewClickedHandler(int x, int y)
    {
        if (_isAiTurn) return;
        
        if (!_gameplayController.Field[x, y].IsEmpty || _gameplayController.GameResult != GameResult.None) return;
        
        _gameplayController.MakeTurn(x, y);

        SetAiTurn(true);

        if (_gameplayController.GameResult != GameResult.None) return;
        
        await _gameplayController.MakeAiTurn();

        SetAiTurn(false);
    }

    private void GameRestartedHandler()
    {
        SetAiTurn(false);
    }

    private void OnEnable()
    {
        _gameplayController.FieldUpdated += FieldUpdatedHandler;   
        _gameplayController.CellUpdated += CellUpdatedHandler;
        _gameplayController.GameRestarted += GameRestartedHandler;

        foreach (var cellView in _cellViews)
        {
            cellView.Clicked += CellViewClickedHandler;
        }
    }

    private void OnDisable()
    {
        _gameplayController.FieldUpdated -= FieldUpdatedHandler;   
        _gameplayController.CellUpdated -= CellUpdatedHandler;
        _gameplayController.GameRestarted -= GameRestartedHandler;
        
        foreach (var cellView in _cellViews)
        {
            cellView.Clicked -= CellViewClickedHandler;
        }
    }
}