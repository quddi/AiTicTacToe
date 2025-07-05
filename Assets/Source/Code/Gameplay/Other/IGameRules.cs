public interface IGameRules
{
    GameResult EstimateGameEnd(FieldCell[,] field);
}