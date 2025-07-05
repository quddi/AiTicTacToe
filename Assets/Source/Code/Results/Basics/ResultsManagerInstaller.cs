using System;
using VContainer;
using VContainer.Unity;

[Serializable]
public class ResultsManagerInstaller : IInstaller
{
    public void Install(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<ResultsManager>()
            .As<IResultsManager>();
    }
}