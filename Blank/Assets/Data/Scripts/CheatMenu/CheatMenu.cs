/*Script cree le : 11-04-2019
 * Par: David Babin
 * Derniere modification: 06-05-2019, 16-05-19,
 * Par: David Babin, David Babin,
 * Description: ajout de commentaires et locking in state du character, Ajout du changing de level avec les alphakeys
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatMenu : MonoBehaviour
{
    #region Variable

    private Vector3 moveDirection = Vector3.zero;
    private Transform tr;
    private PlayerCamera currentCamera;
    private Vector3 velocity;

    //Variables rotation avatar
    private float turnSmoothVelocity;
    public float turnSmoothTime = 2f;
    private float targetRotation;
    private float currentRotation;

    //[Header("Modifie la vitesse de deplacement du ghostMode vertical")]
    //[HideInInspector]
    public float speed = 30f;
    //[Header("Modifie la vitesse de deplacement du ghostMode horizontale")]
    //[HideInInspector]
    public float upSpeed = 20f;
    [HideInInspector]
    public bool ghostMode = false;
    [HideInInspector]
    public bool ghostModeSelected = false;
    [HideInInspector]
    public bool isChangingLevel = false;
    [HideInInspector]
    public bool debugIsOn = false;
    //[HideInInspector]
    public bool lvlIsLoaded = false;
    //[HideInInspector]
    public bool cheatMenuActive = false;
    public bool canChangeLvl = true;
    public Canvas myCanvas;
    #endregion

    private void Start()
    {
        tr = transform;
        
    }

    private void LvlInit()
    {
        lvlIsLoaded = true;
        currentCamera = GameManager.instance.cameraManager.thirdPersonCam;
    }

    void Update()
    {
        if (InputsManager.instance.cheatMenuActive)
        {
            cheatMenuActive = true;
            debugIsOn = true;
            canChangeLvl = true;
            if (/*lvlIsLoaded && */GameManager.instance.currentAvatar != null) DeactivateAvatar();
        }
        else
        {
            cheatMenuActive = false;
            debugIsOn = false;
            canChangeLvl = false;
        }

        if (cheatMenuActive)
        {
            //permet de change de lvl selon l<alpha key qui est appuyer
            if (InputsManager.instance.keyboardKeyWasPressed && canChangeLvl)
            {
                DebugChangeLvl();
                InputsManager.instance.keyboardKeyWasPressed = false;

                cheatMenuActive = false;
                canChangeLvl = false;

                Invoke("LvlInit", 0.5f);
                Invoke("ResetChangeLvl", 5f);
                //Debug.Log("BOOOOO");
                
            }

            if (ghostMode)
            {
                //lock le state de blank falling durant le ghost mode
                MoveGhost();
            }
            /*
            if (InputsManager.instance.back)
                EndDebug();
                */
        }
        //Debug.Log("canChanbger " + canChangeLvl);
    }

    private void ResetChangeLvl()
    {
        canChangeLvl = true;
    }

    /// <summary>
    /// Ouvre le menu du debug et permet de choisir les options de debug(pour l'instant il n'y a pas le menu alors on va straight au ghost mode
    /// </summary>
    public void DeactivateAvatar()
    {
        //GameManager.instance.currentAvatar.DisableInput(true);
        GameManager.instance.currentAvatar.inputDisabled = true;
        StartGhostMode();
    }
    /// <summary>
    /// Active le ghost mode du character
    /// </summary>
    private void StartGhostMode()
    {
        ghostMode = true;
        GameManager.instance.currentAvatar.onGround = false;
        GameManager.instance.currentAvatar.controller.enabled = false;
        GameManager.instance.currentAvatar.stateMachine.ChangeState(GameManager.instance.currentAvatar.stateMachine.fall);
    }

    /// <summary>
    /// Permet de changer de level selon quel alpha key est appuyer
    /// </summary>
    private void DebugChangeLvl()
    {
        switch (InputsManager.instance.lvlKey)
        {
            case KeyCode.Alpha1:
                LevelManager.instance.LoadLevel(LevelManager.LevelToLoad.level1, false);
                break;
            case KeyCode.Alpha2:
                LevelManager.instance.LoadLevel(LevelManager.LevelToLoad.level2, false);
                break;
            case KeyCode.Alpha3:
                LevelManager.instance.LoadLevel(LevelManager.LevelToLoad.level3, false);
                break;
            case KeyCode.Alpha4:
                LevelManager.instance.LoadLevel(LevelManager.LevelToLoad.level4, false);
                break;
            case KeyCode.Alpha5:
                LevelManager.instance.LoadLevel(LevelManager.LevelToLoad.level5, false);
                break;
            case KeyCode.Alpha6:
                LevelManager.instance.LoadLevel(LevelManager.LevelToLoad.level6, false);
                break;
            case KeyCode.Alpha7:
                LevelManager.instance.LoadLevel(LevelManager.LevelToLoad.level7, false);
                break;
            case KeyCode.Alpha8:
                LevelManager.instance.LoadLevel(LevelManager.LevelToLoad.level8, false);
                break;
            case KeyCode.Alpha9:
                LevelManager.instance.LoadLevel(LevelManager.LevelToLoad.level9, false);
                break;
        }

    }

    

    /// <summary>
    /// Lorsque le debug prend fin on redonne tout les controlles de l'avatar
    /// </summary>
    public void EndDebug()
    {
        cheatMenuActive = false;
        lvlIsLoaded = false;
        ghostMode = false;
        ghostModeSelected = false;
        canChangeLvl = true;
                //GameManager.instance.currentAvatar.DisableInput(false);
        GameManager.instance.currentAvatar.inputDisabled = false;
        GameManager.instance.currentAvatar.controller.enabled = true;
    }

    /// <summary>
    /// Permet de bouger le character sans collision.
    /// </summary>
    private void MoveGhost()
    {
        moveDirection = InputsManager.instance.leftStick;

        //Rotation selon les mouvements
        if (moveDirection.magnitude != 0f)
        {
            targetRotation = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + GameManager.instance.currentCamera.tr.eulerAngles.y;
            GameManager.instance.currentAvatar.tr.eulerAngles = Vector3.up * targetRotation;

            //Implementation du mouvement
            GameManager.instance.currentAvatar.tr.position += GameManager.instance.currentAvatar.tr.forward * moveDirection.magnitude * speed * Time.deltaTime;
        }

        if (InputsManager.instance.bumperRight) GameManager.instance.currentAvatar.tr.Translate(Vector3.up * upSpeed * Time.deltaTime);
        if (InputsManager.instance.bumperLeft) GameManager.instance.currentAvatar.tr.Translate(-Vector3.up * upSpeed * Time.deltaTime);
    }
}
