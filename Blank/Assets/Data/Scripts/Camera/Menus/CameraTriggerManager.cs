//Creer par Valentin et Maxime P.
//Date de création [2019-04-08]

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;
public class CameraTriggerManager : MonoBehaviour
{
    //[2019-04-04]Valentin et Maxime P: Type of camera to set in the inspector
    public enum ActiveCam { Player, Tower } 
    public ActiveCam currentCam; 
    public static CameraTriggerManager instance;
    //[2019-04-04]Valentin et Maxime P: List of virtual camera to enter in the inspector
    public List<VirtualCamera> cameras = new List<VirtualCamera>(); 

    ActiveCam c;
    UnityCamEvent OnSwitchCam = new UnityCamEvent();

    void Awake()
    {
        SetupSingleton();

        //[2019-04-04]Valentin et Maxime P: Registers camera added to list in inspector
        RegisterCameras();
        c = currentCam;
    }

    public void Update()
    {
        //[2019-04-04]Valentin et Maxime P: Switch camera in Runtime with enum in inspector
        if (currentCam != c)
            ChangeCurrent(currentCam);
    }

    /// <summary>
    /// Change priority of all camera putting the selected camera at highest priority in cinemachine component.
    /// </summary>
    public void ChangeCurrent(ActiveCam changeTo)
    {
        OnSwitchCam.Invoke(changeTo);
        currentCam = changeTo;
        c = currentCam;
    }

    void RegisterCameras()
    {
        foreach (VirtualCamera c in cameras)
            OnSwitchCam.AddListener((camType) => c.ChangeCamPriority(camType));
    }

    //[2019-04-04]Valentin et Maxime P: Create a singleton of CameraTriggerManager 
    private void SetupSingleton()
    {         
        if (!instance) instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
       
    public class UnityCamEvent : UnityEvent<ActiveCam> { }
   
}
