using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
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
            
        SceneManager.LoadScene(sceneName, new LoadSceneParameters(LoadSceneMode.Single));
            
        while (SceneManager.loadedSceneCount == startScenesCount)
        {
            await UniTask.Yield();
        }
            
        return true;
    }

    public async UniTask LoadGameScene()
    {
        await TryLoad(_loadingConfig.GameSceneName);
    }
}