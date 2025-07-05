using System.Collections.Generic;
using System.Linq;
using VContainer;

public class TeamsManager : ITeamsManager
{
    private TeamsConfig _teamsConfig;
    private Dictionary<string, TeamData> _teamDatas;

    public List<string> TurnsLoop => _teamDatas.Keys.ToList();

    [Inject]
    private void Construct(TeamsConfig teamsConfig)
    {
        _teamsConfig = teamsConfig;

        _teamDatas = _teamsConfig.TeamDatas
            .ToDictionary(data => data.Id, data => data);
    }

    public TeamData GetTeamData(string teamId)
    {
        return !string.IsNullOrEmpty(teamId) && _teamDatas.TryGetValue(teamId, out var result)
            ? result
            : null;
    }
}