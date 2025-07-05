using UnityEngine;
using UnityEngine.UIElements;
using VContainer;
using Image = UnityEngine.UI.Image;

public class FieldCellView : MonoBehaviour
{
    [SerializeField] private Image _icon;
    
    private ITeamsManager _teamsManager;

    [Inject]
    private void Construct(ITeamsManager teamsManager)
    {
        _teamsManager = teamsManager;
    }
    
    public void SetTeamIcon(string teamId)
    {
        var isEmpty = string.IsNullOrEmpty(teamId);
        
        _icon.gameObject.SetActive(!isEmpty);
        
        if (isEmpty) return;
        
        var data = _teamsManager.GetTeamData(teamId);
        
        _icon.sprite = data.SmallIcon;
    }
}