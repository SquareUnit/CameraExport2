using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShotTool : MonoBehaviour
{
    [Tooltip("Factor by wich to increase resolution")]
    public int superSize = 0;

    private int cpt = 1;

    private void Start()
    {
        cpt = 1;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            ScreenCapture.CaptureScreenshot("Assets/Data/ScreenShot" + cpt + ".png", superSize);
            cpt++;
        }
    }

}
