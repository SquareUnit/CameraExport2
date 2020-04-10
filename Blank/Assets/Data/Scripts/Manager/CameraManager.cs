// Création du script : Benjamin Chouinard
// Date : 02-04-2019 16:15
// Modification : 
//      Par: David Babin
//      Date : 16-05-19
//      Description : deactivation de la camera apres l<avoir instancier

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [HideInInspector] public Transform tr;
    private StateMachine stateMachine;

    public PlayerCamera thirdPersonCamModel;
    public PlayerCamera thirdPersonCam;

    private void Start()
    {
        tr = transform;
        GameManager.instance.cameraManager = this;
        GetCamera();
    }

    public void CameraManInit()
    {
        InstanciateCamera();
    }

    private void InstanciateCamera()
    {
        if(thirdPersonCam == null)
        {
            thirdPersonCam = Instantiate(thirdPersonCamModel, GameManager.instance.spcLvlMan.playerStart, Quaternion.identity);
            GameManager.instance.currentCamera = thirdPersonCam;
        }
    }
    
    public void GetCamera()
    {
        thirdPersonCam = GameManager.instance.currentCamera;
    }
}
