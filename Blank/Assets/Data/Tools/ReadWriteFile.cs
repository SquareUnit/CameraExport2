using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Reads and writes files in string format.
/// 
/// Keep it clean and simple!
/// Created by Maxime Phaneuf
/// April 2019
/// </summary>
public static class ReadWriteFile
{
    /// <summary>
    /// Writes a file with specified content at specified directory, name and extension.
    /// </summary>
    public static void WriteToFile(string directory, string fileName, string ext, string content)
    {
        string path = GetPath(directory, fileName, ext);
        FileStream fs = File.Create(path);
        char[] fileContentByte = content.ToCharArray();
        for (int i = 0; i < fileContentByte.Length; i++)
            fs.WriteByte((byte)fileContentByte[i]);
        fs.Close();
    }

    /// <summary>
    /// Loads the string representation of a file with specified name.
    /// </summary>
    public static string LoadFromFile(string fileName)
    {
        string path = GetPath(fileName);
        if (path == null || path == "")
            return "";
        string[] json = File.ReadAllLines(path);
        string output = "";
        foreach (string s in json)
            output += s;
        return output;
    }

    public static string[] LoadLinesFromFile(string fileName)
    {
        string path = GetPath(fileName);
        return File.ReadAllLines(path);
    }

    static string GetPath(string directory, string fileName, string ext)
    {
        string path = Application.dataPath + "/" + directory + "/";
        return path + fileName + ext;
    }

    static string GetPath(string fileName)
    {
        string[] paths = Directory.GetFiles(Application.dataPath, fileName, SearchOption.AllDirectories);
        string path = "";
        if (paths.Length > 0)
            path = paths[0].Replace("\\", "/");
        return path;
    }
}
