using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

public class FieldCellView : MonoBehaviour
{
    [SerializeField] private Color _filledColor;
    [SerializeField] private Color _emptyColor;
    [SerializeField] private Color _winningColor;
    [SerializeField] private Image _icon;
    [SerializeField] private Image _background;
    [SerializeField] private Button _button;
    [SerializeField] private ImageColorLerpAsyncAnimation _colorAnimation;
    [SerializeField] private AbsoluteScaleAsyncAnimation _scaleAnimation;
    
    private ITeamsService _teamsService;
    private FieldCell _cell;
    private CancellationTokenSource _tokenSource;
    private bool _wasEmpty;

    public event Action<int, int> Clicked; 

    [Inject]
    private void Construct(ITeamsService teamsService)
    {
        _teamsService = teamsService;
    }
    
    public void UpdateState(FieldCell cell)
    {
        _cell = cell;

        _icon.gameObject.SetActive(!cell.IsEmpty);

        PlayMarkAnimation(cell.IsEmpty ? _emptyColor : cell.IsWinning ? _winningColor : _filledColor);

        if (cell.IsEmpty)
        {
            foreach (var scaledTransform in _scaleAnimation.ScaledTransforms)
            {
                scaledTransform.localScale = Vector3.zero;
            }

            _wasEmpty = true;
            
            return;
        }
        
        var data = _teamsService.GetTeamData(cell.TeamId);
        
        _icon.sprite = data.Icon;

        if (_wasEmpty) _scaleAnimation.Execute(_tokenSource.Token).Forget();

        _wasEmpty = false;
    }
    
    private void PlayMarkAnimation(Color resultColor)
    {
        ValidateToken();

        _colorAnimation.StartColor = _background.color;
        _colorAnimation.EndColor = resultColor;
        
        _colorAnimation.OrNull()?.Execute(_tokenSource.Token).Forget();
    }

    private void ValidateToken()
    {
        _tokenSource?.Cancel();
        _tokenSource?.Dispose();

        _tokenSource = new();
    }
    
    private void ClickedHandler()
    {
        Clicked?.Invoke(_cell.X, _cell.Y );
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(ClickedHandler);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(ClickedHandler);
    }
}