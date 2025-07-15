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

    /*[Button]
    private async UniTaskVoid Send()
    {
        var field = await _openAiService.GetMoveAsync(BuildBoardPromptFromField(_gameplayController.Field));

        Debug.Log(field);
    }
    
    [Button]
    private void DebugBoard()
    {
        Debug.Log(BuildBoardPromptFromField(_gameplayController.Field));
    }*/
    
    

    [Button]
    private void DeserializeResponse()
    {
        _chatResponse = JsonConvert.DeserializeObject<ChatResponse>(_string);
    }
}