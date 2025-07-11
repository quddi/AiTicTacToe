using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class TeamView : MonoBehaviour
{
    [SerializeField] private Image _teamIcon;
    [SerializeField] private TMP_Text _teamNick;
    [SerializeField] private TMP_Text _winsText;
    
    private ITeamsService _teamsService;
    private IResultsManager _resultsManager;
    private string _teamId;

    [Inject]
    private void Construct(ITeamsService teamsService, IResultsManager resultsManager)
    {
        _resultsManager = resultsManager;
        _teamsService = teamsService;
        
        UpdateWins();
        
        _resultsManager.ResultAdded += GameResultAddedHandler;
    }

    public void SetTeam(string teamId)
    {
        _teamId = teamId;
        
        var data = _teamsService.GetTeamData(_teamId);
        
        _teamIcon.sprite = data.SmallIcon;
        _teamNick.text = data.Nick;
    }

    private void GameResultAddedHandler(GameResultInfo resultInfo)
    {
        UpdateWins();
    }

    private void UpdateWins()
    {
        var wins = _resultsManager.Results.Count(result => result.WinnerId == _teamId && result.Result == GameResult.Win);

        _winsText.text = $"{wins} wins";
    }

    private void OnDestroy()
    {
        if (_resultsManager != null) _resultsManager.ResultAdded -= GameResultAddedHandler;
    }
}