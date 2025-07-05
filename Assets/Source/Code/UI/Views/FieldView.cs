using System;
using System.Collections.Generic;
using UnityEngine;

public class FieldView : MonoBehaviour
{
    [SerializeField] private GameplayController _gameplayController;
    [SerializeField] private List<FieldCellView> _cellViews = new();

    private FieldCellView[,] _field;
    
    private void Awake()
    {
        _field = new FieldCellView[Constants.FieldDimension, Constants.FieldDimension];

        for (var i = 0; i < Constants.FieldDimension; i++)
        {
            for (var j = 0; j < Constants.FieldDimension; j++)
            {
                _field[i, j] = _cellViews[i * Constants.FieldDimension + j];
            }
        }
    }

    private void FieldUpdatedHandler()
    {
        for (int i = 0; i < Constants.FieldDimension; i++)
        {
            for (int j = 0; j < Constants.FieldDimension; j++)
            {
                UpdateCellView(i, j);
            }
        }
    }

    private void UpdateCellView(int x, int y)
    {
        var cell = _gameplayController.Field[x, y];
        var cellView = _field[x, y];
        
        cellView.SetTeamIcon(cell.TeamId);
    }

    private void OnEnable()
    {
        _gameplayController.FieldUpdated += FieldUpdatedHandler;   
    }

    private void OnDisable()
    {
        _gameplayController.FieldUpdated -= FieldUpdatedHandler;   
    }
}