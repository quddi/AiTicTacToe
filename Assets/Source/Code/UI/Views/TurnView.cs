using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class TurnView : MonoBehaviour
{
    [SerializeField] private Color _defaultColor;
    [SerializeField] private Color _winningColor;
    [SerializeField] private Color _drawColor;
    [SerializeField] private Sprite _drawIcon;
    [SerializeField] private GameplayController _gameplayController;
    [SerializeField] private Image _teamIcon;
    [SerializeField] private Image _background;
    [SerializeField] private TMP_Text _title;
    
    private ITeamsManager _teamsManager;

    [Inject]
    private void Construct(ITeamsManager teamsManager)
    {
        _teamsManager = teamsManager;
    }

    private void Start()
    {
        UpdateTeamIcon();
        UpdateBackgroundColor();
        UpdateTitle();
    }

    private void UpdateTeamIcon()
    {
        if (_gameplayController.GameResult == GameResult.Draw)
        {
            _teamIcon.sprite = _drawIcon;
            return;
        }
        
        var data = _teamsManager.GetTeamData(_gameplayController.CurrentTurnTeam);
        
        _teamIcon.sprite = data.Icon;
    }

    private void UpdateBackgroundColor()
    {
        var color = _gameplayController.GameResult switch
        {
            GameResult.None => _defaultColor,
            GameResult.Win => _winningColor,
            GameResult.Draw => _drawColor,
            _ => _defaultColor
        };

        _background.color = color;
    }

    private void UpdateTitle()
    {
        _title.text = _gameplayController.GameResult == GameResult.None ? "Turn" : "Result";
    }
    
    private void TurnPassedHandler()
    {
        UpdateTeamIcon();
        UpdateTitle();
    }

    private void FieldUpdatedHandler()
    {
        UpdateBackgroundColor();
        UpdateTeamIcon();
        UpdateTitle();
    }

    private void OnEnable()
    {
        _gameplayController.TurnPassed += TurnPassedHandler;
        _gameplayController.FieldUpdated += FieldUpdatedHandler;
    }

    private void OnDisable()
    {
        _gameplayController.TurnPassed -= TurnPassedHandler;
        _gameplayController.FieldUpdated -= FieldUpdatedHandler;
    }
}