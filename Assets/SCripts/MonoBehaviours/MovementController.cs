using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    //CONFIG PARAMS
    public float movementSpeed = 3.0f;
    Vector2 movement = new Vector2();
    Animator myAnimator;
    Rigidbody2D myRb2D;
    //OUR OWN DATA TYPE
    //enum CharStates
    //{
    //    walkEast = 1,
    //    walkSouth = 2,
    //    walkWest = 3,
    //    walkNorth = 4,
    //    idleSouth = 5
    //}
    private void Start()
    {
        myAnimator = GetComponent<Animator>();
        myRb2D = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        UpdateState();
    }
    private void FixedUpdate()
    {
        MoveCharacter();
    }
    private void MoveCharacter()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        movement.Normalize();

        myRb2D.velocity = movement * movementSpeed;
    }
    private void UpdateState()
    {
        /*
         * MODIFIED CODE:
         * USING APPROXIMATELY WILL RETURN TRUE IF CLOSE TO THE VALUE
         */
        if(Mathf.Approximately(movement.x, 0) && Mathf.Approximately(movement.y, 0))
        {
            myAnimator.SetBool("isWalking", false);
        }
        else
        {
            myAnimator.SetBool("isWalking", true);
        }
        //
        myAnimator.SetFloat("xDir", movement.x);
        myAnimator.SetFloat("yDir", movement.y);
        //if(movement.x > 0)
        //{
        //    myAnimator.SetInteger(ANIMATIONSTATE, (int)CharStates.walkEast);
        //}
        //else if(movement.x < 0)
        //{
        //    myAnimator.SetInteger(ANIMATIONSTATE, (int)CharStates.walkWest);
        //}
        //else if(movement.y < 0)
        //{
        //    myAnimator.SetInteger(ANIMATIONSTATE, (int)CharStates.walkSouth);
        //}
        //else if(movement.y > 0)
        //{
        //    myAnimator.SetInteger(ANIMATIONSTATE, (int)CharStates.walkNorth);
        //}
        //else
        //{
        //    myAnimator.SetInteger(ANIMATIONSTATE, (int)CharStates.idleSouth);
        //}
    }

}
