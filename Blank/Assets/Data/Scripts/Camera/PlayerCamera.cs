///Creation date : 03-04-19
///Par: Felix Desrosiers-Dorval

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PlayerCamera : MonoBehaviour
{
    public Transform tr;
    [HideInInspector] public new Camera camera;
    [HideInInspector] public PlayerCameraTarget playerCamTarget;
    [HideInInspector] public PlayerCameraTarget camTarget;
    public string[] camObstacleLayers = new string[3];
    private LayerMask obstaclesLayerMask;
    private bool loadingDone = false;

    [HideInInspector] public StateMachine camFSM;
    [HideInInspector] public PlayerCamOnLoad onLoadState;
    [HideInInspector] public PlayerCamDefault defaultState;
    [HideInInspector] public PlayerCamCollision collisionState;
    [HideInInspector] public PlayerCamFall fallingState;
    [HideInInspector] public PlayerCamReset resetState;
    [HideInInspector] public PlayerCamReveal revealState;
    [HideInInspector] public PlayerCamValve valveState;

    [HideInInspector] public CharacterState avatarFSM;

    public float camFOV = 60.0f;
    public float yaw, pitch;
    [NonSerialized] public float smoothTime = 0.022f;
    private float yawSensitivity = 126.0f;
    private float yawAutoSensitivity = 68.0f;
    private float pitchSensitivity = 110.0f;

    public float pitchMin = -25.0f;
    public float pitchMax = 60.0f;

    private Vector3 currRotation, desiredRotation;
    [HideInInspector] public Vector3 rotationSV;
    private float a1, b1, a2, b2;

    // Camera dolly params
    [HideInInspector] public float desiredDollyDst;
    [HideInInspector] public float camDollyMinDist = 2.4f;
    [HideInInspector] public float camDollyMaxDist = 5.0f;

    // Camera height Offset params
    private float desiredYOffset;
    private float camTargetMinYOffset = 0.0f;
    private float camTargetMaxYOffset = 0.15f;

    // Collision params
    private Vector3 dirToCamera;
    private Collider lastCollFound;
    [HideInInspector] public RaycastHit hit;
    [HideInInspector] public float desiredCollOffset = 0.5f;
    public bool isColliding;

    // Sideray's params
    private RaycastHit hitRight, hitLeft;

    // Other
    [HideInInspector] public bool camAxisInUse;
    public bool stateDebugLog;
    public bool raycastsDebug = true;


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        tr = transform;
        camera = GetComponent<Camera>();
        camera.fieldOfView = camFOV;
        avatarFSM = GameManager.instance.currentAvatar.GetComponent<CharacterState>();
        obstaclesLayerMask = LayerMask.GetMask(camObstacleLayers[0], camObstacleLayers[1], camObstacleLayers[2]);
        SetCamDollyParams();

        camFSM = GetComponent<StateMachine>();
        onLoadState = new PlayerCamOnLoad(this);
        defaultState = new PlayerCamDefault(this);
        collisionState = new PlayerCamCollision(this);
        fallingState = new PlayerCamFall(this);
        resetState = new PlayerCamReset(this);
        revealState = new PlayerCamReveal(this);
        valveState = new PlayerCamValve(this);

        StartCoroutine(WaitForLoading(3.0f));
    }

    /// <summary> Wait until loading is done, then fetch player, create camTarget and start machine</summary>
    private IEnumerator WaitForLoading(float delay)
    {
        yield return new WaitForSeconds(delay);
        camTarget = Instantiate(playerCamTarget, GameManager.instance.spcLvlMan.playerStart, GameManager.instance.spcLvlMan.playerStartRotation);
        camTarget.user = this;
        camFSM.ChangeState(defaultState);
        yield return new WaitForEndOfFrame();
        loadingDone = true;
    }

    private void LateUpdate()
    {
        if (loadingDone)
        {
            UpdateCamera();
            camFSM.CheckIfStateChange();
            camFSM.CurrentStateUpdate();
        }
    }

    private void UpdateCamera()
    {
        if(InputsManager.instance.cameraInputsAreDisabled == false) SetYawAndPitch();
        CamTargetVerticalDolly();
        CameraOrientation();
        CameraForwardBackwardDolly();
        IsCamColliding();
        CameraSideRays(3.0f);
    }

    private void SetYawAndPitch()
    {
        // If camera stick not in use, use yaw for cam movements. Else, use camera stick.
        if (InputsManager.instance.rightStick.x > 0)
        {
            yaw += InputsManager.instance.rightStick.x * yawSensitivity * Time.deltaTime;
        }
        if (InputsManager.instance.rightStick.x < 0)
        {
            yaw -= Mathf.Abs(InputsManager.instance.rightStick.x) * yawSensitivity * Time.deltaTime;
        }

        if (InputsManager.instance.rightStick.x == 0 && InputsManager.instance.leftStick.x >= 0.25)
        {
            yaw += InputsManager.instance.leftStick.x * yawAutoSensitivity * Time.deltaTime;
        }
        if (InputsManager.instance.rightStick.x == 0 && InputsManager.instance.leftStick.x <= -0.25)
        {
            yaw -= Mathf.Abs(InputsManager.instance.leftStick.x) * yawAutoSensitivity * Time.deltaTime;
        }

        if (camFSM.currentState != revealState)
        {

            if (yaw > 360.0f)
            {
                yaw -= 360.0f;
                currRotation = new Vector3(currRotation.x, currRotation.y - 360, currRotation.z);
            }
            if (yaw < 0.0f)
            {
                yaw += 360.0f;
                currRotation = new Vector3(currRotation.x, currRotation.y + 360, currRotation.z);
            }
        }

        // Setup pitch and clamp it
        pitch -= InputsManager.instance.rightStick.y * pitchSensitivity * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);
    }

    /// <summary> Dolly the cam target upward or downward depending on the camera pitch. Linear function start dollying below 0 </summary>
    private void CamTargetVerticalDolly()
    {
        desiredYOffset = a2 * pitch + b2;
        camTarget.verticalDolly = Mathf.Clamp(desiredYOffset, camTargetMinYOffset, camTargetMaxYOffset);
    }

    ///<summary> Set up the camera orientation </summary>
    private void CameraOrientation()
    {
        desiredRotation = new Vector3(pitch, yaw, 0.0f);
        currRotation = Vector3.SmoothDamp(currRotation, desiredRotation, ref rotationSV, 0.11f);
        tr.eulerAngles = currRotation;
    }

    /// <summary> Dolly the camera forward or backward depending on the pitch. Linear function start dollying below 0 </summary>
    private void CameraForwardBackwardDolly()
    {
        desiredDollyDst = a1 * pitch + b1;
        desiredDollyDst = Mathf.Clamp(desiredDollyDst, camDollyMinDist, camDollyMaxDist);
    }

    /// <summary> Verify if the player is actively manipulating the camera with the right stick axis </summary>
    /// NOT CURRENTLY USED
    private void IsCameraBeingUsed()
    {
        if (InputsManager.instance.rightStick.x == 0 && InputsManager.instance.rightStick.y == 0) camAxisInUse = false;
        else camAxisInUse = true;
    }

    /// <summary> Check if there is a valid collision </summary>
    private void IsCamColliding()
    {
        dirToCamera = tr.position - camTarget.tr.position;
        if (raycastsDebug) Debug.DrawRay(camTarget.transform.position, dirToCamera, Color.gray);
        //if (Physics.SphereCast(camTarget.tr.position, 0.10f,  dirToCamera, out hit, camDollyMaxDist, obstaclesLayerMask)) TODO: Integrate sphere cast
        if (Physics.Raycast(camTarget.tr.position, dirToCamera, out hit, camDollyMaxDist * 2, obstaclesLayerMask))
        {
            float product = Vector3.Dot(hit.normal, Vector3.up);
            if (product <= 0.3 && product >= -0.3) //TODO : Handle ceilings(|| hit.normal.y < 0))
            {
                IsCollisionValid();
            }
            else isColliding = false;
        }
        else // If raycast not picking up anything
        {
            isColliding = false;
        }
    }

    /// <summary> Check if the collsion distance is small enough for it to be considered valid for camera wall hoovering behaviour</summary>
    public void IsCollisionValid()
    {
        Vector3 hitToCamTarget = camTarget.tr.position - hit.point;
        hitToCamTarget.y = 0.0f;
        float hitAngle = Vector3.Angle(hit.normal, hitToCamTarget);
        float hitToCamDist = Vector3.Distance(hit.point, camTarget.tr.position) - desiredDollyDst;
        float wallToCamDist = Mathf.Sin(Mathf.Deg2Rad * hitAngle) * hitToCamDist;

        if (wallToCamDist < desiredCollOffset) isColliding = true;
        else isColliding = false;
    }

    /// <summary> Cast rays parallel to the camera forward vector. Nudge the camera yaw in the opposite direction if one or multiple collides. </summary>
    private void CameraSideRays(float sideRayCount)
    {
        if (InputsManager.instance.PlayerIsMovingAvatar && !isColliding) 
        {
            Vector3 rayOrigin;
            Vector3 rayDir;
            float latMovSpd = Mathf.Abs(InputsManager.instance.leftStick.x);
            float correctionSpd = 35;
            float rayMaxOffset = 1.2f;

            for(int i = 1; i <= sideRayCount; i++)
            {
                // Rays position and yaw adjustment str
                float raysLateralOffsets = latMovSpd * rayMaxOffset * i/sideRayCount;
                float yawAdjustment = (1 + latMovSpd) * correctionSpd * i/sideRayCount;

                // Right parallel ray
                rayOrigin = tr.position + (raysLateralOffsets * tr.right);
                rayDir = camTarget.tr.position - tr.position - (tr.forward * i/sideRayCount);
                float raylenght =  rayDir.magnitude;

                if (Physics.Raycast(rayOrigin, rayDir, out hitRight, raylenght, obstaclesLayerMask))
                {
                    if (raycastsDebug) Debug.DrawRay(rayOrigin, rayDir, Color.red);
                    yaw += yawAdjustment * Time.deltaTime;
                }
                else if (raycastsDebug) Debug.DrawRay(rayOrigin, rayDir, Color.green);

                // Left parallel ray
                rayOrigin = tr.position - (raysLateralOffsets * tr.right);
                rayDir = camTarget.tr.position - tr.position - (tr.forward * i / sideRayCount);

                if (Physics.Raycast(rayOrigin, rayDir, out hitLeft, raylenght, obstaclesLayerMask))
                {
                    if (raycastsDebug) Debug.DrawRay(rayOrigin, rayDir, Color.red);
                    yaw -= yawAdjustment * Time.deltaTime;
                }
                else if (raycastsDebug) Debug.DrawRay(rayOrigin, rayDir, Color.green);
            }
        }
    }

    /// <summary> Determin how the forward/backward dolly and vertical offset will move the camera, depending on the parameters </summary>
    private void SetCamDollyParams()
    {
        // Forward/backward dolly
        a1 = (camDollyMinDist - camDollyMaxDist) / pitchMin;
        b1 = camDollyMaxDist;

        // Upward/Downward setup
        a2 = (camTargetMaxYOffset - camTargetMinYOffset) / pitchMin;
        b2 = camTargetMinYOffset;
    }

    public LayerMask ObstaclesLayerMask 
    {
        get { return obstaclesLayerMask; }
    }
}

// Tool used to detect the nature of what the camera target is colliding with in front of the avatar.
#if UNITY_EDITOR
[CustomEditor(typeof(PlayerCamera))]
public class PlayerCameraDebugTool : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        PlayerCamera user = (PlayerCamera)target;

        if (GUILayout.Button("Get collision name"))
        {
            Debug.Log("Object " + user.camTarget.SphereCastHit.collider.transform.root.name + " is root of collision");
            if(user.camTarget.SphereCastHit.collider.transform.parent != null)
            {
                Debug.Log(user.camTarget.SphereCastHit.collider.transform.parent.name + " : is direct parent");
            }
            Debug.Log("Hitting the layer number " + user.camTarget.SphereCastHit.collider.gameObject.layer.ToString());
        }
    }
}
#endif