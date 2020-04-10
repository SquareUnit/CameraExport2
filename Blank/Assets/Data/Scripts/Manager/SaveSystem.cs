using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static string path;
    public static bool isPath;

    public static void GenerateDefaultSaveFile()
    {
        GetSavePath();
        if (!File.Exists(path))
        {
            BinaryFormatter bFormatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Create);

            SaveData data = new SaveData();

            bFormatter.Serialize(stream, data);
            isPath = true;
            stream.Close();
        }
        else
            isPath = false;
    }

    public static void DeleteSaveFile()
    {
        GetSavePath();
        File.Delete(path);
        GenerateDefaultSaveFile();
    }

    public static void SaveGame(LevelManager levelManager, GameManager gameManager)
    {
        BinaryFormatter bFormatter = new BinaryFormatter();
        GetSavePath();
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData data = new SaveData(levelManager, gameManager);

        bFormatter.Serialize(stream, data);
        stream.Close();
    }

    public static SaveData LoadGame()
    {
        GetSavePath();
        if (File.Exists(path))
        {
            BinaryFormatter bFormatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            SaveData data = bFormatter.Deserialize(stream) as SaveData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void GetSavePath()
    {
        path = Application.persistentDataPath + "/player.kek";
    }
}
