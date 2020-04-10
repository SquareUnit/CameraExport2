﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamRevealInfo : MonoBehaviour
{
    
    [HideInInspector] public Vector3 camTargetStartPos;
    [Header("Hover on var names to display tooltips")]
    [Tooltip("Leave empty if you just want the camera rotation to move")]
    public Transform camTargetTargetPos;
    public float lerpTime;
    public float revealTime;
    public float desiredYaw;
    public float desiredPitch;

    public void SetupCamRevealInfo()
    {
        camTargetStartPos = GameManager.instance.cameraManager.thirdPersonCam.camTarget.tr.position;
        if(camTargetTargetPos == null)
        {
            camTargetTargetPos = GameManager.instance.cameraManager.thirdPersonCam.camTarget.tr;
        }
    }


}
