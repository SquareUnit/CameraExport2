using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(MeshRenderer))]
public class SwapMaterialV2 : MonoBehaviour
{
    private int matToReplace;
    private int matHolder;
    private int materialChosen;

    public List<MatToSwap> meshList;


    /// <summary> Find how many materials the renderer has by default. This value is equal to the number of sub-meshes in the object mesh.</summary>
    private void Awake()
    {
        Material[] temp = GetComponent<MeshRenderer>().sharedMaterials;
        for (int i = 0; i < meshList.Count; i++)
        {
            meshList[i].name = "Mats for mesh " + i.ToString(); 
        }
    }

    /// <summary> Data container for substitution materials</summary>
    [System.Serializable]
    public class MatToSwap
    {
        [Tooltip("Identifier for the inspector. No influences on values.")] public string name;
        [Tooltip("The materials that can be assigned to the mesh")] public Material[] materialArray;
    }

    /// <summary> Set a new material to the specified mesh </summary>
    public void SetMatToMesh(int meshIndex, int matIndex)
    {
        Material[] allMeshMats = GetComponent<MeshRenderer>().sharedMaterials;
        allMeshMats[meshIndex] = meshList[meshIndex].materialArray[matIndex];
        GetComponent<MeshRenderer>().sharedMaterials = allMeshMats;
    }
}