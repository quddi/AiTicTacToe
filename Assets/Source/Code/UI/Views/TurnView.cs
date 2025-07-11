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
    [SerializeField] private Image _teamIcon;
    [SerializeField] private Image _background;
    [SerializeField] private TMP_Text _title;
    [SerializeField] private Button _restartButton;
    [SerializeField] private GameObject _waitingAnimation;
    
    [SerializeField] private GameplayController _gameplayController;
    
    private ITeamsService _teamsService;

    [Inject]
    private void Construct(ITeamsService teamsService)
    {
        _teamsService = teamsService;
    }

    private void Start()
    {
        UpdateTeamIcon();
        UpdateBackgroundColor();
        UpdateTitle();
        UpdateWaiting();
    }

    private void UpdateTeamIcon()
    {
        if (_gameplayController.GameResult == GameResult.Draw)
        {
            _teamIcon.sprite = _drawIcon;
            return;
        }
        
        var data = _teamsService.GetTeamData(_gameplayController.CurrentTurnTeam);
        
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
        UpdateWaiting();
    }

    private void UpdateWaiting()
    {
        var gameEnded = _gameplayController.GameResult != GameResult.None;
        
        _waitingAnimation.SetActive(!gameEnded);
        _restartButton.gameObject.SetActive(gameEnded);
    }

    private void RestartButtonClickedHandler()
    {
        _gameplayController.Restart();
    }

    private void OnEnable()
    {
        _gameplayController.TurnPassed += TurnPassedHandler;
        _gameplayController.FieldUpdated += FieldUpdatedHandler;
        _restartButton.onClick.AddListener(RestartButtonClickedHandler);
    }

    private void OnDisable()
    {
        _gameplayController.TurnPassed -= TurnPassedHandler;
        _gameplayController.FieldUpdated -= FieldUpdatedHandler;
        _restartButton.onClick.RemoveListener(RestartButtonClickedHandler);
    }
}