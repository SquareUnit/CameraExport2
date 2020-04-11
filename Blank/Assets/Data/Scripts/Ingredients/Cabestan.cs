/*Script cree le : 25-04-2019
 * Par: Philippe Beauchemin
 * Derniere modification: 02-07-2019
 * Par: David Babin
 * Description: Ajout du son du cabestan
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cabestan : Triggerable
{
    AudioSource audioData;
    //[HideInInspector]
    public Animator animatorCabestan;
    private Character avatar;
    private Vector3 closest;
    [HideInInspector]
    public float avatarOffsetRight = 1f;
    [HideInInspector]
    public float avatarOffsetForward = 0.2f;
    public RefCamera refCamera;
    public SoundRelay soundRelay;
    public Triggerable triggerable;


    // Interact sprite button
    [HideInInspector]
    public GameObject interactButton;

    [HideInInspector]
    public bool asBeenTurned = false;

    private Animator LinkedDoor;
    private GrandePorte watKindOfDoor;
    private GrandePorteAlternative alternativeDoor;

    public GameObject door;

    public Triggerable triggerableLookAt;
    private float timeInCabestan;
    private bool doOnce;

    // Trigger en X negatif
    [HideInInspector]
    public Trigger triggerXneg;
    // Trigger en X positif
    [HideInInspector]
    public Trigger triggerXpos;

    // Collision lorsque la porte est ouverte
    private Collider doorCollOpen;
    // Collision lorsque la porte est ferme
    private Collider doorCollClose;

    // At the right place to interact
    private bool canInteract = false;

    // Start is called before the first frame update
    void Start()
    {
        interactButton.SetActive(false);

        Transform tr = this.transform;
        animatorCabestan = GetComponent<Animator>();
        audioData = GetComponent<AudioSource>();
        refCamera = GetComponent<RefCamera>();
        soundRelay = GetComponent<SoundRelay>();

        // Collider de la porte & porte
        if (door != null)
        {
            doorCollOpen = door.transform.Find("Collision_Open").GetComponentInChildren<Collider>();
            doorCollClose = door.transform.Find("Collision_Close").GetComponentInChildren<Collider>();

            // Manually set the right door here
            LinkedDoor = door.GetComponentInChildren<Animator>();
            watKindOfDoor = LinkedDoor.GetComponent<GrandePorte>();
            if (watKindOfDoor == null)
            {
                alternativeDoor = LinkedDoor.GetComponent<GrandePorteAlternative>();
            }
        }
    }

    public override void Activate()
    {
        // Player est a la bonne place
        //GameManager.instance.pantinCamIsOff = true;
        canInteract = true;
    }

    public override void Deactivate()
    {
        //GameManager.instance.pantinCamIsOff = false;
        canInteract = false;
    }

    public override void Toggle()
    {

    }


    void Update()
    {
        // look at for the button
        if (interactButton.gameObject.activeInHierarchy) { interactButton.transform.LookAt(GameManager.instance.currentCamera.Tr); }

        // Afficher le bouton interact (X)
        if (canInteract && !asBeenTurned && door != null)
        {
            //affiche bouton
            interactButton.SetActive(true);

        }
        else
        {
            interactButton.SetActive(false);
        }
        if (canInteract && InputsManager.instance.interact && !asBeenTurned && door != null)
        {
            timeInCabestan = 0;
            avatar = GameManager.instance.currentAvatar;

            if (avatar.stateMachine.currentState == avatar.stateMachine.idle && avatar.stateMachine.idle.timeInState >= 0.2f)
            {

                if (Vector3.Distance(avatar.tr.position, triggerXneg.transform.position) < Vector3.Distance(avatar.tr.position, triggerXpos.transform.position))
                    closest = triggerXneg.transform.position;
                else
                    closest = triggerXpos.transform.position;

                //sound by David
                soundRelay.PlayAudioClip(1);
                AnimateAvatar();

                // one time use only
                asBeenTurned = true;

                // Animation Player
                //Invoke("Avance",0.0f);

                // Animation cabestan
                animatorCabestan.SetBool("isUsed", true);

                // Animation Grande Porte
                if (LinkedDoor != null)
                {
                    if (watKindOfDoor != null)
                    {
                        LinkedDoor.SetBool("isBlankPushing", true); // 21/05/19 ajout du null check
                        //sound by David
                        soundRelay.PlayAudioClip(0);
                        watKindOfDoor.ColliderToOpenState();

                    }
                    else
                    {
                        LinkedDoor.SetBool("closeTheaterDoor", false);
                        LinkedDoor.SetBool("openTheaterDoor", true);
                        if (triggerable != null) { triggerable.Activate(); }
                        //sound by David
                        soundRelay.PlayAudioClip(0);
                        alternativeDoor.ColliderToOpenState();
                    }
                }

                if (door != null)
                {
                    doOnce = true;
                    // Retire la collision de la porte
                    doorCollClose.enabled = false;
                    refCamera.Lookat(door.transform);
                }
            }
        }
        if (doOnce)
        {
            timeInCabestan += Time.deltaTime;
            if (doOnce && triggerableLookAt != null && timeInCabestan >= 3f)
            {
                triggerableLookAt.Activate();
                doOnce = false;
            }
        }
    }

    public void AnimateAvatar()
    {
        avatar.stateMachine.interact.interactTime = 4f;
        avatar.stateMachine.ChangeState(avatar.stateMachine.interact);
        avatar.tr.position = closest;

        //Rotation
        Vector3 lookAtPos = transform.position;
        lookAtPos.y = avatar.tr.position.y;
        avatar.tr.LookAt(lookAtPos);

        avatar.tr.position += avatar.tr.right * avatarOffsetRight + avatar.tr.forward * avatarOffsetForward;

        avatar.animator.SetTrigger("wheelOfPain");
    }



}
