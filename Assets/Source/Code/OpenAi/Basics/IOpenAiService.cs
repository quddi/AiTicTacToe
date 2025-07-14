using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IOpenAiService
{
    UniTask<Vector2Int?> GetMoveAsync(string userInput);
}