using System;

[Serializable]
public class DefaultSquareGameRules : IGameRules
{
    public bool EstimateGameEnd(FieldCell[,] field)
    {
        if (field.GetLength(0) != field.GetLength(1))
            throw new ArgumentException($"Field must be square, but length were {field.GetLength(0)} and {field.GetLength(1)}");

        EstimateHorizontal(field);
        EstimateVertical(field);
        EstimateDiagonal(field);
        
        return IsGameEnded(field);
    }

    private void EstimateHorizontal(FieldCell[,] field)
    {
        for (int i = 0; i < field.GetLength(0); i++)
        {
            var currentResult = true;
            
            for (int j = 0; j < field.GetLength(1) - 1; j++)
            {
                currentResult = currentResult && AreSameTeamCells(field[i, j], field[i, j + 1]);
            }

            if (currentResult)
            {
                for (int j = 0; j < field.GetLength(1); j++)
                {
                    field[i, j].IsWinning = true;
                }
                
                return;
            }
        }
    }
    
    private void EstimateVertical(FieldCell[,] field)
    {
        for (int i = 0; i < field.GetLength(1); i++)
        {
            var currentResult = true;
            
            for (int j = 0; j < field.GetLength(0) - 1; j++)
            {
                currentResult = currentResult && AreSameTeamCells(field[j, i], field[j + 1, i]);
            }

            if (currentResult)
            {
                for (int j = 0; j < field.GetLength(1); j++)
                {
                    field[j, i].IsWinning = true;
                }
                
                return;
            }
        }
    }
    
    private void EstimateDiagonal(FieldCell[,] field)
    {
        var dimension = field.GetLength(0);
        
        var currentResult = true;
        
        for (int i = 0; i < dimension - 1; i++)
        {
            currentResult = currentResult && AreSameTeamCells(field[i, i], field[i + 1, i + 1]);
        }

        if (currentResult)
        {
            for (int i = 0; i < dimension; i++)
            {
                field[i, i].IsWinning = true;
            }
            
            return;
        }

        currentResult = true;
        
        for (int i = 0; i < dimension - 1; i++)
        {
            currentResult = currentResult && AreSameTeamCells(field[i, dimension - i - 1], field[i + 1, dimension - i - 2]);
        }

        if (!currentResult) return;
        
        for (int i = 0; i < dimension; i++)
        {
            field[i, dimension - i - 1].IsWinning = true;
        }
    }

    private bool AreSameTeamCells(FieldCell cell1, FieldCell cell2)
    {
        return cell1.TeamId == cell2.TeamId && !cell1.IsEmpty && !cell2.IsEmpty;
    }

    private bool IsGameEnded(FieldCell[,] field)
    {
        var anyWin = false;
        var allFilled = true;
        
        for (int i = 0; i < field.GetLength(0); i++)
        {
            for (int j = 0; j < field.GetLength(1); j++)
            {
                anyWin = anyWin || field[i, j].IsWinning;
                allFilled = allFilled && !field[i, j].IsEmpty;
            }
        }

        return anyWin || allFilled;
    }
}