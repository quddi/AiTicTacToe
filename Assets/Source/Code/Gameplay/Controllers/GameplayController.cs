using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Cysharp.Threading.Tasks;
using TriInspector;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;

public class GameplayController : MonoBehaviour
{
#if UNITY_EDITOR
    [Dropdown(nameof(TeamsIds))]
#endif
    [SerializeField] private string _firstPlayerTeam;
    [SerializeReference] private IGameRules _gameRules;
    
    private ITeamsService _teamsService;
    private IResultsManager _resultsManager;
    private IOpenAiService _openAiService;
    
    private List<string> _turnsLoop;

    [field: SerializeField, ReadOnly] public string CurrentTurnTeam { get; private set; }
    [field: SerializeField, ReadOnly] public GameResult GameResult { get; private set; }
    public FieldCell[,] Field { get; private set; }
    
    public event Action FieldUpdated;
    public event Action TurnPassed;
    public event Action GameRestarted;
    public event Action<int, int> CellUpdated;

    [Inject]
    private void Construct(ITeamsService teamsService, IResultsManager resultsManager, IOpenAiService openAiService)
    {
        _openAiService = openAiService;
        _resultsManager = resultsManager;
        _teamsService = teamsService;
        _turnsLoop = _teamsService.TurnsLoop;

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
        GameRestarted?.Invoke();
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

    public async UniTask MakeAiTurn()
    {
        var prompt = BuildBoardPromptFromField(Field);

        Debug.LogWarning(prompt);

        Vector2Int? resultMove = null;

        for (int i = 0; i < _openAiService.ReTriesCount; i++)
        {
            resultMove = await _openAiService.GetMoveAsync(prompt);
        
            if (resultMove == null)
                throw new Exception("AI move is null");

            if (Field[resultMove.Value.x, resultMove.Value.y].IsEmpty)
                break;
            else
                prompt += $" Клітинка ({resultMove.Value.x}, {resultMove.Value.y}) занята, не обирай її.";
        }
        
        if (!Field[resultMove!.Value.x, resultMove.Value!.y].IsEmpty) 
            throw new Exception($"AI move is bad! ({resultMove.Value.x}, {resultMove.Value.y})");
        
        MakeTurn(resultMove.Value.x, resultMove.Value.y);
    }

    [Button]
    private void SetCellState(int x, int y, string teamId, bool isWinning)
    {
        Field[x, y].TeamId = teamId;
        Field[x, y].IsWinning = isWinning;
        
        CellUpdated?.Invoke(x, y);
    }
    
    public string BuildBoardPromptFromField(FieldCell[,] field)
    {
        var sb = new StringBuilder();

        sb.AppendLine("Ось поточне поле (4x4), де X — твій хід, O — мій. Порожні клітинки — \".\":");

        var availableMoves = new HashSet<Vector2Int>();

        for (int row = 0; row < 4; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                var cell = field[row, col];

                char symbol;
                if (cell.IsEmpty)
                {
                    symbol = '.';
                    availableMoves.Add(new Vector2Int { x = row, y = col });
                }
                else if (cell.TeamId == _firstPlayerTeam)
                {
                    symbol = 'X';
                }
                else
                {
                    symbol = 'O';
                }

                sb.Append(symbol);

                if (col < 3) sb.Append(" ");
            }
            sb.AppendLine();
        }

        sb.AppendLine();
        sb.AppendLine($"Я граю за X. Зроби наступний хід, максимально намагайся виграти, обери лише з доступних варіантів: {ToString(availableMoves)}. " +
                      $"Відповідай у форматі function call (row, column).");

        return sb.ToString();
    }

    private string ToString(HashSet<Vector2Int> availableMoves)
    {
        return string.Join(", ", availableMoves.Select(x => $"({x.x}, {x.y})"));
    }

#if UNITY_EDITOR
    private List<string> TeamsIds => TeamsConfig.TeamsIds;
#endif
}