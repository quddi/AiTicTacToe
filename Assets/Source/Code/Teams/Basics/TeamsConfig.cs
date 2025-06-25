using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Teams Config", fileName = "Teams config")]
public class TeamsConfig : ScriptableObject
{
    [field: SerializeField] public List<TeamData> TeamDatas { get; private set; } = new();

#if UNITY_EDITOR
    public static List<string> TeamsIds => ExtensionMethods.GetAllScriptableObjects<TeamsConfig>()
        .First().TeamDatas
        .Select(data => data.Id)
        .ToList();
#endif
}