using System.Collections.Generic;
using TriInspector;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Loading Config", fileName = "Loading config")]
public class LoadingConfig : ScriptableObject
{
#if UNITY_EDITOR
    [field: Dropdown(nameof(GetAllSceneNames)), Tooltip("Fill scene names first!")]
#endif
    [field: SerializeField] public string GameSceneName { get; set; }

#if UNITY_EDITOR
    public static List<string> GetAllSceneNames()
    {
        var sceneNames = new List<string>();

        var scenes = EditorBuildSettings.scenes;

        foreach (var scene in scenes)
        {
            if (scene == null) continue;

            var path = scene.path;

            var sceneName = System.IO.Path.GetFileNameWithoutExtension(path);

            if (!string.IsNullOrEmpty(sceneName))
            {
                sceneNames.Add(sceneName);
            }
        }

        return sceneNames;
    }
#endif
}