using System;
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
    
    private ITeamsManager _teamsManager;
    private FieldCell _cell;

    public event Action<int, int> Clicked; 

    [Inject]
    private void Construct(ITeamsManager teamsManager)
    {
        _teamsManager = teamsManager;
    }
    
    public void UpdateState(FieldCell cell)
    {
        _cell = cell;
        _icon.gameObject.SetActive(!cell.IsEmpty);

        if (cell.IsEmpty) _background.color = _emptyColor;
        else _background.color = cell.IsWinning ? _winningColor : _filledColor;
        
        if (cell.IsEmpty) return;
        
        var data = _teamsManager.GetTeamData(cell.TeamId);
        
        _icon.sprite = data.Icon;
    }

    private void ClickedHandler()
    {
        Clicked?.Invoke(_cell.X, _cell.Y);
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