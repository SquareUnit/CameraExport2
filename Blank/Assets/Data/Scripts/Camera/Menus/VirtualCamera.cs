//Creer par Valentin et Maxime P.
//Date de création [2019-04-08]

using Cinemachine;

public class VirtualCamera : CinemachineVirtualCamera
{
    public CameraTriggerManager.ActiveCam type;
    
    public void ChangeCamPriority(CameraTriggerManager.ActiveCam changeTo)
    {
        if (changeTo != type)
            Priority = 1;
        else
            Priority = 10;
    }
        
}
