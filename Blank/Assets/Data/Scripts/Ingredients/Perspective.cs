/// SettupPlatformEvent passer dans le manager. REFACTOR
/// G/n/rer automatiquement les reference a utiliser. REFACTOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class Perspective : MonoBehaviour
{
    private Transform tr;
    [Tooltip("Only for undefined platforms")] public bool undefinedStartVisible = true;
    public enum Type { undefined, additive, substractive }
    public Type type;

    private bool hasNavObstacle;
    private bool hasSwapMaterial;
    private Renderer meshRend;
    public Collider coll1;
    public Collider coll2;
    public Collider triggerBox;
    private bool platfSideCollDisabled;
    private NavMeshObstacle navObstacle;
    private SwapMaterial swapMaterial;

    private ParticleSystem[] pSystems;
    private ParticleSystemRenderer pSysRend;
    private bool hasPSyst;
    public ParticleSystem pSys01;
    public ParticleSystem pSys02;
    public Material pSysNeutralMat;
    public Material pSysAddMat;
    public Material pSysSubMat;

    private bool allowSetup = true;

    private void Awake()
    {
        tr = transform;

        if (GetComponentInChildren<MeshRenderer>() != null)
        {
            meshRend = GetComponentInChildren<MeshRenderer>();
        }
        else if (GetComponentInChildren<SkinnedMeshRenderer>() != null)
        {
            meshRend = GetComponentInChildren<SkinnedMeshRenderer>();
        }

        if (GetComponentInChildren<NavMeshObstacle>() != null)
        {
            hasNavObstacle = true;
            navObstacle = GetComponentInChildren<NavMeshObstacle>();
        }

        if (GetComponentInChildren<SwapMaterial>() != null)
        {
            hasSwapMaterial = true;
            swapMaterial = GetComponentInChildren<SwapMaterial>();
        }
    }

    void Start()
    {
        GameManager.instance.callReset.AddListener(ResetOnDeath);
        if (GetComponentInChildren<ParticleSystem>() != null)
        {
            hasPSyst = true;
            pSystems = new ParticleSystem[2];
            pSystems = GetComponentsInChildren<ParticleSystem>();
        }
    }

    /// <summary> Run once when avatar != null and once again each time inspector is modified</summary>
    private void Update()
    {
        if (GameManager.instance.currentAvatar != null)
        {
            if (allowSetup)
            {
                SetupPlatformEvents(GetPlatformType(type));
                allowSetup = false;
            }
        }
    }

    private void OnValidate()
    {
        allowSetup = true;
    }

    private void SetupPlatformEvents(int currentType)
    {
        switch (currentType)
        {
            case 0:
                RemoveAllListeners();
                if (hasSwapMaterial)
                {
                    swapMaterial.meshSelected = SwapMaterial.meshToAffect.dissolve;
                    swapMaterial.matSelected = SwapMaterial.matToSwap.mat00;
                    swapMaterial.SetMatToMesh(swapMaterial.matList[(int)swapMaterial.matSelected]);
                    swapMaterial.meshSelected = SwapMaterial.meshToAffect.bottom;
                    swapMaterial.matSelected = SwapMaterial.matToSwap.mat03;
                    swapMaterial.SetMatToMesh(swapMaterial.matList[(int)swapMaterial.matSelected]);
                }
                if (hasPSyst)
                {
                    if (pSystems[0] != null)
                    {
                        pSysRend = pSystems[0].GetComponent<ParticleSystemRenderer>();
                        pSysRend.material = pSysNeutralMat;
                    }
                    if (pSystems[1] != null)
                    {
                        pSysRend = pSystems[1].GetComponent<ParticleSystemRenderer>();
                        pSysRend.material = pSysNeutralMat;
                    }
                }
                // Set initial state of default platform to 'everything visible'
                if (undefinedStartVisible)
                {
                    PermanentlyActivate();
                }
                else
                {
                    PermanentlyDeactivate();
                }

                break;

            case 1:
                // Remove old listeners & add new listeners
                GameManager.instance.callSubstractiveStart.RemoveListener(SubstractiveActivate);
                GameManager.instance.callSubstractiveEnd.RemoveListener(SubstractiveDeactivate);
                GameManager.instance.callAdditiveStart.AddListener(AdditiveActivate);
                GameManager.instance.callAdditiveEnd.AddListener(AdditiveDeactivate);
                // Set platform materials
                if (hasSwapMaterial)
                {
                    swapMaterial.meshSelected = SwapMaterial.meshToAffect.dissolve;
                    swapMaterial.matSelected = SwapMaterial.matToSwap.mat01;
                    swapMaterial.SetMatToMesh(swapMaterial.matList[(int)swapMaterial.matSelected]);
                    swapMaterial.meshSelected = SwapMaterial.meshToAffect.bottom;
                    swapMaterial.matSelected = SwapMaterial.matToSwap.mat04;
                    swapMaterial.SetMatToMesh(swapMaterial.matList[(int)swapMaterial.matSelected]);
                }
                if (hasPSyst)
                {
                    if (pSystems[0] != null)
                    {
                        pSysRend = pSystems[0].GetComponent<ParticleSystemRenderer>();
                        pSysRend.material = pSysAddMat;
                    }

                    if (pSystems[1] != null)
                    {
                        pSysRend = pSystems[1].GetComponent<ParticleSystemRenderer>();
                        pSysRend.material = pSysAddMat;
                    }
                }
                AddDeactivate();
                break;

            case 2:
                // Remove old listeners & add new listeners
                GameManager.instance.callAdditiveStart.RemoveListener(AdditiveActivate);
                GameManager.instance.callAdditiveEnd.RemoveListener(AdditiveDeactivate);
                GameManager.instance.callSubstractiveStart.AddListener(SubstractiveActivate);
                GameManager.instance.callSubstractiveEnd.AddListener(SubstractiveDeactivate);
                // Set platform materials
                if (hasSwapMaterial)
                {
                    swapMaterial.meshSelected = SwapMaterial.meshToAffect.dissolve;
                    swapMaterial.matSelected = SwapMaterial.matToSwap.mat02;
                    swapMaterial.SetMatToMesh(swapMaterial.matList[(int)swapMaterial.matSelected]);
                    swapMaterial.meshSelected = SwapMaterial.meshToAffect.bottom;
                    swapMaterial.matSelected = SwapMaterial.matToSwap.mat05;
                    swapMaterial.SetMatToMesh(swapMaterial.matList[(int)swapMaterial.matSelected]);
                }
                if (hasPSyst)
                {
                    if (pSystems[0] != null)
                    {
                        pSysRend = pSystems[0].GetComponent<ParticleSystemRenderer>();
                        pSysRend.material = pSysSubMat;
                    }
                    if (pSystems[1] != null)
                    {
                        pSysRend = pSystems[1].GetComponent<ParticleSystemRenderer>();
                        pSysRend.material = pSysSubMat;
                    }
                }
                SubDeactivate();
                break;
        }
    }

    private int GetPlatformType(Type platformType)
    {
        return (int)platformType;
    }

    private void RemoveAllListeners()
    {
        GameManager.instance.callSubstractiveStart.RemoveListener(SubstractiveActivate);
        GameManager.instance.callSubstractiveEnd.RemoveListener(SubstractiveDeactivate);
        GameManager.instance.callAdditiveStart.RemoveListener(AdditiveActivate);
        GameManager.instance.callAdditiveEnd.RemoveListener(AdditiveDeactivate);
    }

    private void AdditiveActivate()
    {
        if (hasPSyst)
        {
            pSystems[0].Play(true);
            pSystems[1].Stop();
        }
        meshRend.enabled = true;
        if (coll1 != null) coll1.enabled = true;
        if (coll2 != null) coll2.enabled = true;
        if (hasNavObstacle) navObstacle.enabled = true;
    }

    private void AdditiveDeactivate()
    {
        CancelInvoke("AddDeactivate");
        Invoke("AddDeactivate", 0.150f);
    }

    private void AddDeactivate()
    {
        if (hasPSyst)
        {
            pSystems[0].Stop();
            pSystems[1].Play(true);
        }
        meshRend.enabled = false;
        if (coll1 != null) coll1.enabled = false;
        if (coll2 != null) coll2.enabled = false;
        if (hasNavObstacle) navObstacle.enabled = false;
    }

    private void SubstractiveActivate()
    {
        if (hasPSyst)
        {
            pSystems[0].Stop();
            pSystems[1].Play(true);
        }
        meshRend.enabled = false;
        if (coll1 != null) coll1.enabled = false;
        if (coll2 != null) coll2.enabled = false;
        if (hasNavObstacle) navObstacle.enabled = false;
    }

    private void SubstractiveDeactivate()
    {
        CancelInvoke("SubDeactivate");
        Invoke("SubDeactivate", 0.150f);
    }

    private void SubDeactivate()
    {
        if (hasPSyst)
        {
            pSystems[0].Play(true);
            pSystems[1].Stop();
        }
        meshRend.enabled = true;
        if (coll1 != null) coll1.enabled = true;
        if (coll2 != null) coll2.enabled = true;
        if (hasNavObstacle) navObstacle.enabled = true;
    }

    public void PermanentlyActivate()
    {
        if (hasPSyst)
        {
            pSystems[0].Play(true);
            pSystems[1].Stop();
        }
        meshRend.enabled = true;
        if (coll1 != null) coll1.enabled = true;
        if (coll2 != null) coll2.enabled = true;
        if (hasNavObstacle) navObstacle.enabled = true;
    }

    public void PermanentlyDeactivate()
    {
        if (hasPSyst)
        {
            pSystems[0].Stop();
            pSystems[1].Play(true);
        }
        meshRend.enabled = false;
        if (coll1 != null) coll1.enabled = false;
        if (coll2 != null) coll2.enabled = false;
        if (hasNavObstacle) navObstacle.enabled = false;
    }

    public void ResetOnDeath()
    {
        if(type == Type.additive)
        {
            AdditiveDeactivate();
        }
        else if(type == Type.substractive)
        {
            SubstractiveDeactivate();
        }
    }
}
