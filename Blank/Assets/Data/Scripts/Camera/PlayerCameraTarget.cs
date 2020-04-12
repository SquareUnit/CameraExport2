using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraTarget : MonoBehaviour
{
    public Transform tr;
    public PlayerCamera user;
    private float baseHeight;
    private Vector3 animRootPos;
    public float verticalDolly;
    private RaycastHit sphereCastHit;
    private float sphereCastRadius = 0.4f;
    private float sphereCastDistance = 0.65f;

    [Range(-1.5f, 1.5f)] public float heightAdjustment;
    private Vector3 forwardOffset;
    private float forwardOffsetVal = 0.35f;
    private Vector3 yOffset;

    public Vector3 targetPosSV;
    private float SmoothMaxSpeed = 25.0f;
    private Color debugColor;

    public Vector3 revealTargetPos;
    public Vector3 revealInitPos;
    public float revealLerpTime;
    public float t;
    public float tStamp;

    public void Start()
    {
        DontDestroyOnLoad(gameObject);
        tr = transform;
        baseHeight = GameManager.instance.currentAvatar.GetComponent<CharacterController>().height;
        tr.position = new Vector3(tr.position.x, tr.position.y + baseHeight, tr.position.z);
    }

    public void LateUpdate()
    {
        if (GameManager.instance.currentAvatar == enabled && GameManager.instance.currentAvatar != null)
        {
            animRootPos = GameManager.instance.currentAvatar.animator.rootPosition;

            SetOffsets();

            if (user.camFSM.currentState != user.revealState)
            {
                SetRotation();
                SetPosition();
            }
            else
            {
                SetCinematicPosition();
            }
        }
    }

    private void SetOffsets()
    {
        /// Forward offset value
        forwardOffset = tr.forward * forwardOffsetVal;
        /// Define a desired height for camTarget position
        if (GameManager.instance.currentAvatar.stateMachine.currentState != GameManager.instance.currentAvatar.stateMachine.crouch)
        {
            yOffset.y = baseHeight + heightAdjustment + verticalDolly;
        }
        else
        {
            yOffset.y = baseHeight + heightAdjustment - 0.75f;
        }
    }

    /// <summary> Pass the info down to those who needs it </summary>
    private void SetRotation()
    {
        tr.rotation = GameManager.instance.currentAvatar.tr.rotation;
    }

    // Position handling if the camera is not in reveal state
    private void SetPosition()
    {
        // If there is an object in front of avatar, do not offset camTarget forward or will will be on the other side of the wall
        if (Physics.SphereCast(animRootPos + yOffset, sphereCastRadius, tr.forward, out sphereCastHit, sphereCastDistance, user.ObstaclesLayerMask))
        {
            tr.position = Vector3.SmoothDamp(tr.position, animRootPos + yOffset, ref targetPosSV, 0.06f, SmoothMaxSpeed);
            debugColor = Color.red;
        }
        else
        {
            tr.position = Vector3.SmoothDamp(tr.position, animRootPos + yOffset + forwardOffset, ref targetPosSV, 0.06f, SmoothMaxSpeed);
            debugColor = Color.green;
        }
    }

    // Position handling if the camera is in cinematic state
    private void SetCinematicPosition()
    {
        if (!user.revealState.revealStartDone)
        {
            t = Mathf.SmoothStep(0.0f, 1.0f, (Time.time - tStamp) / revealLerpTime);
            tr.position = Vector3.Lerp(revealInitPos, revealTargetPos, t);
        }

        if (user.revealState.revealPauseDone)
        {
            t = Mathf.SmoothStep(0.0f, 1.0f, (Time.time - tStamp) / (revealLerpTime / 2));
            tr.position = Vector3.Lerp(revealTargetPos, animRootPos + yOffset + forwardOffset, t);
        }

        debugColor = Color.green;
    }

    private void OnDrawGizmos()
    {
        {
            Debug.DrawRay(tr.position, tr.forward * sphereCastDistance, debugColor);
            Gizmos.color = debugColor;
            Gizmos.DrawWireSphere(sphereCastHit.point, sphereCastRadius);
            Gizmos.DrawSphere(tr.position, 0.05f);
        }
    }

    public RaycastHit SphereCastHit 
    {
        get { return sphereCastHit; }
    }
}