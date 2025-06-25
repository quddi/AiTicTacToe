using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class TeamView : MonoBehaviour
{
    [SerializeField] private Image _teamIcon;
    [SerializeField] private TMP_Text _teamNick;
    
    private ITeamsManager _teamsManager;

    [Inject]
    private void Construct(ITeamsManager teamsManager)
    {
        _teamsManager = teamsManager;
    }
    
    public void SetTeam(string teamId)
    {
        var data = _teamsManager.GetTeamData(teamId);
        
        _teamIcon.sprite = data.SmallIcon;
        _teamNick.text = data.Nick;
    }
}