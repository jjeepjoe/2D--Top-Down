using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ANY OBJECT TO USE THIS SCRIPT NEEDS THESE COMPONENTS, AND WILL ATTACH THEM.
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Animator))]
public class Wander : MonoBehaviour
{
    //CONFIG PARAMS
    public float pursuitSpeed;
    public float wanderSpeed;
    float currentSpeed;
    public float directionChangeInterval; //how often to change directions
    public bool followPlayer; //to toggle the follow mode or not
    Vector3 endPosition; //the random direction destination
    float currentAngle = 0;  //change angle
    //CASHE COMPONENT
    Coroutine moveCoroutine; //so we can control the coroutine.
    Rigidbody2D rb2D;
    Animator animator;
    Transform targetTransform = null; //the Player we are following
    CircleCollider2D circleCollider;

    //EDITOR GIZMO FOR VISUAL DEBUG.
    private void OnDrawGizmos()
    {
        if(circleCollider != null)
        {
            Gizmos.DrawWireSphere(transform.position, circleCollider.radius);
        }
    }
    //HANDLE THE COMPONENTS
    private void Start()
    {
        animator = GetComponent<Animator>();
        currentSpeed = wanderSpeed;
        rb2D = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        StartCoroutine(WanderRoutine());
    }
    //
    private void Update()
    {
        Debug.DrawLine(rb2D.position, endPosition, Color.red);
    }
    //THIS IS THE THINKING PART: PICK AN END POINT, THEN AT A SET INTERVAL BAD GUY WILL CHANGE DIRECTIONS,
    //CHECK IF MOVING ALREADY, STOP, START A NEW MOVE, SAVE THE MOVE CONNECTION.
    public IEnumerator WanderRoutine()
    {
        while (true)
        {
            ChooseNewEndpoint();
            if(moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }
            //NOT MOVING SO LETS MOVE
            moveCoroutine = StartCoroutine(Move(rb2D, currentSpeed));
            yield return new WaitForSeconds(directionChangeInterval);
        }
    }
    /*
     * CHOOSE RANDOM NUMBER 0 - 359, ADD IT TO OUR CURRENT DIRECTION. (could be higher than 360.)
     * Mathf.Repeat: LOOPS THE VALUE SO IT IS NOT GREATER THAN 360.
     * SETS THE ENDPOINT BY SENDING THE RESULT TO OUR METHOD TO CONVERT ANGLE TO RADIANS.
     */
    private void ChooseNewEndpoint()
    {
        currentAngle += Random.Range(0, 360);
        currentAngle = Mathf.Repeat(currentAngle, 360);
        endPosition += Vector3FromAngle(currentAngle);
    }
    /*
     * WE MADE THIS TO DO OUR MATH RADIANS TO VECTORS: TAKES AN ANGLE AND CONVERTS TO RADIANS.
     * TAKES A PARAMETER AND CONVERTS TO RADIANS, SO THE VECTOR CAN UNDERSTAND.
     */
    Vector3 Vector3FromAngle(float inputAngleDegrees)
    {
        float inputAngleRadians = inputAngleDegrees * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(inputAngleRadians), Mathf.Sin(inputAngleRadians), 0);
    }
    /*
     * THIS IS THE ACTION ON THE BAD GUY: sqrMagnitude IS A VECTOR3 FUNCTION THAT GETS THE ROUGH DISTANCE CAL.
     * CHECK THAT > ZERO, IF CHASING THE PLAYER THE TRANSFORM WILL NOT BE NULL.
     * Vector3.MoveTowards: CALCULATES THE MOVEMENT DIRECTION ONLY, ASSIGN TO A VAR
     * USE THE sqrMagnitude: TO ROUND OUT THE DIRECTION, THEN ONCE AT POINT STOP ANIMATION.
     */
    public IEnumerator Move(Rigidbody2D rigidBodyToMove, float speed)
    {
        //sqrMagnitude is a Unity propety to return distance vector math.
        float remainingDistance = (transform.position - endPosition).sqrMagnitude;
        while(remainingDistance > float.Epsilon)
        {
            if(targetTransform != null)
            {
                endPosition = targetTransform.position;
            }
            //MAKE SURE WE HAVE THE COMPONENT
            if(rigidBodyToMove != null)
            {
                animator.SetBool("isWalking", true);
                //MoveTowards returns the vector toward the target(start pos, end pos, speed)
                Vector3 newPosition = Vector3.MoveTowards(rigidBodyToMove.position, endPosition, 
                    speed * Time.deltaTime);
                rb2D.MovePosition(newPosition);
                remainingDistance = (transform.position - endPosition).sqrMagnitude;
            }
            yield return new WaitForFixedUpdate();
        }
        //LESS THAN ZERO
        animator.SetBool("isWalking", false);
    }
    /*
     * USED TO DETECT THE PLAYER AND CHASE THEM.
     */
    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if(otherCollider.gameObject.CompareTag("Player") && followPlayer)
        {
            currentSpeed = pursuitSpeed;
            targetTransform = otherCollider.gameObject.transform;

            if(moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }
            moveCoroutine = StartCoroutine(Move(rb2D, currentSpeed));
        }
    }
    /*
     * ONCE OUT OF THE COLLIDER RANGE WILL STOP AND WANDER AGAIN.
     */
    private void OnTriggerExit2D(Collider2D otherCollder)
    {
        if (otherCollder.gameObject.CompareTag("Player"))
        {
            animator.SetBool("isWalking", false);
            currentSpeed = wanderSpeed;
            if(moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }
            targetTransform = null;
        }
    }
}
