using UnityEngine;
using VContainer;

public class Bootstraper : MonoBehaviour
{
    [Inject]
    private void Construct(ILoadingService loadingService)
    {
        loadingService.LoadGameScene();
    }
}