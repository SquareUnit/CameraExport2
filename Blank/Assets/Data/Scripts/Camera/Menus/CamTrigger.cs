//Creer par Valentin et Maxime P.
//Date de création [2019-04-08]

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CamTrigger : MonoBehaviour
{
    public CameraTriggerManager.ActiveCam SwitchTo, Default;
    public UnityEvent CameraEvent;

    private void OnTriggerEnter(Collider other)
    {
        CameraTriggerManager.instance.ChangeCurrent(SwitchTo);
        CameraEvent.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        CameraTriggerManager.instance.ChangeCurrent(Default);
    }
}
