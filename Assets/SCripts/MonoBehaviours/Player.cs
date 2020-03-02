using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    //CONFIG PARAMS


    void OnTriggerEnter2D(Collider2D otherColllider)
    {
        if (otherColllider.gameObject.CompareTag("CanBePickedUp"))
        {
            otherColllider.gameObject.SetActive(false);
        }
    }
}

