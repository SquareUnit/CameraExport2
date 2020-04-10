using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestaltManagerLvl9 : MonoBehaviour {
    public ValveLvl09[] valveIsActive;



    public PenseeBlankText penseeBlank;
    public AudioSource audiosource;

    public Triggerable triggerable;

    public bool win = false;

    // Start is called before the first frame update
    void Start()
    {
        if (triggerable != null)
        {
            foreach (ValveLvl09 valve2 in valveIsActive)
            {
                valve2.myManager = this;
            }

        }
    }

    public void Check()
    {
        int cpt = 0;

        if (triggerable != null)
        {
            foreach (ValveLvl09 valve2 in valveIsActive)
            {
                if (valve2.isWinning == true)
                {
                    cpt++;
                }
                if (cpt == valveIsActive.Length)
                    win = true;
                else
                    win = false;

            }         

            PreWin();
        }

    }
    public void PreWin()
    {
        if (win)
        {
            Invoke("Win", 2);
        }
    }

    public void Win()
    {
        penseeBlank.gameObject.SetActive(false);
        triggerable.Activate();
        audiosource.PlayOneShot(SoundManager.instance.audioClipDict["sfx_SuccessA"]);
    }
    
}