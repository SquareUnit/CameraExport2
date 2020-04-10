using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent),typeof(Rigidbody))]

public class NmeController : MonoBehaviour
{

    #region Variable State Machine
    public enum NmeStates
    {
        IDLE, CHASE, SEARCH, DEFEND
    }
    private NmeStateMachine currentState;
    private NmeStateChase chaseState;
    private NmeStateIdle idleState;
    private NmeStateSearch searchState;
    
    #endregion

    #region Variable Standard
    [Header("Drag the GameManager Here")]
    [Header("")]
    public GameManager gameManager;
    protected Transform tr;
    protected Transform target;
    protected NavMeshAgent agent;
    [Header("Ajust the size of a enemy detection range")]
    [Header("")]
    [Range(2.0f, 30f)]
    public float lookRadius = 10f;
    [Header("Ajust the speed of a enemy")]
    [Header("")]
    [Range(0.1f, 10f)]
    public float speed;
    #endregion

    #region Variable WayPoints ( TEMPORAIRE )
    [Header("Set number of Waypoint, Drag Empty object to set points")]
    [Header("")]
    public Transform[] wayPoints;
    private int destPoint = 0;
    #endregion

    #region Start : Get,Target Avatar, StateMachine
    protected void Start()
    {
        tr = this.transform;
        agent = GetComponent<NavMeshAgent>();
        target = gameManager.currentAvatar.transform; // TARGET AVATAR
        // State Machine
        chaseState = new NmeStateChase(this);
        idleState = new NmeStateIdle(this);
        searchState = new NmeStateSearch(this);
        
        currentState = idleState;
        agent.speed = speed;
    }
    #endregion

    #region Nme Mouvement, WayPoints Path
    protected void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position); // GET AVATAR POSITION
        // NME CHASE AVATAR
        if (distance <= lookRadius)
        {
            agent.SetDestination(target.position);
            FaceTarget();
        }
        // Call Fonction Next Waypoint Destination
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            GotoNextPoint();
    }
    #endregion

    #region Fonction FaceTarget ( Nme will face avatar )
    void FaceTarget()
    {
        Vector3 direction = (target.position - tr.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        tr.rotation = Quaternion.Slerp(tr.rotation, lookRotation, Time.deltaTime * 5f);
    }
    #endregion

    #region Draw Gizmo Nme Radius Detection
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
    #endregion

    #region Fonction GotoNextPoint ( Nme go to next waypoint )
    void GotoNextPoint()
    {
        if (wayPoints.Length == 0)
            return;

        agent.destination = wayPoints[destPoint].position;

        destPoint = (destPoint + 1) % wayPoints.Length;
    }
    #endregion

    #region StateMachine
    public void ChangeState(NmeStates nextState)
    {
        switch (nextState)
        {
            case NmeStates.IDLE:
                currentState = idleState;
                break;
            case NmeStates.CHASE:
                currentState = chaseState;
                break;
            case NmeStates.SEARCH:
                currentState = searchState;
                break;

        }
    }
    #endregion
}
