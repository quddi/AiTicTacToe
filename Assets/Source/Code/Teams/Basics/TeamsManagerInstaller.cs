using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

[Serializable]
public class TeamsManagerInstaller : IInstaller
{
    [SerializeField] private TeamsConfig _teamsConfig;
    
    public void Install(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<TeamsManager>()
            .As<ITeamsManager>()
            .WithParameter("teamsConfig", _teamsConfig);
    }
}