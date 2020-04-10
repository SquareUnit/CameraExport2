using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTriggerable : Triggerable
{
    public AudioSource myAudio;
    public AudioClip myClip;

    
    public override void Activate()
    {
        if (myAudio != null) {
            myAudio.clip = myClip;
            myAudio.PlayOneShot(myAudio.clip);
            Debug.Log("played");

        }
    }
    public override void Deactivate()
    {

    }
    public override void Toggle()
    {

    }
}
