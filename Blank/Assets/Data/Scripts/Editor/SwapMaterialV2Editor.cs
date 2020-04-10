using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SwapMaterialV2))]
public class SwapMaterialV2Editor : Editor
{
    private SwapMaterialV2 editor;
    private GUIStyle myStyle;
    private int fontsize = 13;
    private int meshCount;
    private string[] meshVal;
    private string[] matVal;
    private int mesh;
    private int mat;

    void OnEnable()
    {
        editor = (SwapMaterialV2)target;

        meshCount = editor.GetComponent<MeshRenderer>().sharedMaterials.Length;
        meshVal = new string[editor.meshList.Count];
        int temp = 0;
        for (int i = 0; i < editor.meshList.Count; i++)
        {
            meshVal[i] = "Mesh number " + i.ToString();
            if(editor.meshList[i].materialArray.Length > temp)
            {
                temp = editor.meshList[i].materialArray.Length;
            }
        }

        matVal = new string[temp];
        for (int i = 0; i < temp; i++)
        {
            matVal[i] = "Material number " + i.ToString();
        }
    }

    public override void OnInspectorGUI()
    {
        Repaint();
        // Params
        myStyle = new GUIStyle(GUI.skin.button);
        myStyle.fontSize = fontsize;
        myStyle.normal.textColor = Color.white;
        myStyle.alignment = TextAnchor.MiddleCenter;
        myStyle.stretchWidth = true;
        myStyle.stretchHeight = true;
        // ---------------------------------------------------------------------------
        GUILayout.BeginHorizontal("box");
        GUILayout.Label("Number of meshes on this object : " + meshCount.ToString(), GUILayout.Width(225));
        GUILayout.Space(15);
        if (GUILayout.Button("Add material to list", myStyle))
        {
            editor.meshList.Add(new SwapMaterialV2.MatToSwap());
        }
        GUILayout.EndHorizontal();
        // ---------------------------------------------------------------------------
        GUILayout.Space(5);
        GUILayout.BeginVertical("box");
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Apply mat" + mat.ToString() + " to mesh" + mesh.ToString(), myStyle))
        {
            editor.SetMatToMesh(mesh, mat);
        }
        GUILayout.Space(30);
        mesh = EditorGUILayout.Popup(mesh, meshVal, GUILayout.Width(115));
        GUILayout.Space(5);
        mat = EditorGUILayout.Popup(mat, matVal, GUILayout.Width(115));
        GUILayout.EndHorizontal();
        GUILayout.Space(10);
        GUILayout.EndVertical();
        // ---------------------------------------------------------------------------
        base.OnInspectorGUI();
    }
}