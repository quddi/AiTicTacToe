using System.Text;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using VContainer;

public class OpenAiService : IOpenAiService
{
    private OpenAiConfig _config;

    [Inject]
    private void Construct(OpenAiConfig openAiConfig)
    {
        _config = openAiConfig;
    }
    
    public async UniTask<Vector2Int?> GetMoveAsync(string userInput)
    {
        var messages = new[]
        {
            new Message("system", "Ти граєш у хрестики-нулики 4x4. Твоя задача — обрати наступний хід."),
            new Message("user", userInput)
        };

        var tool = new Tool
        {
            function = new ToolFunction
            {
                name = "select_move",
                description = "Отримати координати ходу у грі 4x4",
                parameters = new ToolParameters
                {
                    properties = new ToolProperties()
                }
            }
        };

        var requestData = new ChatRequest(_config.Model, _config.ToolChoice)
        {
            messages = messages,
            tools = new[] { tool },
        };

        var jsonData = JsonConvert.SerializeObject(requestData);
        var request = new UnityWebRequest(_config.ApiUrl, "POST");
        var bodyRaw = Encoding.UTF8.GetBytes(jsonData);

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + _config.ApiKey);
        
        await request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            var resultJson = request.downloadHandler.text;
            Debug.Log(resultJson);
            
            var response = JsonConvert.DeserializeObject<ChatResponse>(resultJson);
            
            if (response.choices is { Length: > 0 })
            {
                var argumentsJson = response.choices[0].message.tool_calls[0].function.arguments;
                var move = JsonConvert.DeserializeObject<MoveData>(argumentsJson);
                
                return new Vector2Int(move.row, move.column);
            }

            Debug.LogError("Tool call не повернув аргументів. Відповідь: " + resultJson);
            return null;
        }
        
        Debug.LogError("OpenAI API error: " + request.error + "\n" + request.downloadHandler.text);
        return null;
    }
}