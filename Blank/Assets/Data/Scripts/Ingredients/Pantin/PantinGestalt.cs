/*Script cree le : 25-04-2019
 * Par: David Babin
 * Derniere modification: 04-06-2019 ==== aujout nouvelle position pantin
 * Description: Rotation du pantin lors qu'il est activer
 */
using System;
using UnityEngine;

public class PantinGestalt : MonoBehaviour
{
    //partie par philippe
    [HideInInspector]
    public GameObject bouttonValve;

    [HideInInspector]
    public GestaltManager myManager;
    //orientation en X de départ
    public enum pivotAngle { Plancher, MurZ, MurMoinsZ, MurX, MurMoinsX };
    [Header("Orientation du pantin selon la vue en Y")]
    public pivotAngle anglePivot;

    //Orientation de départ
    public enum startDirection { North, East, South, West, NorthEast, SouthEast, SouthWest, NorthWest };
    [Header("Orientation de départ du pantin")]
    public startDirection orientation;

    //Position de depart du pantin gestalt
    public enum startPosition { triangleFront45, triangleDown60, triangleDownRightCorner60,
        triangleDownLeftCorner60, squareTopLeftCorner90, squareTopRightCorner90, squareFront90,
        squareDownLeftCorner90, squareDownRightCorner90, FL_90, FR_90, FL_45, FR_45, triangle60FrontCenter, triangle60FrontLeft, triangle60FrontRight, triangleLeft45Bottom90, triangleRight45Bottom90
    };
    [Header("Position de depart du pantin")]
    public startPosition positionDepart;

    //orientation apres chaque tour conserver pour savoir si le pantin est dans la bonne direction
    private enum currentDirection { North, East, South, West, NorthEast, SouthEast, SouthWest, NorthWest };
    private currentDirection currentOrientation;

    //orientation finale du pantin
    public enum goodDirection { North, East, South, West, NorthEast, SouthEast, SouthWest, NorthWest };
    [Header("Orientation finale du puzzle")]
    public goodDirection goodOrientation;

    [HideInInspector]
    public bool goodPosition = false;

    private Transform tr;
    private Vector3 startRotation = Vector3.zero;

    //orientation des pantins sur le sol de base
    private Vector3 floor = new Vector3(0, 0, 0);
    //orientation des pantins sur les murs par rapport au y
    private Vector3 wallmoinsZ = new Vector3(0, 0, 0);
    private Vector3 wallMoinsX = new Vector3(0, 90, 0);
    private Vector3 wallZ = new Vector3(0, 180, 0);
    private Vector3 wallX = new Vector3(0, 270, 0);
    [HideInInspector]
    public ValveTriggerable myValve;
    [HideInInspector]
    public Animator myAnimator;
    //[HideInInspector]
    public Transform myPantinTr;

    public bool grandPantin;
    [HideInInspector] public int offSet;


    private void Start()
    {
        if (!grandPantin)
        {
            offSet = 5;
        }
        else
        {
            offSet = 12;
        }
        myValve = GetComponentInChildren<ValveTriggerable>();
        myValve.pantinGestalt = this;
        tr = transform;
        myPantinTr = tr.Find("Pantin");
        myAnimator = GetComponent<Animator>();
        bouttonValve = myPantinTr.Find("bouttonValve").gameObject;
        StartPlacementPantin();
        bouttonValve.SetActive(false); 
    }

    private void Update()
    {
        //Debug.Log(currentOrientation.ToString());
    }

    public void Rotation()
    {
        myAnimator.SetTrigger("turn");
    }

    public void StartTurning()
    {
        PlacementPantin();
        Rotation();
        CheckPuzzleState();
    }

    private void CheckPuzzleState()
    {
        if ((int)currentOrientation == (int)goodOrientation)
        {
            goodPosition = true;
        }
        else
        {
            goodPosition = false;
        }
        if(myManager != null) myManager.CheckIfGood();
    }

    public void ToNextDirection()
    {
        if (myManager != null)
        {
            if (myManager.utilise == false)
            {
                StartTurning();
            }
        }
    }

    private void StartPlacementPantin()
    {
        switch (anglePivot)
        {
            case pivotAngle.Plancher:
                startRotation = floor;
                currentOrientation = currentDirection.North;
                bouttonValve.transform.position = new Vector3(myPantinTr.transform.position.x, myPantinTr.transform.position.y + offSet, myPantinTr.transform.position.z);
                break;
            case pivotAngle.MurZ:
                startRotation = wallZ;
                currentOrientation = currentDirection.North;
                myPantinTr.Rotate(Vector3.left, 270f);
                myPantinTr.Rotate(Vector3.up, startRotation.y);
                bouttonValve.transform.position = new Vector3(myPantinTr.transform.position.x, myPantinTr.transform.position.y, myPantinTr.transform.position.z - offSet);
                break;
            case pivotAngle.MurMoinsZ:
                startRotation = wallmoinsZ;
                currentOrientation = currentDirection.North;
                myPantinTr.Rotate(Vector3.left, 270f);
                bouttonValve.transform.position = new Vector3(myPantinTr.transform.position.x, myPantinTr.transform.position.y, myPantinTr.transform.position.z + offSet);
                break;
            case pivotAngle.MurX:
                startRotation = wallX;
                currentOrientation = currentDirection.North;
                myPantinTr.Rotate(Vector3.left, 90f);
                myPantinTr.Rotate(Vector3.forward, 180f);
                myPantinTr.Rotate(Vector3.up, 90f);
                bouttonValve.transform.position = new Vector3(myPantinTr.transform.position.x + offSet, myPantinTr.transform.position.y, myPantinTr.transform.position.z);
                break;
            case pivotAngle.MurMoinsX:
                startRotation = wallMoinsX;
                currentOrientation = currentDirection.North;
                myPantinTr.Rotate(Vector3.left, 270f);
                myPantinTr.Rotate(Vector3.up, startRotation.y);
                bouttonValve.transform.position = new Vector3(myPantinTr.transform.position.x - offSet, myPantinTr.transform.position.y, myPantinTr.transform.position.z);
                break;
            default:
                startRotation = floor;
                currentOrientation = currentDirection.North;
                bouttonValve.transform.position = new Vector3(myPantinTr.transform.position.x, myPantinTr.transform.position.y + offSet, myPantinTr.transform.position.z);
                break;
        }
        switch (orientation)
        {
            case startDirection.North:
                currentOrientation = currentDirection.North;
                break;
            case startDirection.East:
                currentOrientation = currentDirection.East;
                myPantinTr.Rotate(Vector3.forward, 90f);
                break;
            case startDirection.South:
                currentOrientation = currentDirection.South;
                myPantinTr.Rotate(Vector3.forward, 180f);
                break;
            case startDirection.West:
                currentOrientation = currentDirection.West;
                myPantinTr.Rotate(Vector3.forward, 270f);
                break;
            case startDirection.NorthEast:
                currentOrientation = currentDirection.NorthEast;
                myPantinTr.Rotate(Vector3.forward, 45f);
                break;
            case startDirection.SouthEast:
                currentOrientation = currentDirection.SouthEast;
                myPantinTr.Rotate(Vector3.forward, 135f);
                break;
            case startDirection.SouthWest:
                currentOrientation = currentDirection.SouthWest;
                myPantinTr.Rotate(Vector3.forward, 225f);
                break;
            case startDirection.NorthWest:
                currentOrientation = currentDirection.NorthWest;
                myPantinTr.Rotate(Vector3.forward, 315f);
                break;
            default:
                currentOrientation = currentDirection.North;
                break;
        }
        switch (positionDepart)
        {
            case startPosition.triangleFront45:
                myAnimator.SetBool("45_90R90L", true);
                break;
            case startPosition.triangleDown60:
                myAnimator.SetBool("60_180R180L", true);
                break;
            case startPosition.triangleDownRightCorner60:
                myAnimator.SetBool("60_0R90L", true);
                break;
            case startPosition.triangleDownLeftCorner60:
                myAnimator.SetBool("60_90R0L", true);
                break;
            case startPosition.squareTopLeftCorner90:
                myAnimator.SetBool("90_180R90L", true);
                break;
            case startPosition.squareTopRightCorner90:
                myAnimator.SetBool("90_90R180L", true);
                break;
            case startPosition.squareFront90:
                myAnimator.SetBool("90_90R90L", true);
                break;
            case startPosition.squareDownLeftCorner90:
                myAnimator.SetBool("90_0R90L", true);
                break;
            case startPosition.squareDownRightCorner90:
                myAnimator.SetBool("90_90R0L", true);
                break;
            case startPosition.FL_90:
                myAnimator.SetBool("FL_90", true);
                break;
            case startPosition.FR_90:
                myAnimator.SetBool("FR_90", true);
                break;
            case startPosition.FL_45:
                myAnimator.SetBool("FL_45", true);
                break;
            case startPosition.FR_45:
                myAnimator.SetBool("FR_45", true);
                break;
            case startPosition.triangle60FrontCenter:
                myAnimator.SetBool("60_90R90L", true);
                break;
            case startPosition.triangle60FrontLeft:
                myAnimator.SetBool("FL_60", true);
                break;
            case startPosition.triangle60FrontRight:
                myAnimator.SetBool("FR_60", true);
                break;
            case startPosition.triangleLeft45Bottom90:
                myAnimator.SetBool("45_135R180L", true);
                break;
            case startPosition.triangleRight45Bottom90:
                myAnimator.SetBool("45_90R45L", true);
                break;
            default:
                myAnimator.SetBool("45_90R90L", true);
                break;
        }
        CheckPuzzleState();
    }

    private void PlacementPantin()
    {
        switch (currentOrientation)
        {
            case currentDirection.North:
                currentOrientation = currentDirection.East;
                break;
            case currentDirection.East:
                currentOrientation = currentDirection.South;
                break;
            case currentDirection.South:
                currentOrientation = currentDirection.West;
                break;
            case currentDirection.West:
                currentOrientation = currentDirection.North;
                break;
            case currentDirection.NorthEast:
                currentOrientation = currentDirection.SouthEast;
                break;
            case currentDirection.SouthEast:
                currentOrientation = currentDirection.SouthWest;
                break;
            case currentDirection.SouthWest:
                currentOrientation = currentDirection.NorthWest;
                break;
            case currentDirection.NorthWest:
                currentOrientation = currentDirection.NorthEast;
                break;
            default:
                currentOrientation = currentDirection.North;
                break;
        }
    }
}
