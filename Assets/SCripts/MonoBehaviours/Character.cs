using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    /*
     * THIS IS THE TOP LEVEL OF THE CHARACTER CLASS, WHERE ALL LIVE OBJECTS WILL START. 
     */

    //CONFIG PARAMS
    public HitPoints hitPoints;  //HANDLE TO SCRIPTABLE OBJECT
    public float startingHitPoints;
    public float maxHitPoints;
}
