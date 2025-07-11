using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

[Serializable]
public class TeamsServiceInstaller : IInstaller
{
    [SerializeField] private TeamsConfig _teamsConfig;
    
    public void Install(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<TeamsService>()
            .As<ITeamsService>()
            .WithParameter("teamsConfig", _teamsConfig);
    }
}