using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Weapon : MonoBehaviour
{
    //CONFIG PARAMS
    public GameObject ammoPrefab;
    static List<GameObject> ammoPool;
    public int poolSize;
    public float weaponVelocity;
    bool isFiring;
    [HideInInspector] public Animator animator;
    Camera localCamera;
    float positiveSlope;
    float negativeSlope;
    enum Quadrant
    {
        East,
        South,
        West,
        North
    }
    //LOAD THE POOL OF AMMO
    private void Awake()
    {
        if(ammoPool == null)
        {
            ammoPool = new List<GameObject>();
        }

        for(int i = 0; i < poolSize; i++)
        {
            GameObject ammoObject = Instantiate(ammoPrefab);
            ammoObject.SetActive(false);
            ammoPool.Add(ammoObject);
        }
    }
    //CONNECT TO THE COMPONENTS AND SET VARIABLES
    private void Start()
    {
        animator = GetComponent<Animator>();
        isFiring = false;
        localCamera = Camera.main;  // A HANDLE TO THE CAMERA
        //SLOPE INTERCEPT WORK, SET THE SCREEN CORNERS, THEN DRAW A LINE TO THE CORNERS AND SET AS VARS.
        Vector2 lowerleft = localCamera.ScreenToWorldPoint(new Vector2(0, 0));
        Vector2 upperRight = localCamera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        Vector2 upperLeft = localCamera.ScreenToWorldPoint(new Vector2(0, Screen.height));
        Vector2 lowerRight = localCamera.ScreenToWorldPoint(new Vector2(Screen.width, 0));
        //THE LINES DIVIDING THE SCREEN FROM THE PLAYER POSITION
        positiveSlope = GetSlope(lowerleft, upperRight);
        negativeSlope = GetSlope(upperLeft, lowerRight);
    }
    //CHECK FOR USER INPUT LEFT-CLICK
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isFiring = true;
            FireAmmo();
        }
        UpdateState();
    }
    //POOL CONTROL, WILL PUT THE OBJECT AT THE LOCATION TO WHERE MOUSE WAS CLICKED
    private GameObject SpawnAmmo(Vector3 location)
    {
        foreach(GameObject ammo in ammoPool)
        {
            if (ammo.activeSelf == false)
            {
                ammo.SetActive(true);
                ammo.transform.position = location;
                return ammo;
            }
        }
        return null;
    }
    /*
     * WE WILL SHOOT TO THE MOUSE CLICK-POINT ON SCREN TO WORLD SPACE.
     * CONNECT TO THE ARC CLASS AND CALL THE COROUTINE, WITH THE SETTING TO SPEED
     * note: THE SPEED WILL BE THE SAME NO MATTER THE DISTANCE
     */
    private void FireAmmo()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GameObject ammo = SpawnAmmo(transform.position);
        if(ammo != null)
        {
            Arc arcScript = ammo.GetComponent<Arc>();
            float travelDuration = 1.0f / weaponVelocity;
            StartCoroutine(arcScript.TravelArc(mousePosition, travelDuration));
        }
    }
    //FIND THE SLOPE (M VARIABLE) AND SAVE IT FOR LATER.
    float GetSlope(Vector2 pointOne, Vector2 pointTwo)
    {
        return (pointTwo.y - pointOne.y) / (pointTwo.x - pointOne.x);
    }
    //CHECK TO SEE IF MOUSE-CLICK WAS ABOVE THIS LINE.
    bool HigherThanPositiveSlopeLine(Vector2 inputPosition)
    {
        Vector2 playerPosition = gameObject.transform.position;
        Vector2 mousePosition = localCamera.ScreenToWorldPoint(inputPosition);
        //GET THE B VARIABLE TO SEE IF MOUSE IS ABOVE THE LINE.
        float yIntercept = playerPosition.y - (positiveSlope * playerPosition.x);
        float inputIntercept = mousePosition.y - (positiveSlope * mousePosition.x);
        //TRUE OR FALSE IS ABOVE THE PLAYER LINE
        return inputIntercept > yIntercept;
    }
    //CHECK TO SEE IF MOUSE-CLICK WAS ABOVE THIS LINE, SAME AS ABOVE.
    bool HigherThanNegativeSlopeLine(Vector2 inputPosition)
    {
        Vector2 playerPosition = gameObject.transform.position;
        Vector2 mousePosition = localCamera.ScreenToWorldPoint(inputPosition);
        //
        float yIntercept = playerPosition.y - (negativeSlope * playerPosition.x);
        float inputIntercept = mousePosition.y - (negativeSlope * mousePosition.x);
        return inputIntercept > yIntercept;
    }
    //FIND THE POSITION WITH ALL THE FACTS, RETURNED TO THE UPDATE STATE
    private Quadrant GetQuadrant()
    {
        Vector2 mousePosition = Input.mousePosition;
        Vector2 playerPosition = transform.position;
        //GET THE POSITION INFO
        bool higherThanPositionSlopeLine = HigherThanPositiveSlopeLine(Input.mousePosition);
        bool higherThanNegativeSlopeLine = HigherThanNegativeSlopeLine(Input.mousePosition);
        //MAKE THE CHOICE BASED ON THE POSITION.
        if(!higherThanPositionSlopeLine && higherThanNegativeSlopeLine)
        {
            return Quadrant.East;
        }
        else if(!higherThanPositionSlopeLine && !higherThanNegativeSlopeLine)
        {
            return Quadrant.South;
        }
        else if(higherThanPositionSlopeLine && !higherThanNegativeSlopeLine)
        {
            return Quadrant.West;
        }
        else
        {
            return Quadrant.North;
        }
    }
    //WE DO THE WORK TO SET THE DIRECTION FOR THE ANIMATION.
    private void UpdateState()
    {
        if (isFiring)
        {
            Vector2 quadrantVector;
            Quadrant quadEnum = GetQuadrant();
            switch (quadEnum)
            {
                case Quadrant.East:
                    quadrantVector = new Vector2(1.0f, 0.0f);
                    break;
                case Quadrant.South:
                    quadrantVector = new Vector2(0.0f, -1.0f);
                    break;
                case Quadrant.West:
                    quadrantVector = new Vector2(-1.0f, 1.0f);
                    break;
                case Quadrant.North:
                    quadrantVector = new Vector2(0.0f, 1.0f);
                    break;
                default:
                    quadrantVector = new Vector2(0.0f, 0.0f);
                    break;
            }
            animator.SetBool("isFiring", true);
            animator.SetFloat("fireXdir", quadrantVector.x);
            animator.SetFloat("fireYdir", quadrantVector.y);
            isFiring = false;
        }
        else
        {
            animator.SetBool("isFiring", false);
        }
    }
    //A UNITY METHOD LIFE CYCLE.
    private void OnDestroy()
    {
        ammoPool = null;
    }
}
