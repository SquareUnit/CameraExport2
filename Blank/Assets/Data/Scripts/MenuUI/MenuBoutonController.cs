//Creer par Valentin 
//Date de création [2019-04-08].
//Modification Valentin le [2019-04-09]

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Rewired;

public class MenuBoutonController : MonoBehaviour
{
    //[valentin 2019-04-05] donne un index a chaque bouton
    public int index;
    private bool keyDown;
    //[valentin 2019-04-05] Maximum d'index, chaque bouton à un index en partant de l'index 0. index = id du bouton
    [SerializeField] int maxIndex;
    public AudioSource audioSource;

    //[Valentin 2019-04-08] The Rewired Player et Player ID
    private Player player;
    public int playerID = 0;

    private void Awake()
    {
        player = ReInput.players.GetPlayer(playerID);
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    
    void Update()
    {
        //[valentin 2019-04-08] Si t'as pesé sur le pitons ca bouge sinon ca bouge pô
        if (player.GetAxis("vertical") != 0)        
            VerticalMove();        
        else
            keyDown = false;
    }

    //[valentin 2019-04-05] Fonction de navigation vertical dans le menu
    void VerticalMove()
    {
        if (!keyDown)
        {
            if (player.GetAxis("vertical") < 0)            
                index = (index < maxIndex ? index + 1 : 0);            
            else if (player.GetAxis("vertical") > 0)            
                index = (index > 0 ? index - 1 : maxIndex);
            keyDown = true;
        }
    }   
    
}


