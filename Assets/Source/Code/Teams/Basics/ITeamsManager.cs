using System.Collections.Generic;

public interface ITeamsManager
{
    List<string> TurnsLoop { get; }
    
    TeamData GetTeamData(string teamId);
}