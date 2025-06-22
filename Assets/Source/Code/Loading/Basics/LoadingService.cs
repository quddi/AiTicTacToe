using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using VContainer;

public class LoadingService : ILoadingService
{
    private LoadingConfig _loadingConfig;

    private string _currentSceneName;

    [Inject]
    private void Construct(LoadingConfig loadingConfig)
    {
        _loadingConfig = loadingConfig;
    }

    public async UniTask<bool> TryLoad(string sceneName)
    {
        if (!string.IsNullOrEmpty(_currentSceneName))
            await SceneManager.UnloadSceneAsync(_currentSceneName);

        var startScenesCount = SceneManager.sceneCount;
            
        var scene = SceneManager.LoadScene(sceneName, new LoadSceneParameters(LoadSceneMode.Additive));
            
        while (SceneManager.loadedSceneCount == startScenesCount)
        {
            await UniTask.Yield();
        }

        SceneManager.SetActiveScene(scene);
            
        return true;
    }

    public async UniTask LoadGameScene()
    {
        await TryLoad(_loadingConfig.GameSceneName);
    }
}