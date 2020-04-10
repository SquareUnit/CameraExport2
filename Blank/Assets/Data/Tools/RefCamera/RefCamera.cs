using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefCamera : MonoBehaviour
{
    public float targetOffsetX;
    public float targetOffsetY;
    public float targetOffsetZ;
    public float camPitch = 15f;

    public void ResetCamera()
    {
        GameManager.instance.currentCamera.camFSM.ChangeState(GameManager.instance.currentCamera.resetState);
    }

    public void Lookat(Transform lookAtTarget)
    {
        //Debug.Log(lookAtTarget.position.ToString());
        GameManager.instance.currentCamera.valveState.valueToDesiredPitch = camPitch;
        GameManager.instance.currentCamera.valveState.target = lookAtTarget.position + lookAtTarget.right * targetOffsetX + lookAtTarget.up * targetOffsetY + lookAtTarget.forward * targetOffsetZ;
        GameManager.instance.currentCamera.camFSM.ChangeState(GameManager.instance.currentCamera.valveState);
    }
}
