using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    //CONFIG PARAMS
    float hitPoints;
    public int damageStrength;
    Coroutine damgeCoroutine;  //WE CAN SAVE A REFERENCE TO THE RUNNING COROUTNE.

    //METHOD needs to be included:
    //SINCE THE PARENT MADE THE METHODS ABSTRACT, WE HAVE TO INCLUDE THEM WITH OVERRIDE.
    public override IEnumerator DamageCharacter(int damage, float interval)
    {
        //THE BREAK WILL EXIT THE WHILE LOOP.
        while (true)
        {
            StartCoroutine(FlickerCharacter());
            hitPoints = hitPoints - damage;
            //EPSILON WITH FLOATS PREVENTS ROUNDING ERRORS.
            if(hitPoints <= float.Epsilon)
            {
                KillCharacter();
                break;
            }
            //
            if(interval > float.Epsilon)
            {
                //THIS MEANS ZERO SO 0 INTERVAL WILL JUST RUN ONCE.
                yield return new WaitForSeconds(interval);
            }
            else
            {
                break;
            }
        }
    }
    //METHOD needs to be included: WILL BE SET IN THE INSPECTOR.
    public override void ResetCharacter()
    {
        hitPoints = startingHitPoints;
    }
    //
    private void OnEnable()
    {
        ResetCharacter();
    }
    //DETECT COLLISIONS WITH PLAYER
    private void OnCollisionEnter2D(Collision2D otherCollider)
    {
        if (otherCollider.gameObject.CompareTag("Player"))
        {
            Player player = otherCollider.gameObject.GetComponent<Player>();
            //CHECK TO SEE IS ALREADY RUNNING COROUTINE, IF NOT STORE A REFERENCE TO THE RUNNING ROUTINE.
            if(damgeCoroutine == null)
            {
                damgeCoroutine = StartCoroutine(player.DamageCharacter(damageStrength, 1.0f));
            }
        }
    }
    //SINCE WE HAVE A REFERENCE TO THE COROUTINE WE CAN STOP IT ONCE WE EXIT COLLISION.
    private void OnCollisionExit2D(Collision2D otherCollider)
    {
        if (otherCollider.gameObject.CompareTag("Player"))
        {
            if(damgeCoroutine != null)
            {
                StopCoroutine(damgeCoroutine);
                damgeCoroutine = null;
            }
        }
    }
}
