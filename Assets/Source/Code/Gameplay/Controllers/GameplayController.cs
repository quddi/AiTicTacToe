using System;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    public FieldCell[,] Field { get; private set; }

    public event Action FieldUpdated;
    
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
}