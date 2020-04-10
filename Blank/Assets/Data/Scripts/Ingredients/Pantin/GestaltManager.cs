/*Script cree le : 25-04-2019
 * Par: Steven Judge
 * Derniere modification: 06-05-2019
 * Par: Steven Judge & David Babin
 * Description: manager du puzzle de gestalt
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestaltManager : MonoBehaviour
{
    public Triggerable triggerable;
    public PantinGestalt[] pantin;
    public CamLookTriggerable camLook;
    public PlayerCamRevealInfo camReveal;
    public SoundRelay soundRelay;
    public bool puzzleCompleted = false;
    [HideInInspector] public bool utilise = false;
    private int compteur = 0;
    public static int gestaltManID { get; private set; }

    public void Init()
    {
        ++gestaltManID;
        if (triggerable != null)
        {
            foreach (PantinGestalt p in pantin)
            {
                if (p != null)
                {
                    p.myManager = this;
                }
            }
        }
        camLook = GetComponent<CamLookTriggerable>();
        camReveal = GetComponent<PlayerCamRevealInfo>();
    }
    /*
    private void Update()
    {
        if (utilise)
        {
            foreach (PantinGestalt p in pantin)
            {
                p.myValve.puzzleIsDone = true;
            }
        }
    }*/

    public void CheckIfGood()
    {
        if (triggerable != null)
        {
            if (utilise == false)
            {
                foreach (PantinGestalt p in pantin)
                {
                    if (p.goodPosition == true)
                    {
                        compteur += 1;
                    }
                }

                if (compteur == pantin.Length)
                {
                    // sound plus look at piece
                    //DrawFxLineToEachPantinGestalt();
                    Invoke("EndPuzzle", 1f);
                    soundRelay.PlayAudioClip(0);
                    utilise = true;
                    puzzleCompleted = true;

                    Invoke("SolutionFX", 2.0f);

                }
                compteur = 0;
            }
        }
    }
    public void EndPuzzle()
    {
        RevealPiece();
        Invoke("TriggerableActivate", camReveal.lerpTime + 0.5f);
    }

    public void TriggerableActivate()
    {
        triggerable.Activate();
    }

    public void RevealPiece()
    {
        camLook.Activate();
    }


    public void SolutionFX()
    {
        // ajout FX sur pantin
        for (int i = 0; i <= pantin.Length; i++)
        {
            GameManager.instance.fxPool.GetObjectAutoReturn("FX_Pantin_SolvedGlow", pantin[i].myPantinTr.position, 8.0f);
        }
    }
    /*public void DrawFxLineToEachPantinGestalt()
    {
        int nextPantin = 1;
        for (int i = 0; i < pantin.Length; i++)
        {
            if (i == pantin.Length)
            {
                nextPantin = 0;
            }
            PantinGestaltFx temp = pantin[i].myPantinTr.GetComponentInChildren<PantinGestaltFx>();
            PantinGestaltFx temp1 = pantin[i + nextPantin].myPantinTr.GetComponentInChildren<PantinGestaltFx>();
            temp.LRDContact.transform.position = temp1.LRDBase.transform.position;
            temp.myLineRD.enabled = true;            
        }
    }*/
}


