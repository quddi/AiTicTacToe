using VContainer;

public class OpenAiService : IOpenAiService
{
    private OpenAiConfig _config;

    [Inject]
    private void Construct(OpenAiConfig openAiConfig)
    {
        _config = openAiConfig;
    }
}