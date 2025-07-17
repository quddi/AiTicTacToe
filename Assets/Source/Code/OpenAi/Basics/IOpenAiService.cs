using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IOpenAiService
{
    int ReTriesCount { get; }
    
    UniTask<Vector2Int?> GetMoveAsync(string userInput);
}