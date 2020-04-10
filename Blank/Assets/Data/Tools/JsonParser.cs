using UnityEngine;
using UnityEditor;
using System.IO;

/// <summary>
/// Use this to parse any object from and to .json format.
///
/// Keep it clean and simple!
/// Created by Maxime Phaneuf
/// April 2019
/// </summary>
public static class JsonParser
{
    /// <summary>
    /// Load object from json into specified object with same parameters and attributes .
    /// </summary>
    public static void LoadJsonToObject(string json, System.Object obj)
    {
        JsonUtility.FromJsonOverwrite(json, obj);
    }

    /// <summary>
    /// Create a new json string with parameters and attributes of specified object.
    /// </summary>
    public static string CreateNewJson(System.Object content)
    {
        return JsonUtility.ToJson(content);
    }

}