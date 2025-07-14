using System.Text;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using TriInspector;
using UnityEngine;
using VContainer;

public class Test : MonoBehaviour
{
    [SerializeField] private string _string;
    [SerializeField] private GameplayController _gameplayController;
    [SerializeField] private TeamsController _teamsController;
    [SerializeReference] private ChatResponse _chatResponse;
    
    private IOpenAiService _openAiService;

    private string FirstPlayerId => _teamsController.LeftTeamId;

    [Inject]
    private void Construct(IOpenAiService openAiService)
    {
        _openAiService = openAiService;
    }

    [Button]
    private async UniTaskVoid Send()
    {
        var field = await _openAiService.GetMoveAsync(BuildBoardPromptFromField(_gameplayController.Field));

        Debug.Log(field);
    }
    
    [Button]
    private void DebugBoard()
    {
        Debug.Log(BuildBoardPromptFromField(_gameplayController.Field));
    }
    
    public string BuildBoardPromptFromField(FieldCell[,] field)
    {
        var sb = new StringBuilder();

        sb.AppendLine("Ось поточне поле (4x4), де X — твій хід, O — мій. Порожні клітинки — \".\":");

        for (int row = 0; row < 4; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                var cell = field[row, col];

                char symbol;
                if (cell.IsEmpty)
                {
                    symbol = '.';
                }
                else if (cell.TeamId == FirstPlayerId)
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
        sb.AppendLine("Я граю за X. Зроби наступний хід. Відповідай у форматі function call (row, column).");

        return sb.ToString();
    }

    [Button]
    private void DeserializeResponse()
    {
        _chatResponse = JsonConvert.DeserializeObject<ChatResponse>(_string);
    }
}