using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerManager : MonoBehaviour
{
    public VideoPlayer myVP;
    private ScriptedEvent scriptedEvent;
    public int timer = 0;

    void Start()
    {
        myVP = GetComponent<VideoPlayer>();
        myVP.loopPointReached += ThisIsTheEnd;
        scriptedEvent = GetComponent<ScriptedEvent>();

        if(myVP.targetCamera == null)
            Invoke("GetCamera", 5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!myVP.isPlaying) myVP.Play();
        if (InputsManager.instance.back && myVP.isPlaying)
        {
            timer++;
            if (timer >= 2)
            {
                myVP.Stop();
                myVP.enabled = false;
                timer = 0;
                if(scriptedEvent != null)
                    scriptedEvent.Activate();

            }
        }
    }

    private void ThisIsTheEnd(UnityEngine.Video.VideoPlayer vp)
    {
        myVP.enabled = false;
        scriptedEvent.Activate();
    }

    private void GetCamera()
    {
        myVP.targetCamera = GameManager.instance.currentCamera.GetComponent<Camera>();
    }
}
