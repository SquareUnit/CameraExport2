using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolTester : MonoBehaviour
{
    public string FXName;
    ParticleSystem currentFx;
    public GameObject positionFX;
    // Start is called before the first frame update
    void Start()
    {
        //myPoolFX = GameManager.instance.GetComponent<PoolingManager>();
        if (FXName != "" && positionFX != null)
        {           
                GameObject o = GameManager.instance.fxPool.GetObject(FXName, positionFX.transform.position, positionFX.transform.rotation.eulerAngles);  //myPoolFX.GetObject(FXName, transform.position);
                o.transform.parent = transform;
                if (o != null)
                    currentFx = o.GetComponentInChildren<ParticleSystem>();      
        }
    }
    
    public void Activate()
    {
        currentFx.Play();
    }

    public void Deactivate()
    {
        currentFx.Stop();
    }
    
}
