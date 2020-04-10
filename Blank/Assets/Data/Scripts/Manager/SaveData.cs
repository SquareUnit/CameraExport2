using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int currentLevel;
    public float[] levelStartPos;
    public CollectibleInfo[] collectibleInfos;
    public bool[] collectibleDecals;

    /// <summary> Default constructor, all values to 0 or null. This is a blank save file </summary>
    public SaveData()
    {
        currentLevel = 0;
        //levelStartPos = new float[3] { 0.0f, 0.0f, 0.0f };

        //collectibleInfos = new CollectibleInfo[15];
        //for (int i = 0; i < collectibleInfos.Length; i++)
        //{
        //    collectibleInfos[i].collectibleName = "";
        //    collectibleInfos[i].collectibleDescription = "";
        //    collectibleInfos[i].collectibleEnabled = false;
        //}

        //collectibleDecals = new bool[255];
        //for (int i = 0; i < collectibleDecals.Length; i++)
        //{
        //    collectibleDecals[i] = false;
        //}
    }

    public SaveData(LevelManager levelManager, GameManager gameManager)
    {
        //levelStartPos = new float[3];
        //collectibleInfos = new CollectibleInfo[15];
        //collectibleDecals = new bool[20];

        currentLevel = levelManager.currentLvl;
        //levelStartPos[0] = gameManager.checkpointLocation.x;
        //levelStartPos[1] = gameManager.checkpointLocation.y;
        //levelStartPos[2] = gameManager.checkpointLocation.z;

        //for (int i = 0; i < collectibleInfos.Length; i++)
        //{
        //    collectibleInfos[i] = gameManager.collectibleInfos[i];
        //}

        //collectibleDecals = gameManager.collectibleDecals;
    }
}

///// Hints
//// Vector3 are non serializable, use float3 arrays to store positions or rotations