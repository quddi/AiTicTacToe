using System;
using System.Threading;
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
    [SerializeField] private AsyncAnimation _markAnimation;
    
    private ITeamsService _teamsService;
    private FieldCell _cell;
    private CancellationTokenSource _tokenSource;

    public event Action<int, int> Clicked; 

    [Inject]
    private void Construct(ITeamsService teamsService)
    {
        _teamsService = teamsService;
    }
    
    public void UpdateState(FieldCell cell)
    {
        _cell = cell;
        var isEmpty = !cell.IsEmpty;
        _icon.gameObject.SetActive(isEmpty);

        if (cell.IsEmpty) _background.color = _emptyColor;
        else _background.color = cell.IsWinning ? _winningColor : _filledColor;
        
        if (cell.IsEmpty) return;
        
        var data = _teamsService.GetTeamData(cell.TeamId);
        
        _icon.sprite = data.Icon;
        
        if (isEmpty) PlayMarkAnimation();
    }
    
    private void PlayMarkAnimation()
    {
        ValidateToken();
        
        _markAnimation.OrNull()?.Execute(_tokenSource.Token);
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