// Créé par: Lucien Lehmann
// En date du: 03-05-19
// Modifié par: David Babin
// En date du: 09/07/19 : Refactorisation et optimisation
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PantinInfo : MonoBehaviour
{
    public Transform tr;
    public Transform camLocTr;

    public bool isActivate = false;
    public SkinnedMeshRenderer skinMeshRD;
    public SpriteRenderer interactButton;
    public SoundRelay soundRelay;

    public enum pantinPosition { lookingOut, downWard, searchingAround, sitting };
    public pantinPosition startPosition;
    public Animator myAnimator;
    private int currentPosiTab = 0;
    private bool errorFound;
    public bool canPlaySound = true;
    private bool doOnce = true;

    private void Awake()
    {
        tr = transform;
    }
    private void Start()
    {
        GameManager.instance.spcLvlMan.pantinInMap.Add(this);
        interactButton = GetComponentInChildren<SpriteRenderer>();
        skinMeshRD = GetComponentInChildren<SkinnedMeshRenderer>();
        soundRelay = GetComponent<SoundRelay>();
        interactButton.gameObject.SetActive(false);
        myAnimator = GetComponent<Animator>();

        switch (startPosition)
        {
            case pantinPosition.lookingOut:
                myAnimator.SetBool("pose01", true);
                break;
            case pantinPosition.downWard:
                myAnimator.SetBool("pose02", true);
                break;
            case pantinPosition.searchingAround:
                myAnimator.SetBool("pose03", true);
                break;
            case pantinPosition.sitting:
                myAnimator.SetBool("pose04", true);
                break;
        }
    }


    private void Update()
    {
        // look at for the button
        if (interactButton.gameObject.activeInHierarchy)
        {
            interactButton.transform.LookAt(GameManager.instance.currentCamera.tr);
        }

        // Afficher le bouton interact (X)
        if (isActivate)
        {
            //affiche bouton
            interactButton.gameObject.SetActive(true);

        }
        else
        {
            interactButton.gameObject.SetActive(false);
        }

        if (!GetComponentInChildren<SkinnedMeshRenderer>().isVisible)
        {
            interactButton.gameObject.SetActive(false);
        }

        if (doOnce && GameManager.instance.spcLvlMan.pantinInMap.Count <= 1)
        {
            for (int i = 0; i < GameManager.instance.spcLvlMan.pantinInMap.Count - 1; i++)
            {
                if (GameManager.instance.spcLvlMan.pantinInMap[currentPosiTab] == transform.gameObject)
                {
                    errorFound = false;
                    break;
                }
                else
                {
                    errorFound = true;
                    currentPosiTab++;
                }
            }

            if (errorFound)
            {
                GameManager.instance.spcLvlMan.pantinInMap.Add(this);
            }
            doOnce = false;
        }

        //sound by David
        if (canPlaySound)
        {
            //soundRelay.PlayAudioClip(0);
            canPlaySound = false;
            Invoke("ResetCanPlaySound", Random.Range(4f, 8f));
        }
    }

    public void ResetCanPlaySound()
    {
        canPlaySound = true;
    }
}
