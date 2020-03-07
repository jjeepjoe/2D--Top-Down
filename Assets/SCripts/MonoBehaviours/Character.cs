using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    /*
     * THIS IS THE TOP LEVEL OF THE CHARACTER CLASS, WHERE ALL LIVE OBJECTS WILL START. 
     */

    //CONFIG PARAMS
    
    public float startingHitPoints;
    public float maxHitPoints;

    //THIS WILL DESTROY THE ATTACHED GAMEOBJECT.
    public virtual void KillCharacter()
    {
        Destroy(gameObject);
    }
    //manditory method for all children
    public abstract void ResetCharacter();

    //manditory method for all children
    public abstract IEnumerator DamageCharacter(int damage, float interval);

}   
