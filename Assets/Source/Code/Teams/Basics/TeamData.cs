using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Datas/Team Data", fileName = "Team data")]
public class TeamData : ScriptableObject
{
    [field: SerializeField] public string Id { get; private set; }
    
    [field: SerializeField] public string Nick { get; private set; }

    [field: SerializeField] public Sprite Icon { get; private set; }
    
    [field: SerializeField] public Sprite SmallIcon { get; private set; }
}