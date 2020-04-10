using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TSVToCustomObject
{
    const string FILENAME = "Assets\\Data\\Dialogues.tsv";

    /// <summary>
    /// Gets a dictionary of dialogue from file.
    /// </summary>
    public static Dictionary<string, Dialogue> GetDialogueFromTSV()
    {
        string[] raw = ReadWriteFile.LoadLinesFromFile(FILENAME);
        return GetDialogue(raw);
    }

    static Dictionary<string, Dialogue> GetDialogue(string[] raw)
    {
        Dictionary<string, Dialogue> dialogues = new Dictionary<string, Dialogue>();
        Dialogue d = new Dialogue();
        string[] split;
        string lastID = "";
        for (int i = 1; i < raw.Length; i++)
        {
            split = raw[i].Split('\t');
            if (split.Length > 0)
            {
                if (lastID == "")
                    lastID = split[0];
                else if (split[0] != "" && lastID != split[0])
                {
                    d.ID = lastID;
                    dialogues.Add(lastID, d);
                    lastID = split[0];
                    d = new Dialogue();
                }
                d.lines.Add(new Dialogue.Line(split));
            }
        }
        d.ID = lastID;
        dialogues.Add(lastID, d);
        return dialogues;
    }
        
}
