#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class APlacer{

		public GameObject objetAMettre;



}
public class LevelArtRandomizer : EditorWindow {
	[Range(1,10)]
	public static int size = 1;
	public static bool uniform = true;
	public bool active = false;
	public static GameObject[] modelesVoulus = new GameObject[10];
	public static int[] polyParModeles = new int[10];
	public Vector3 minRandomRotation = Vector3.zero;
	public Vector3 maxRandomRotation = new Vector3 (180f, 180f, 180f);
	public Vector3 minRandomScale = Vector3.one;
	public Vector3 maxRandomScale = Vector3.one;
	private static int nbOfPlacedObjects = 0;
	public GameObject tempGO = null;
	private int totalPolys = 0;

    private Vector3 buPosition;
    private Quaternion buRotation;
    private Vector3 buScale;
    private GameObject buGo;
    private int buPolys;
    private bool isShiftPressed = false;

	[MenuItem("Tools/Modelisation/LevelArtRandomizer")]
	static void Init(){
		LevelArtRandomizer window = (LevelArtRandomizer)EditorWindow.GetWindow(typeof(LevelArtRandomizer));
		window.Show ();

	}
	void OnInspectorUpdate(){
		Repaint();
	}
	void OnEnable()
	{
		SceneView.onSceneGUIDelegate += SceneGUI;
	}
	void OnGUI(){
		//size = EditorGUILayout.IntField ("Grosse de la banque d'objets:", size);
		active = EditorGUILayout.Toggle ("Active", active); 
		if(active && tempGO == null){
			tempGO = new GameObject ("LARandomizer");
			nbOfPlacedObjects = 0;
			EditorUtility.SetDirty (tempGO);
		}
		else {
			if(!active && tempGO!=null){
				if(nbOfPlacedObjects==0){
					DestroyImmediate (tempGO.gameObject);
					totalPolys = 0;
				}
				else{
					tempGO = null;
					totalPolys = 0;
					nbOfPlacedObjects = 0;
				}

			}
		}
		size = (int)EditorGUILayout.Slider ("Nombre d'objets:", size, 1, 10);
		for(int i = 0; i<size;i++){
			modelesVoulus [i] = EditorGUILayout.ObjectField (modelesVoulus [i], typeof(GameObject),false) as GameObject ;
			if(modelesVoulus[i]!=null)	{
				MeshFilter myMesh = modelesVoulus [i].GetComponent<MeshFilter> ();
				if(myMesh == null){
					myMesh = modelesVoulus [i].GetComponentInChildren<MeshFilter> ();
				}
				if (myMesh != null)
					polyParModeles [i] = myMesh.sharedMesh.triangles.Length / 3;
				else
					polyParModeles [i] = 0;
			}

		}
		//objVoulu  = EditorGUILayout.ObjectField (objVoulu , typeof(GameObject),false) as GameObject ;
		EditorGUILayout.Separator();
		EditorGUILayout.LabelField("Rotations");
		minRandomRotation = EditorGUILayout.Vector3Field ("Rotation minimum", minRandomRotation);
		maxRandomRotation = EditorGUILayout.Vector3Field ("Rotation Maximum", maxRandomRotation);
		EditorGUILayout.Separator();
		EditorGUILayout.LabelField("Scale");
		minRandomScale = EditorGUILayout.Vector3Field ("Scale minimum", minRandomScale);
		maxRandomScale = EditorGUILayout.Vector3Field ("Scale Maximum", maxRandomScale);
		uniform = EditorGUILayout.Toggle ("Uniforme?(base sur x)", uniform);
		//nbOfPlacedObjects = EditorGUILayout.IntField ("Nombre d'objets instancies :", nbOfPlacedObjects);
		EditorGUILayout.LabelField("Nombre d'objets instancies : "+ nbOfPlacedObjects);

		/*for(int i = 0 ; i < polyParModeles.Length;i++){
			totalPolys  += polyParModeles [i];
		}*/
		EditorGUILayout.LabelField("Nombre de polygones : "+ totalPolys );
		EditorGUILayout.Separator();
        EditorGUILayout.HelpBox("Instructions :\nPour utiliser, mettre en 'active'\nLeft-Click : Mettre un modèle aléatoire\nShift+Right-Clic: Remplacer le dernier modèle à la même position/rotation/scale ", MessageType.Info, true);


    }		

    void SetBUFromTransform(GameObject go, Vector3 pos, Quaternion rot, Vector3 sca, int nbPoly){
        buGo = go;
        buPosition = pos;
        buRotation = rot;
        buScale = sca;
        buPolys = nbPoly;
        
    }

	void SceneGUI(SceneView sceneView){
		if(active){
            Event e = Event.current;
           
            isShiftPressed = Event.current.modifiers == EventModifiers.Shift;

            if (Event.current.type == EventType.MouseDown && e.button==0 && !isShiftPressed){
				if(tempGO == null){
					active = false;
					totalPolys = 0;
					return;
				}
				Ray worldRay = HandleUtility.GUIPointToWorldRay (Event.current.mousePosition);
				RaycastHit hit;
				if(Physics.Raycast(worldRay,out hit, 10000)){
					int tempRandomInt = Random.Range (0, size);
					GameObject prefabInstance = PrefabUtility.InstantiatePrefab (modelesVoulus[tempRandomInt] ) as GameObject;
					prefabInstance.transform.position = hit.point;
					totalPolys  += polyParModeles[tempRandomInt];
					Quaternion rotationVoulue = Quaternion.Euler (Random.Range (minRandomRotation.x, maxRandomRotation.x), Random.Range (minRandomRotation.y, maxRandomRotation.y), Random.Range (minRandomRotation.z, maxRandomRotation.z));

					Vector3 scaleVoulu = Vector3.one;
					if(uniform){
						float ran = Random.Range (minRandomScale.x, maxRandomScale.x);
						scaleVoulu *= ran;
					}
					else {
						scaleVoulu.x *= Random.Range (minRandomScale.x, maxRandomScale.x);
						scaleVoulu.y *= Random.Range (minRandomScale.y, maxRandomScale.y);
						scaleVoulu.z *= Random.Range (minRandomScale.z, maxRandomScale.z);
					}
					prefabInstance.transform.rotation = rotationVoulue;
					prefabInstance.transform.localScale = scaleVoulu;
					prefabInstance.transform.SetParent(tempGO.transform);
                    
					EditorUtility.SetDirty (prefabInstance);
                    SetBUFromTransform(prefabInstance, prefabInstance.transform.position, rotationVoulue, scaleVoulu, polyParModeles[tempRandomInt]);
					nbOfPlacedObjects++;
					tempGO.name = "LARandomizer ("+ nbOfPlacedObjects +" : "+totalPolys +")" ;
				//	Selection.objects[0] = tempGO;
					/*var key = new Event (KeyCode.LeftArrow, type = EventType.keyDown); 
					SendEvent (key);*/


				}
			}
			//Event.current.Use ();
            else
            {
               if(isShiftPressed && Event.current.type == EventType.MouseDown && e.button == 1)
                {
                    if(buGo != null) { 
                        int tempRandomInt = Random.Range(0, size);
                        GameObject prefabInstance = PrefabUtility.InstantiatePrefab(modelesVoulus[tempRandomInt]) as GameObject;
                        prefabInstance.transform.position = buPosition;
                        totalPolys += polyParModeles[tempRandomInt];
                        totalPolys -= buPolys;
                        Quaternion rotationVoulue = buRotation;

                        Vector3 scaleVoulu = buScale;
                   
                        prefabInstance.transform.rotation = rotationVoulue;
                        prefabInstance.transform.localScale = scaleVoulu;
                        prefabInstance.transform.SetParent(tempGO.transform);
                        DestroyImmediate(buGo);
                        EditorUtility.SetDirty(prefabInstance);
                        SetBUFromTransform(prefabInstance, prefabInstance.transform.position, rotationVoulue, scaleVoulu, polyParModeles[tempRandomInt]);
                        //nbOfPlacedObjects++;

                        tempGO.name = "LARandomizer (" + nbOfPlacedObjects + " : " + totalPolys + ")";
                    }
                }
            }
			if(!active) totalPolys = 0;

		}
	}
}

#endif
