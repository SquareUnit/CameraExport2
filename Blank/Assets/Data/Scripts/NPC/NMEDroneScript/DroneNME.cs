using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(StateMachine))]
public class DroneNME : MonoBehaviour
{
    public enum BaseState { patrol, idle}
    public BaseState baseState;

    // Parameters
    [HideInInspector] public Transform tr;
    [HideInInspector] public Transform avatarTr;
    [HideInInspector] public Vector3 dirToAvatar;
    [HideInInspector] public SoundRelay soundRelay;
    private bool loadingDone;
    private bool doOnce = true;
    [HideInInspector] public bool warned = true;
    private float distanceToPlayer;
    private float droneHeightOffset = 3.5f;
    [HideInInspector] public Vector3 desiredHeightOffset = new Vector3(0, 0, 0);
    [Range(0.1f, 4.0f)] public float moveSmoothTime = 1;
    [Range(1.0f, 10.0f)] public float moveMaxSpd = 4;
    public bool isIdle = false;

    // State Machine
    private StateMachine machine;
    private DroneIdle sDroneIdle;
    private DroneLookAround sDroneLookAround;
    private DronePatrol sDronePatrol;
    private DroneAttackChargeUp sDroneAttackChargeUp;
    private DroneAttack sDroneAttack;
    private DroneReset sDroneReset;

    // Anim&FX
    private Animator droneAnimator;
    [HideInInspector] public MeshRenderer spotLightHalo;
    [HideInInspector] public LineRenderer spotLightFlare;
    [HideInInspector] public Light spotLight;
    [HideInInspector] public float baseLightAngle = 70.0f;
    [HideInInspector] public float targetLightAngle = 40.0f;
    [HideInInspector] public float baseLightIntensity = 5.0f;
    [HideInInspector] public float targetLightIntensity = 20.0f;
    [HideInInspector] public float baseIndirectMult = 1.0f;
    [HideInInspector] public float targetIndirectMult = 3.0f;
    [HideInInspector] public Color baseSpotLightColor = Color.white;
    [HideInInspector] public Color targetSpotLightColor = new Vector4(1.0f, 0.15f, 0.15f, 1.0f);
    [HideInInspector] public Color baseFlareColor = Color.white;
    [HideInInspector] public Color targetFlareColor = new Vector4(1.0f, 0.15f, 0.15f, 1.0f);
    [HideInInspector] public Color baseHaloColor = Color.white;
    [HideInInspector] public Color targetHaloColor = new Vector4(1.0f, 0.15f, 0.15f, 1.0f);
    [HideInInspector] public ParticleSystem droneDetection, droneDistortion;
    public Transform droneChargeTop;
    public Transform droneChargeLeft;
    public Transform droneChargeRight;

    [HideInInspector] public LineRenderer droneLazer;

    // Pathing
    [HideInInspector] public NMEFollow nmeFollow;
    [HideInInspector] public Lerpable lerpable;
    [HideInInspector] public GameObject patrolTarget;

    // Other
    [HideInInspector] public Transform targetPlayerTr;
    private float fovForwardAngle;

    // Detection
    [HideInInspector] public DroneDetectionCone detectionCone;
    [HideInInspector] public bool playerIsVisible;

    private LayerMask obstaclesMask;
    private LayerMask playerObstacleMask;
    private RaycastHit checkForPlayer;
    private RaycastHit groundCheckHit;

    public bool stateDebugOn = true;
    public int counter;

    void Awake()
    {
        nmeFollow = GetComponent<NMEFollow>();
        lerpable = GetComponent<Lerpable>();
        soundRelay = GetComponent<SoundRelay>();
        droneAnimator = GetComponent<Animator>();
        detectionCone = GetComponentInChildren<DroneDetectionCone>();
        detectionCone.transform.localScale += new Vector3(3f, 3f, 4f);
        spotLight = GetComponentInChildren<Light>();
        obstaclesMask = LayerMask.GetMask("Obstacles");
        playerObstacleMask = LayerMask.GetMask("Obstacles", "Player");

        StopAllFX();
    }

    private void Start()
    {
        tr = transform;
        machine = GetComponent<StateMachine>();
        sDroneIdle = new DroneIdle(this);
        sDroneLookAround = new DroneLookAround(this);
        sDronePatrol = new DronePatrol(this, moveSmoothTime, moveMaxSpd);
        sDroneAttackChargeUp = new DroneAttackChargeUp(this);
        sDroneAttack = new DroneAttack(this);
        sDroneReset = new DroneReset(this);
        Invoke("PlayLoopingSFXOneShot", 2.0f);
        GameManager.instance.callReset.AddListener(ResetOnDeath);
        StartCoroutine(WaitForLoading(2.0f, isIdle));
    }

    /// <summary> Wait until loading is done, then fetch player, create camTarget and start machine</summary>
    private IEnumerator WaitForLoading(float delay, bool isIdle)
    {
        yield return new WaitForSeconds(delay);
        avatarTr = GameManager.instance.currentAvatar.tr;
        if (isIdle)
            Machine.ChangeState(sDroneIdle);
        else
            Machine.ChangeState(sDronePatrol);
        loadingDone = true;
    }

    private void Update()
    {
        if(loadingDone)
        {
            SndWarnPlayerOnce();
            machine.CheckIfStateChange();
            machine.CurrentStateUpdate();
            counter++;
        }
    }

    public void ResetOnDeath()
    {
        machine.ChangeState(sDroneReset);
    }

    /// <summary> Stop all FXs before we start the required one only</summary>
    public void StopAllFX()
    {
        droneDetection.Stop();
        droneDistortion.Stop();
    }

    private void SndWarnPlayerOnce()
    {
        if (!warned && counter % 15 == 0)
        {
            distanceToPlayer = Vector3.Distance(tr.position, avatarTr.position);
            if (distanceToPlayer <= 10.0f)
            {
                warned = true;
                soundRelay.PlayAudioClip(4);
            }
        }
    }

    /// <summary> All logic that is common to all states</summary>
    public void DroneUpdateState()
    {
        FindDesiredHeight();
        ResolveDetection();
    }

    private void FindDesiredHeight()
    {
        if (Physics.Raycast(tr.position, -Vector3.up, out groundCheckHit, 100.0f, obstaclesMask))
        {
            float distToGround = Vector3.Distance(tr.position, groundCheckHit.point);
            Debug.DrawLine(tr.position, groundCheckHit.point, Color.white);
            desiredHeightOffset = new Vector3(0, droneHeightOffset - distToGround, 0);
        }
    }

    private void ResolveDetection()
    {
        if (detectionCone.playerInRange && GameManager.instance.currentAvatar.stateMachine.currentState != GameManager.instance.currentAvatar.stateMachine.death)
        {
            dirToAvatar = new Vector3(avatarTr.position.x, avatarTr.position.y + GameManager.instance.avatarHeight / 2, avatarTr.position.z) - tr.position;
            if (Physics.Raycast(tr.position, dirToAvatar, out checkForPlayer, 100.0f, playerObstacleMask) && checkForPlayer.collider.tag == "Player")
            {
                playerIsVisible = true;
            }
            else playerIsVisible = false;
        }
        else playerIsVisible = false;
    }

    public void PlayFx(AnimationEvent animation)
    {
        GameManager.instance.fxPool.GetObjectAutoReturn(animation.stringParameter, tr.position, tr.eulerAngles, animation.floatParameter);
    }

    public void PlayLoopingSFXOneShot()
    {
        soundRelay.PlayAudioClip(5);
    }

    public void PlayFxTop(AnimationEvent animation)
    {
        GameManager.instance.fxPool.GetObjectAutoReturn(animation.stringParameter, droneChargeTop.transform.position, tr.eulerAngles, animation.floatParameter);
    }

    public void PlayFxSide(AnimationEvent animation)
    {
        GameManager.instance.fxPool.GetObjectAutoReturn(animation.stringParameter, droneChargeLeft.transform.position, tr.eulerAngles, animation.floatParameter);
        GameManager.instance.fxPool.GetObjectAutoReturn(animation.stringParameter, droneChargeRight.transform.position, tr.eulerAngles, animation.floatParameter);
    }

    #region ACCESSORS

    public Animator DroneAnimator {
        get { return droneAnimator; }
    }
    public float DroneHeightOffset {
        get { return droneHeightOffset; }
    }
    public StateMachine Machine {
        get { return machine; }
    }
    public DroneIdle DroneIdleState {
        get { return sDroneIdle; }
    }
    public DroneLookAround DroneLookAroundState {
        get { return sDroneLookAround; }
    }
    public DronePatrol DronePatrolState {
        get { return sDronePatrol; }
    }
    public DroneAttackChargeUp DroneAttackChargeUpState {
        get { return sDroneAttackChargeUp; }
    }
    public DroneAttack DroneAttackState {
        get { return sDroneAttack; }
    }
    public DroneReset DroneResetState {
        get { return sDroneReset; }
    }

    #endregion

    #region Events Functions
    public void EventChangeDroneState(int state)
    {
        switch(state)
        {
            case 0:
                Machine.ChangeState(DroneIdleState);
                break;
            case 1:
                Machine.ChangeState(DronePatrolState);
                break;
            case 2:
                Machine.ChangeState(DroneResetState);
                break;
            default:
                Debug.LogError("GameObject : " + gameObject.name + " unity event change state has an unexpected value. 0 is idle, 1 is patrol and 2 is reset");
                break;
        } 
    }

    public void EventDestroyDrone() 
    {
        Destroy(gameObject);
    }

    public void EventDestroyDronePath()
    {
        Destroy(nmeFollow.path);
    }

    public void EventDestroyDroneAndPath()
    {
        Destroy(gameObject);
        Destroy(nmeFollow.path);
    }

    public void ToggleLookAround(float idleStateDuration)
    {
        sDroneIdle.duration = idleStateDuration;
        sDroneIdle.lookAround = !sDroneIdle.lookAround;
    }

    public void TogglePatrol(float idleStateDuration)
    {
        sDroneIdle.duration = idleStateDuration;
        sDroneIdle.patrol = !sDroneIdle.patrol;
    }
    #endregion

    private void OnValidate()
    {
        if (sDronePatrol != null)
        {
            sDronePatrol.moveSmoothTime = moveSmoothTime;
            sDronePatrol.moveMaxSpd = moveMaxSpd;
        }
    }
}