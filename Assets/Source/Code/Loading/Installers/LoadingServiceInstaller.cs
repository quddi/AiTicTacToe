using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

[Serializable]
public class LoadingServiceInstaller : IInstaller
{
    [SerializeField] private LoadingConfig _loadingConfig;

    public void Install(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<LoadingService>()
            .AsImplementedInterfaces()
            .WithParameter("loadingConfig", _loadingConfig);
    }
}