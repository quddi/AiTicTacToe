using System.Collections.Generic;
using TriInspector;
using UnityEngine;

public class TeamsController : MonoBehaviour
{
#if UNITY_EDITOR
    [field: Dropdown(nameof(TeamIds))]
#endif
    [field: SerializeField] public string LeftTeamId { get; private set; }
    
#if UNITY_EDITOR
    [field: Dropdown(nameof(TeamIds))]
#endif
    [field: SerializeField] public string RightTeamId { get; private set; }

    [SerializeField] private TeamView _leftTeamView;
    [SerializeField] private TeamView _rightTeamView;

    private void Start()
    {
        _leftTeamView.SetTeam(LeftTeamId);
        _rightTeamView.SetTeam(RightTeamId);
    }

#if UNITY_EDITOR
    private List<string> TeamIds => TeamsConfig.TeamsIds;
#endif
}