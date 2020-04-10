using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCards : MonoBehaviour
{
    private Renderer rd;
    public Color[] color;
    //private Camera cam;
    private Transform tr;
    private Transform camTr;
    [Range(2f, 100f)]
    public float minFadeDistance = 50f;
    private float distanceToCam;
    [Range(0f, 1f)]
    public float alphaHori = 1f;
    [Range(0f, 1f)]
    public float alphaVerti = 1f;
    public bool isEditionMode = false;
    /* [Range(0f, 2f)]
     public float stretchUVX =1f;
     [Range(0f, 2f)]
     public float stretchUVY = 1f;*/



    // Start is called before the first frame update
    void Start()
    {
        rd = GetComponent<Renderer>();
        tr = transform;
        color[0].a = alphaHori;
        color[1].a = alphaVerti;
        rd.materials[0].color = color[0];
        rd.materials[1].color = color[1];
        //  rd.material.mainTextureScale = new Vector2(stretchUVX, stretchUVY);
    }

    // Update is called once per frame
    void Update()
    {
        if (rd.isVisible)
        {
           //rd.material.mainTextureScale = new Vector2(stretchUVX, stretchUVY);
            //rd.material.mainTextureOffset = new Vector2(stretchUVX , stretchUVY) ;
            distanceToCam = Vector3.Distance(tr.position, GameManager.instance.currentCamera.tr.position);
            if (distanceToCam <= minFadeDistance)
            {
                color[0].a = alphaHori * distanceToCam / minFadeDistance;
                color[1].a = alphaVerti * distanceToCam / minFadeDistance;
            }
            else
            {
                color[0].a = alphaHori ;
                color[1].a = alphaVerti ;
            }
            //Debug.Log(color.a);
            rd.materials[0].color = color[0];
            rd.materials[1].color = color[1];
        }

        if (isEditionMode)
        {
            color[0].a = alphaHori;
            color[1].a = alphaVerti;
            rd.materials[0].color = color[0];
            rd.materials[1].color = color[1];
        }
    }
}
