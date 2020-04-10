using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(StateMachine))]
public class HeadlessNPC : Triggerable
{
    IStates lastState;

    #region Parameters
    // trigger de reaction
    [HideInInspector] public Collider detectZone;
    [HideInInspector] public Trigger detectTrigger;
    private Randomizer randomizer;
    // path follow du HH
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Transform[] path;
    [HideInInspector] public int pathCpt = 0;

    [Header("Drag the Path script here")]
    public PathObject pathObject;

    // valeur avant de tourner un coin
    [HideInInspector] public float distanceToTurn;

    [HideInInspector] public StateMachine stateMachine;

    public HeadlessIdle idle;
    public HeadlessMove move;
    public HeadlessReact react;
    public HeadlessScared scared;
    public HeadlessEventTrain eventTrain;

    [HideInInspector] public Animator animator;

    [HideInInspector] public Transform tr;

    [HideInInspector] public Vector3 forward;
    private bool enPlace = false;
    [HideInInspector] public HeadlessNPC eventFileSuivant;
    [HideInInspector] public bool isFirstInLine = false;
    [HideInInspector] public Transform backInLineRef;
    [HideInInspector] public bool isInLeftLine = false;
    [HideInInspector] public bool isInFrontOfBlank = false;
    [HideInInspector] public bool isBehindBlank = false;
    
    [Header("Options de comportements")]
    public bool isSitting = false;
    public bool isSittingVar = false;

    [Header("Temps en secondes entre les random anims")]
    public float randomTimeMin = 5;
    public float randomTimeMax = 12;

    #endregion


    void Awake()
    {
       stateMachine = GetComponent<StateMachine>();
    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        randomizer = GetComponent<Randomizer>();

        idle = new HeadlessIdle(this);
        move = new HeadlessMove(this);
        react = new HeadlessReact(this);
        scared = new HeadlessScared(this);
        eventTrain = new HeadlessEventTrain(this);

        tr = this.transform;

        animator = GetComponent<Animator>();
        animator.SetBool("idleVariationAll", true);
        // state de depart
        if (eventFileSuivant != null)
        {
            if (isFirstInLine)
                Invoke("NextPosInLine", 8f);

            eventFileSuivant = eventFileSuivant.GetComponent<HeadlessNPC>();
            stateMachine.ChangeState(eventTrain);
            if(isSitting || isSittingVar ){ Invoke("PlayRandomAnim", Random.Range(randomTimeMin, randomTimeMax)); }
        }    
        else
            stateMachine.ChangeState(idle);

        if (isInFrontOfBlank || isBehindBlank)
        {
            stateMachine.ChangeState(eventTrain);
            Invoke("GetBlank", 5f);
            if (isSitting || isSittingVar) { Invoke("PlayRandomAnim", Random.Range(randomTimeMin, randomTimeMax)); }
        }

        // Get the trigger zone component
        detectTrigger = detectZone.GetComponent<Trigger>();

        // null reference for path, for Stationnary Headless
        if (pathObject != null)
        {
            GetPath();
            // forward du headless, pour navmesh 
            forward = tr.forward;

            distanceToTurn = 0.5f;

            tr.position = pathObject.wayPoints[0].transform.position;
            stateMachine.ChangeState(move);
        }

        // Comportements assis

        if (isSitting)
        {
            animator.SetBool("IdleVariationBench", true);
        }
        if (isSittingVar)
        {
            animator.SetBool("IdleVariationBench2", true);
        }
    }

    private void Update()
    {
        stateMachine.CurrentStateUpdate();
    }

    /// <summary>
    /// Update le path à la prochaine destination, loop du dernier au premier point.
    /// </summary>
    public void UpdatePath()
    {
        pathCpt++;
        if (pathCpt == path.Length)
            pathCpt = 0;
    }

    /// <summary>
    /// Regarde la destination la plus proche pour retourner en patrol
    /// </summary>
    /// <returns></returns>
    public int CheckClosestDestination()
    {
        int closestDistance = 0;
        for (int i = 0; i < path.Length; i++)
        {
            if (Vector3.Distance(tr.position, path[i].position) < Vector3.Distance(tr.position, path[closestDistance].position))
                closestDistance = i;
        }
        return closestDistance;
    }

    /// <summary>
    /// Convertis le Path tool en destinations pour le navMesh
    /// </summary>
    public void GetPath()
    {
        path = new Transform[pathObject.wayPoints.Count];
        for (int i = 0; i < path.Length; i++)
        {
            path[i] = pathObject.wayPoints[i].transform;
        }
        agent.SetDestination(path[CheckClosestDestination()].position);
    }

    /*
    public void OnAnimatorMove()
    {
        agent.speed = animator.deltaPosition.magnitude / Time.deltaTime;         // set le speed de l'agent du nav mesh egal au speed de l'animation        
    }
    */

    // Lorsque Blank rentre dans la zone de detection du Headless
    public override void Activate()
    {
        if (detectTrigger.activant.CompareTag("Player") && stateMachine.currentState != eventTrain)
        {
            // do the reaction
            lastState = stateMachine.currentState;
            stateMachine.ChangeState(react);
        }
    }

    // Lorsque Blank sort dans la zone de detection du Headless
    public override void Deactivate()
    {
        if (detectTrigger.activant.CompareTag("Player") && stateMachine.currentState != eventTrain)
        {
            // enfaite retourne a son previous state
            stateMachine.ChangeState(lastState);
        }
    }

    public override void Toggle()
    {
        // nothin
    }

    // Fonction pour React State
    public void RandomReact()
    {
        // set a random reaction
        
        int animNumberOne = 0;
        int animFinal = 3; // augmente en fonction du nombre de reaction possible
        int randomNumber = Random.Range(animNumberOne, animFinal+1);

        switch (randomNumber)
        {
            case 0:
                // forward need to face Blank
                animator.SetTrigger("reaction1");  
                break;
            case 1:
                animator.SetTrigger("reaction2"); 
                break;
            case 2:
                animator.SetTrigger("reaction3"); 
                break;
            case 3:
                if (GameManager.instance.currentAvatar.transform.position.x <= tr.position.x)
                {
                    animator.SetTrigger("reaction4");
                }
                else
                {
                    animator.SetTrigger("reaction5");
                }
                break;

        }
    }

    public void NextPosInLine()
    {

        if (isFirstInLine)
        {
            Invoke("AssignFirstInLine", 1f);
            isFirstInLine = false;
            Invoke("ExitLine", 5f);

            if (isInLeftLine)
                animator.SetBool("enter_train_left", true);
            else
                animator.SetBool("enter_train", true);
            
        }

        animator.SetTrigger("stop_idle");

        if (eventFileSuivant != null)
            eventFileSuivant.Invoke("NextPosInLine", Random.Range(0.3f, 0.8f));

        else if (isInFrontOfBlank)
            GameManager.instance.currentAvatar.Invoke("NextPosInLine", Random.Range(0.3f, 0.8f));

        
    }

    public void ExitLine()
    {
        gameObject.SetActive(false);
    }

    public void GetBlank()
    {
        if (isInFrontOfBlank)
            GameManager.instance.currentAvatar.headlessInFront = this;

        else if (isBehindBlank)
            GameManager.instance.currentAvatar.headlessInBack = this;
    }

    public void AssignFirstInLine()
    {
        if (isInFrontOfBlank)
            GameManager.instance.currentAvatar.isFirstInLine = true;
        else
            eventFileSuivant.isFirstInLine = true;
    }

    public void PlayRandomAnim()
    {
        animator.SetTrigger(randomizer.Pick());

        if(animator.GetBool("idleVariationAll"))
            Invoke("PlayRandomAnim", Random.Range(randomTimeMin, randomTimeMax));
    }
}
