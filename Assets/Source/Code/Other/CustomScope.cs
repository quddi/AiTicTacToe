using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class CustomScope : LifetimeScope
{
    [SerializeReference] private List<IInstaller> _installers = new();
    
    protected override void Configure(IContainerBuilder builder)
    {
        base.Configure(builder);
        
        foreach (var installer in _installers)
        {
            installer.Install(builder);
        }
    }
}