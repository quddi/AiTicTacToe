using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Teams Config", fileName = "Teams config")]
public class TeamsConfig : ScriptableObject
{
    [field: SerializeField] public List<TeamData> TeamDatas { get; private set; } = new();
}