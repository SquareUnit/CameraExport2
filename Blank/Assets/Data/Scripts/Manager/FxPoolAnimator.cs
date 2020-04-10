using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxPoolAnimator : MonoBehaviour
{
    public string FXName;
    ParticleSystem currentFx;
    [HideInInspector] public PoolingManager myPoolFX;
    public float time;
    // Start is called before the first frame update
    void Start()
    {
        myPoolFX = GameManager.instance.GetComponent<PoolingManager>();
        if (FXName != "")
        {
            GameObject o = myPoolFX.GetObjectAutoReturn(FXName, transform.position, transform.eulerAngles, time);//GameManager.instance.fxPool.GetObject(FXName, transform.position);
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

    public void PlayFX(string fxName)
    {

    }

    public void PlayFX(string fxName, float fxTime)
    {

    }

}
