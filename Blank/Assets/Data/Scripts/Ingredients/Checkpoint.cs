using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : Triggerable
{
    public Renderer rend;
    public Material checkPointMat;
    public SoundRelay soundRelay;
    public bool dontPlaySound;


    private void Start()
    {
        soundRelay = GetComponent<SoundRelay>();

    }

    public override void Activate()
    {
        MusicPlayer.instance.AddLowPassFilters(1.5f, 400);
        GameManager.instance.checkpointLocation = transform.position;
        GameManager.instance.checkpointRotation = transform.eulerAngles;
        rend.material = checkPointMat;
        UIManager.instance.EnableSavePanel();
        if(!dontPlaySound)
            soundRelay.PlayAudioClip(0);

        // FX phil
        GameManager.instance.fxPool.GetObjectAutoReturn("FX_SavePoint",transform.position, 2.0f);
    }

    public override void Deactivate()
    {

    }

    public override void Toggle()
    {
       
    }
}
