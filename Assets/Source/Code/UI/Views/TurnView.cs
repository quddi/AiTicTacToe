using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class TurnView : MonoBehaviour
{
    [SerializeField] private GameplayController _gameplayController;
    [SerializeField] private Image _teamIcon;
    
    private ITeamsManager _teamsManager;

    [Inject]
    private void Construct(ITeamsManager teamsManager)
    {
        _teamsManager = teamsManager;
    }

    private void Start()
    {
        UpdateTeamIcon();
    }

    private void UpdateTeamIcon()
    {
        var data = _teamsManager.GetTeamData(_gameplayController.CurrentTurnTeam);
        
        _teamIcon.sprite = data.Icon;
    }
    
    private void TurnPassedHandler()
    {
        UpdateTeamIcon();
    }

    private void OnEnable()
    {
        _gameplayController.TurnPassed += TurnPassedHandler;
    }

    private void OnDisable()
    {
        _gameplayController.TurnPassed -= TurnPassedHandler;
    }
}