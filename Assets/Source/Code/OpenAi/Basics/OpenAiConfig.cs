using UnityEngine;
using UnityEngine.Internal;

[CreateAssetMenu(menuName = "Configs/Open Ai Config", fileName = "Open Ai config")]
public class OpenAiConfig : ScriptableObject
{
    [field: SerializeField] public string ApiKey { get; private set; }
}