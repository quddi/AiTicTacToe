using UnityEngine;
using UnityEngine.Internal;

[CreateAssetMenu(menuName = "Configs/Open Ai Config", fileName = "Open Ai config")]
public class OpenAiConfig : ScriptableObject
{
    [field: SerializeField] public string ApiKey { get; private set; }
    
    [field: SerializeField] public string ApiUrl { get; private set; }
    
    [field: SerializeField] public string Model { get; private set; }
    
    [field: SerializeField] public string ToolChoice { get; private set; }
    
    [field: SerializeField] public int ReTriesCount { get; private set; }
}