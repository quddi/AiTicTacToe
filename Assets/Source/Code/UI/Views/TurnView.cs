using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using Dropdown = TriInspector.DropdownAttribute;

public class TurnView : MonoBehaviour
{
#if UNITY_EDITOR
    [Dropdown(nameof(TeamsIds))]
#endif
    [SerializeField] private string _rightTeamId;
    [SerializeField] private Color _defaultColor;
    [SerializeField] private Color _winningColor;
    [SerializeField] private Color _drawColor;
    [SerializeField] private Sprite _drawIcon;
    [SerializeField] private Image _teamIcon;
    [SerializeField] private Image _background;
    [SerializeField] private TMP_Text _title;
    [SerializeField] private Button _restartButton;
    [SerializeField] private WaitingAnimationView _waitingAnimation;
    [SerializeField] private AsyncAnimation _appearAnimation;
    [SerializeField] private AsyncAnimation _disappearAnimation;
    
    [SerializeField] private GameplayController _gameplayController;
    
    private ITeamsService _teamsService;
    private CancellationTokenSource _tokenSource;

    [Inject]
    private void Construct(ITeamsService teamsService)
    {
        _teamsService = teamsService;
    }

    private void Start()
    {
        UpdateTeamIcon().Forget();
        UpdateBackgroundColor();
        UpdateTitle();
        UpdateWaiting();
    }

    private async UniTaskVoid UpdateTeamIcon()
    {
        ValidateToken();
        
        await _disappearAnimation.Execute(_tokenSource.Token);
        
        if (_gameplayController.GameResult == GameResult.Draw)
        {
            _teamIcon.sprite = _drawIcon;
            
            await _appearAnimation.Execute(_tokenSource.Token);
            
            return;
        }
        
        var data = _teamsService.GetTeamData(_gameplayController.CurrentTurnTeam);
        
        _teamIcon.sprite = data.Icon;
        
        await _appearAnimation.Execute(_tokenSource.Token);
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

    private void UpdateWaiting()
    {
        var gameEnded = _gameplayController.GameResult != GameResult.None;

        var active = !gameEnded && _gameplayController.CurrentTurnTeam == _rightTeamId;
        
        if (active) _waitingAnimation.Appear();
        else _waitingAnimation.Disappear();
        
        _restartButton.gameObject.SetActive(gameEnded);
    }

    private void ValidateToken()
    {
        _tokenSource?.Cancel();
        _tokenSource?.Dispose();
        
        _tokenSource = new();
    }

    private void TurnPassedHandler()
    {
        UpdateTeamIcon().Forget();
        UpdateTitle();
        UpdateWaiting();
    }

    private void FieldUpdatedHandler()
    {
        UpdateBackgroundColor();
        UpdateTeamIcon().Forget();
        UpdateTitle();
        UpdateWaiting();
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
    
#if UNITY_EDITOR
    private IEnumerable<string> TeamsIds => TeamsConfig.TeamsIds;
#endif
}