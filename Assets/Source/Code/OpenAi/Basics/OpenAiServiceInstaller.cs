using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

[Serializable]
public class OpenAiServiceInstaller : IInstaller
{
    [SerializeField] private OpenAiConfig _openAiConfig;
    
    public void Install(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<OpenAiService>()
            .As<IOpenAiService>()
            .WithParameter("openAiConfig", _openAiConfig);
    }
}