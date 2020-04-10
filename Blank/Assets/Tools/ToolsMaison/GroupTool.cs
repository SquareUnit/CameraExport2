#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;

public class GroupTool : EditorWindow {

	[MenuItem("Tools/Group %g")]
	private static void Group(){
		Transform[] selected = Selection.GetTransforms(SelectionMode.TopLevel);
		GameObject myParent = new GameObject();
		myParent.name = "Group(" + selected.Length +")";
		foreach(Transform selection in selected){
			selection.SetParent(myParent.transform);
		}
	}

	[MenuItem("Tools/Ungroup %u")]
	private static void Ungroup(){
		//GameObject selected = Selection.activeObject as GameObject;
		Transform[] selection = Selection.GetTransforms(SelectionMode.TopLevel);

		foreach(Transform selected in selection ){	
			if(selected.name.Contains("Group"))
			{
				selected.DetachChildren();
				DestroyImmediate(selected.gameObject);
			}
			else selected.SetParent(null);
		}
	}

	/*static void Init(){
		GroupTool window = (GroupTool)EditorWindow.GetWindow(typeof(GroupTool));
		window.Show ();
	}*/

	/*void OnGUI(){
		if(GUILayout.Button("Grouper")){
			Transform[] selected = Selection.GetTransforms(SelectionMode.TopLevel);
			GameObject myParent = new GameObject();
			myParent.name = "Group(" + selected.Length +")";
			foreach(Transform selection in selected){
				selection.SetParent(myParent.transform);
			}
		}

		if(GUILayout.Button("Degrouper")){
			GameObject selected = Selection.activeObject as GameObject;
			selected.transform.DetachChildren();
			DestroyImmediate(selected.gameObject);
		}

	}*/


}
#endif
