using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchTrigger : Triggerable
{
    public override void Activate()
    {
        GameManager.instance.currentAvatar.canDecrouch = false;
        GameManager.instance.currentCamera.pitchMinMax = new Vector2(-25f, 0f);
    }

    public override void Deactivate()
    {
        GameManager.instance.currentAvatar.canDecrouch = true;
        GameManager.instance.currentCamera.pitchMinMax = new Vector2(-25f, 60f);
    }

    public override void Toggle()
    {

    }
}
