using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    //CONFIG PARAMS
    public int damageInflicted;

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if(otherCollider is BoxCollider2D)
        {
            Enemy enemy = otherCollider.gameObject.GetComponent<Enemy>();
            StartCoroutine(enemy.DamageCharacter(damageInflicted, 0.0f));
            gameObject.SetActive(false);
        }
    }
}
