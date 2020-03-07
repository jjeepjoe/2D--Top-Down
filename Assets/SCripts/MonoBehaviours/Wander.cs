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
    Vector3 endPosition; //the destination
    float currentAngle = 0;  //change angle
    //CASHE COMPONENT
    Coroutine moveCoroutine; //so we can control the coroutine.
    Rigidbody2D rb2D;
    Animator animator;
    Transform targetTransform = null; //the Player we are following

    //
    private void Start()
    {
        animator = GetComponent<Animator>();
        currentSpeed = wanderSpeed;
        rb2D = GetComponent<Rigidbody2D>();
        StartCoroutine(WanderRoutine());
    }
    //
    public IEnumerator WanderRoutine()
    {
        while (true)
        {
            ChooseNewEndpoint();
            if(moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }
            //
            moveCoroutine = StartCoroutine(Move(rb2D, currentSpeed));
            yield return new WaitForSeconds(directionChangeInterval);
        }
    }
    //
    void ChooseNewEndpoint()
    {
        currentAngle += Random.Range(0, 360);
        currentAngle = Mathf.Repeat(currentAngle, 360);
        endPosition += Vector3FromAngle(currentAngle);
    }
    //
    Vector3 Vector3FromAngle(float inputAngleDegrees)
    {
        float inputAngleRadians = inputAngleDegrees * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(inputAngleRadians), Mathf.Sin(inputAngleRadians), 0);
    }
    //
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
        animator.SetBool("isWalking", false);
    }

}
