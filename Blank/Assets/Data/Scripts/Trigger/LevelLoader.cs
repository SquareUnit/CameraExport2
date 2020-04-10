// Script Owner : Felix Desrosiers - 25/04/2019
// Last Modification : David Babin - 13/06/19 : fade to black ajouter


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : Triggerable
{
    public LevelManager.LevelToLoad desiredLevel;
    public bool levelDesignOnly = false;

    public override void Activate()
    {
        ChangeLvl();
    }

    public void ChangeLvl()
    {
        InputsManager.instance.StopVibration();
        LevelManager.instance.LoadLevel(desiredLevel, levelDesignOnly);
        Destroy(this.gameObject);
    }
    public override void Deactivate() {}
    public override void Toggle() {}
}
