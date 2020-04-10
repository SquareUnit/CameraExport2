// Créé par: Lucien Lehmann
// En date du: 03-05-19
// Modifié par: David Babin
// En date du: 08/07/19 : Refactorisation et optimisation

using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PantinCamera : MonoBehaviour
{
    private Transform tr;

    private Transform hitTr;
    private Vector3 newForward;

    public List<PantinInfo> pantinInMap;
    private float closestDistance = float.MaxValue;
    private int smallestPosiInList = 0;
    private int currentPosiInList;
    private Vector3 pantinCurrentPosi;

    private bool playerCamOn = true;
    private bool tpOnce = true;
    private bool collisionInVision;
    private bool fadeOnce = true;
    private bool playerDisable = true;
    private bool inPatin = false;

    private float distance;
    private float yaw;
    private float pitch;
    private float rotationSmoothTime = 0.12f;

    private Vector2 yawMinMax;
    private Vector2 pitchMinMax;

    private Vector3 avatarOffSet = Vector3.up * 2f;
    private Vector3 currentRotation;
    private Vector3 desiredRotation;
    private Vector3 rotationSmoothVel;

    private Character currentAvatar;
    private PantinInfo pantinInfo;
    private PlayerCamera playerCam;

    public LayerMask currentColisionLayer;

    private bool canGoInPatin = false;

    private void Awake()
    {
        tr = transform;
        newForward = tr.rotation.eulerAngles;
        playerCam = GetComponent<PlayerCamera>();
    }

    private void Update()
    {
        if(currentAvatar == null) currentAvatar = GameManager.instance.currentAvatar;
        
        if (pantinInMap.Count > 0)
        {
            for (int i = 0; i < pantinInMap.Count; i++)
            {
                if (pantinInMap[i] != null)
                {
                    if (pantinInMap[i].skinMeshRD != null && pantinInMap[i].skinMeshRD.isVisible)
                    {
                        distance = Vector3.Distance(tr.position, pantinInMap[i].tr.position);
                        closestDistance = Vector3.Distance(tr.position, pantinInMap[smallestPosiInList].tr.position);

                        if (closestDistance > distance)
                        {
                            smallestPosiInList = i;
                        }

                        if (pantinInfo != pantinInMap[smallestPosiInList].GetComponentInParent<PantinInfo>())
                        {
                            pantinInfo.isActivate = false;
                            pantinInfo = pantinInMap[smallestPosiInList].GetComponentInParent<PantinInfo>();
                        }

                        collisionInVision = Physics.Linecast(currentAvatar.tr.position + Vector3.up * 2f, pantinInMap[smallestPosiInList].tr.position + Vector3.up * 2f, currentColisionLayer);
                        

                        if (pantinInMap[smallestPosiInList].skinMeshRD.isVisible && !GameManager.instance.pantinCamIsOff && !collisionInVision && currentAvatar.onGround)
                        {
                            if (playerCamOn)
                            {
                                pantinInfo.isActivate = true;
                            }
                        }
                        else
                        {
                            pantinInfo.isActivate = false;
                            int n = 0;
                            while (pantinInMap[n].skinMeshRD.isVisible == false)
                            {
                                n++;
                            }
                            smallestPosiInList = n;
                        }
                    }
                }
            }

            if (pantinInMap[smallestPosiInList] != null && pantinInMap[smallestPosiInList].skinMeshRD != null && pantinInMap[smallestPosiInList].skinMeshRD.isVisible && currentAvatar.onGround || inPatin)
            {
                if (InputsManager.instance.back && fadeOnce && !GameManager.instance.characterIsDead)
                {
                    if (!collisionInVision || inPatin == true)
                    {
                        canGoInPatin = true;
                        UIManager.instance.Invoke("EnableFadeToBlackPatin", 0f);
                        fadeOnce = false;
                        playerCam.enabled = false;
                        currentAvatar.stateMachine.interact.interactTime = float.MaxValue;
                        currentAvatar.stateMachine.ChangeState(currentAvatar.stateMachine.interact);
                        playerDisable = false;
                    }
                }

                if(InputsManager.instance.crouch && fadeOnce && inPatin == true)
                {
                    UIManager.instance.Invoke("EnableFadeToBlackPatin", 0f);
                    fadeOnce = false;
                    playerCam.enabled = false;
                    currentAvatar.stateMachine.interact.interactTime = float.MaxValue;
                    currentAvatar.stateMachine.ChangeState(currentAvatar.stateMachine.interact);
                    playerDisable = false;
                }


                if (UIManager.instance.fadeToBlack.color.a >= 0.99 && tpOnce)
                {
                    if (playerCamOn && !collisionInVision && canGoInPatin)
                    {
                        canGoInPatin = false;

                        inPatin = true;

                        pantinInfo.isActivate = false;

                        hitTr = pantinInfo.camLocTr;
                        currentPosiInList = smallestPosiInList;
                        pantinCurrentPosi = pantinInMap[currentPosiInList].tr.position;

                        playerCamOn = false;
                        GameManager.instance.playerCamOn = playerCamOn;

                        closestDistance = float.MaxValue;

                        StartingCamera(hitTr);
                    }
                    else
                    {
                        if (!playerCamOn)
                        {
                            currentAvatar.canCrouch = false;
                            currentAvatar.Invoke("ResetCrouch", 0.5f);
                            pantinInMap[currentPosiInList].tr.position = pantinCurrentPosi;
                            playerCam.enabled = true;
                            playerCamOn = true;
                            inPatin = false;
                            GameManager.instance.playerCamOn = playerCamOn;

                            if (currentAvatar.isCrouching)
                                currentAvatar.stateMachine.ChangeState(currentAvatar.stateMachine.crouch);
                            else
                                currentAvatar.stateMachine.ChangeState(currentAvatar.stateMachine.idle);
                        }
                    }
                    tpOnce = false;
                }
            }
        }

        if (UIManager.instance.fadeToBlack.color.a == 0)
        {
            tpOnce = true;
            fadeOnce = true;
            playerDisable = true;
        }
    }

    private void LateUpdate()
    {
        if (!playerCamOn)
        {
            SetCameraOrientation();
        }
    }

    ///<summary> Set up the camera orientation </summary>
    ///turn off when in player cam
    private void SetCameraOrientation()
    {
        desiredRotation = new Vector3(pitch, yaw, 0.0f);
        currentRotation = Vector3.SmoothDamp(currentRotation, desiredRotation, ref rotationSmoothVel, rotationSmoothTime);
        tr.eulerAngles = currentRotation;
    }

    /// <summary> Set the camera pantin position and the rotation so it's facing the pantin default forward </summary>
    /// God was here
    public void StartingCamera(Transform hitTr)
    {
        tr.position = hitTr.position;
        newForward = hitTr.rotation.eulerAngles;
        yaw = newForward.y;
        pitch = newForward.x;

        pantinInMap[currentPosiInList].transform.position = new Vector3(1000, 1000, 1000);

    }

    public void Init()
    {
        currentAvatar = GameManager.instance.currentAvatar;
        for (int i = 0; i < pantinInMap.Count; i++)
            pantinInMap.RemoveAt(i);
        pantinInMap = GameManager.instance.spcLvlMan.pantinInMap;
        for (int i = 0; i < pantinInMap.Count; i++)
            pantinInMap[i].GetComponent<PantinInfo>();
        currentPosiInList = pantinInMap.Count;
        pantinInfo = pantinInMap[0];
    }
}