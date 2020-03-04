using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    //CONFIG PARAMS
    //WE INHERIT THE CLASS VARIABLES HITPOINTS FROM THE CHARACTER CLASS

    void OnTriggerEnter2D(Collider2D otherColllider)
    {
        if (otherColllider.gameObject.CompareTag("CanBePickedUp"))
        {
            Item hitObject = otherColllider.gameObject.GetComponent<Consumable>().consumableItem;
            if(hitObject != null)
            {
                print("it: " + hitObject.objectName);
                switch (hitObject.itemType)
                {
                    case Item.ItemType.COIN:
                        break;
                    case Item.ItemType.HEALTH:
                        AdjustHitPoints(hitObject.quantity);
                        break;
                    default:
                        break;
                }
                otherColllider.gameObject.SetActive(false);
            }
        }
    }
    //
    public void AdjustHitPoints(int amount)
    {
        hitPoints = hitPoints + amount;
        print("Adjusted hitpoints by: " + amount + ". New value: " + hitPoints);
    }
}

