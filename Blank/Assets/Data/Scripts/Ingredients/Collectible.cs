using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public SpriteRenderer interactButton;
    public Transform collectiblePlane;
    public DialogueTrigger dialogueTrigger;

    public int collectibleArrayPos;

    private void Start()
    {
        interactButton = GetComponentInChildren<SpriteRenderer>();
        interactButton.gameObject.SetActive(false);
        dialogueTrigger = GetComponent<DialogueTrigger>();

        collectiblePlane = transform.Find("Glyph_Jester_01_LOD0");
    }
    //private void Update()
    //{
    //    // look at for the button
    //    if (interactButton.gameObject.activeInHierarchy) { interactButton.transform.LookAt(GameManager.instance.currentCamera.tr); }
    //    // look at for collectible
    //    if (collectiblePlane != null && GameManager.instance.currentCamera != null)
    //    {
    //        collectiblePlane.LookAt(GameManager.instance.currentCamera.tr);
    //    }
    //}

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            //TODO : Decal de bouton interact
            interactButton.gameObject.SetActive(true);

            if (InputsManager.instance.interact)
            {
                //MusicPlayer.instance.AddLowPassFilters(1.0f);
                Character avatar = GameManager.instance.currentAvatar;

                avatar.stateMachine.interact.interactTime = 0.5f;
                avatar.stateMachine.ChangeState(GameManager.instance.currentAvatar.stateMachine.interact);

                Vector3 lookAtPos = transform.position;

                lookAtPos.y = avatar.tr.position.y;
                avatar.tr.LookAt(lookAtPos);

               // GameManager.instance.collectibleInfos[collectibleArrayPos].collectibleEnabled = true;
               // GameManager.instance.collectibleInfos[collectibleArrayPos].UpdateToggle();

                avatar.animator.SetTrigger("grab");
                dialogueTrigger.StartDialogue();
                gameObject.SetActive(false);
              //  collectiblePlane.gameObject.SetActive(false);

                

            }
        }
    }
}
