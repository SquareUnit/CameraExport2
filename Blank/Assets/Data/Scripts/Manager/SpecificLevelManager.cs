/*Script cree le : 16-05-2019
 * Par: David Babin
 * Derniere modification: 
 * Par: 
 * Description: 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class SpecificLevelManager : MonoBehaviour
{
    //TO DO : starter le camera manager, gestalt manager, pooling et les checkpoint;

    public CameraManager cameraManager;
    [Header("Nombre de puzzle de gestalt dans votre niveau")]
    public GestaltManager[] gestalts;
    public PoolingManager FX;
    [Header("Placer votre prefab Spawnpoint ici")]
    public SpawnPointLocation spawnPoint;
    [HideInInspector]
    public Vector3 playerStart;
    [HideInInspector]
    public Quaternion playerStartRotation;
    [HideInInspector]
    public List<PantinInfo> pantinInMap;
    public PostProcessProfile lvlProfile;

    [Header("Can Blank jump in this level?")]
    public bool jumpUnlocked = false;
    [Header("Can Blank activate additive perspective in this level?")]
    public bool perspAddUnlocked = false;
    [Header("Can Blank activate substractive perspective in this level?")]
    public bool perspSubsUnlocked = false;
    [Header("Does Blank have a head in this level?")]
    public bool hasHead = true;

    public int totalCollectible;
    public int collectibleCollected;

    private void Start()
    {
        GameManager.instance.spcLvlMan = this;
        GameManager.instance.fxPool = FX;
    }

    public void Init()
    {
        GameManager.instance.spcLvlMan = this;
        if (cameraManager.thirdPersonCam == null)
        {
            cameraManager.CameraManInit();
            cameraManager.thirdPersonCam.GetComponent<PantinCamera>().Init();
            cameraManager.thirdPersonCam.GetComponent<PostProcessVolume>().profile = lvlProfile;
        }
        Invoke("AssignGestalt", 2f);
        AssignSpawnPoint();
        Invoke("AssignCamTarget", 0.2f);
    }

    public void OnChangeLevel()
    {
        AssignSpawnPoint();
        Invoke("AssignPantinCamera", 4f);
        Invoke("AssignGestalt", 2f);
        Invoke("AssignCameraProfile", 1f);
        AssignCamTarget();
    }

    public void AssignCamTarget()
    {
        cameraManager.thirdPersonCam.camFSM.ChangeState(cameraManager.thirdPersonCam.onLoadState);
    }

    public void AssignSpawnPoint()
    {
        playerStart = spawnPoint != null ? spawnPoint.transform.position : Vector3.zero;
        playerStartRotation = spawnPoint != null ? spawnPoint.transform.rotation : Quaternion.identity;
    }

    public void AssignPantinCamera()
    {
        if (cameraManager.thirdPersonCam != null)
            cameraManager.thirdPersonCam.GetComponent<PantinCamera>().Init();
    }

    public void AssignGestalt()
    {
        for (int i = 0; i < gestalts.Length; i++)
        {
            if (gestalts[i] != null)
            {
                gestalts[i].Init();
            }
        }
    }

    public void AssignCameraProfile()
    {
        if (cameraManager.thirdPersonCam != null && cameraManager.thirdPersonCam.GetComponent<PostProcessVolume>() != null)
            cameraManager.thirdPersonCam.GetComponent<PostProcessVolume>().profile = lvlProfile;
    }
}
