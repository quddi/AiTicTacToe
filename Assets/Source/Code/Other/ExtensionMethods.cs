using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

public static class ExtensionMethods
{
    public static T OrNull<T>(this T obj) where T : Object
    {
        return obj == null ? null : obj;
    }
    
#if UNITY_EDITOR
    public static IEnumerable<T> GetAllScriptableObjects<T>() where T : ScriptableObject
    {
        string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);
            
        foreach (var guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
                
            yield return AssetDatabase.LoadAssetAtPath<T>(path);
        }
    }
#endif
}