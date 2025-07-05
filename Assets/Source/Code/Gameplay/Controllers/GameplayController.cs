using System;
using TriInspector;
using UnityEngine;
using UnityEngine.UIElements;

public class GameplayController : MonoBehaviour
{
    public FieldCell[,] Field { get; private set; }

    public event Action FieldUpdated;
    public event Action<int, int> CellUpdated;
    
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

    [Button]
    private void SetCellState(int x, int y, string teamId, bool isWinning)
    {
        Field[x, y].TeamId = teamId;
        Field[x, y].IsWinning = isWinning;
        
        CellUpdated?.Invoke(x, y);
    }
}