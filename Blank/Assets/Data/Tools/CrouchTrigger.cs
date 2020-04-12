using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchTrigger : Triggerable
{
    public override void Activate()
    {
        GameManager.instance.currentAvatar.canDecrouch = false;
        GameManager.instance.currentCamera.pitchMin = -25.0f;
        GameManager.instance.currentCamera.pitchMax = 0.0f;
    }

    public override void Deactivate()
    {
        GameManager.instance.currentAvatar.canDecrouch = true;
        GameManager.instance.currentCamera.pitchMin = -25.0f;
        GameManager.instance.currentCamera.pitchMax = 60.0f;
    }

    public override void Toggle()
    {

    }
}
