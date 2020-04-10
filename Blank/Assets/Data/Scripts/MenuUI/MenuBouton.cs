//Creer par Valentin 
//Date de création [2019-04-08]
//Modification Valentin le [2019-04-09]

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.UI;
using UnityEngine.Events;
public class MenuBouton : MonoBehaviour
{
    [SerializeField] MenuBoutonController menuButtonController;
    [SerializeField] Animator animator;
    [SerializeField] AnimatorFunctions animatorFunctions;
    //[Valentin 2019-04-08] Index de l'instance bouton dans le panel.  
    public int thisIndex;  

    //[Valentin 2019-04-08] The Rewired Player et Player ID
    private Player player;
    private int playerID = 0;
    //[Valentin 2019-04-08] Unity event si tu pése a ca selectionne comme un click souris
    public UnityEvent OnButtonPress = new UnityEvent();
    //[Valentin 2019-04-08] Unity event, si tu click ca retourne un int pour savoir quel level tu load.
    public UnityIntEvent OnButtonPressInt = new UnityIntEvent();    
    public int levelToLoad;
    [HideInInspector]
    public bool onPressed, onSelect;
    [HideInInspector]


    private void Awake()
    {
        //TO DO : ADD REWIRED
        player = ReInput.players.GetPlayer(playerID);
    }
    // Update is called once per frame
    void Update()
    {
        onPressed = player.GetButtonDown("actionButton");
        if(menuButtonController != null)
            onSelect = menuButtonController.index == thisIndex;
        AnimationUpdate();
        OnPress();
    }

    void AnimationUpdate()
    {
        animator.SetBool("pressed",  onSelect && onPressed);
        animator.SetBool("selected", onSelect);
    }

    public virtual void OnPress()
    {
        //Set les booleans et synchronise l'etat des boutons avec l'animator
        if (onSelect && onPressed)
            OnButtonPress.Invoke();
    }

    public class UnityIntEvent : UnityEvent<int> { }
}
