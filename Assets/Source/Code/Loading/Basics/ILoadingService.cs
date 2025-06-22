using Cysharp.Threading.Tasks;

public interface ILoadingService : IService
{
    public UniTask<bool> TryLoad(string sceneName);

    public UniTask LoadGameScene();
}