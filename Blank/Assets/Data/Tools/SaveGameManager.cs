/// <summary>
/// Module that loads and saves binary files with specified objects.
///
/// Keep it clean and simple!
/// Created by Maxime Phaneuf
/// April 2019
/// </summary>
public static class SaveGameManager
{
    const string EXTENSION = ".sg";
    const string DIRECTORY = "Data/SaveGame";

    /// <summary>
    /// Saves an object with all its parameter and attributes to binary file.
    /// </summary>
    public static void Save(string name, System.Object obj)
    {
        if (name == "" || name == null)
            return;
        string json = JsonParser.CreateNewJson(obj);
        string binary = BinaryParser.StringToBinary(json);
        ReadWriteFile.WriteToFile(DIRECTORY, name, EXTENSION, binary);
    }

    /// <summary>
    /// Loads an object's parameter and attributes from a binary file.
    /// </summary>
    public static void Load(string fileName, System.Object obj)
    {
        if (fileName == "" || fileName == null)
            return;
        string binary = ReadWriteFile.LoadFromFile(fileName + EXTENSION);
        string json = BinaryParser.BinaryToString(binary);
        JsonParser.LoadJsonToObject(json, obj);

    }
}
