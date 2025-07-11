using System.Collections.Generic;

public interface ITeamsService
{
    List<string> TurnsLoop { get; }
    
    TeamData GetTeamData(string teamId);
}