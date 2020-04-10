using System.Text;
using UnityEngine;
using static PoolingManager;

#if UNITY_EDITOR
public static class PoolHelper {

    const string constFileName = "PoolConst.cs";
    const string poolFileNAme = "PoolingManager.cs";

    public static void GenerateConstFile(PoolObject[] poolArray)
    {
        string path = GetPath();
        string content = GetFileContent(poolArray);
        WriteToFile(path, content);
    }

    static void WriteToFile(string path, string content)
    {
        using (System.IO.FileStream fs = System.IO.File.Create(path))
        {
            char[] fileContentByte = content.ToCharArray();
            for (int i = 0; i < fileContentByte.Length; i++)
            {
                fs.WriteByte((byte)fileContentByte[i]);
            }
            fs.Close();
        }
        UnityEditor.AssetDatabase.Refresh();
    }

    static string GetFileContent(PoolObject[] poolArray)
    {
        StringBuilder fileContent = new StringBuilder();
        fileContent.Append("public static class PoolConst \n{ \n");
        for (int i = 0; i < poolArray.Length; i++)
        {
            fileContent.Append("        "); //for indentation
            fileContent.Append("public const string ");
            fileContent.Append(poolArray[i].name);
            fileContent.Append(" = \"" + poolArray[i].name + "\"; \n"); // = poolObject;
        }
        fileContent.Append("}");

        return fileContent.ToString();
    }

    static string GetPath()
    {
        string[] paths = System.IO.Directory.GetFiles(Application.dataPath, poolFileNAme, System.IO.SearchOption.AllDirectories);
        string path = paths[0].Replace(poolFileNAme, "").Replace("\\", "/");
        return path + constFileName;
    }

}
#endif
