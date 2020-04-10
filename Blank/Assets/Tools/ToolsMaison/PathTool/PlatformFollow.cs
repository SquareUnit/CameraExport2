using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformFollow : MonoBehaviour
{
    public PathFollow p;
    public Vector3 dir;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        p = GetPlatform();
        if (p != null)
        {
            dir = p.GetTargetDirection();
        }
    }

    PathFollow GetPlatform()
    {
       
        return null;
    }
    private void LateUpdate()
    {
        if(p)
            transform.position += p.GetTargetDirection();
    }
           
}
