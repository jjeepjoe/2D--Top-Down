using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    //CONFIG PARAMS

    //PROPERTIES
    public HealthBar healthBarPrefab;
    HealthBar healthBar;  //HANDLE TO THE HEALTH BAR SCRIPT
    public Inventory inventoryPrefab;
    Inventory inventory;

    private void Start()
    {
        hitPoints.value = startingHitPoints;
        healthBar = Instantiate(healthBarPrefab);
        healthBar.character = this;  //THE PLAYER CONNECTION TO THE HEALTH BAR SCRIPT
        //
        inventory = Instantiate(inventoryPrefab);

    }
    //WE INHERIT THE CLASS VARIABLES HITPOINTS FROM THE CHARACTER CLASS
    //LOOKING FOR COLLISIONS
    void OnTriggerEnter2D(Collider2D otherColllider)
    {
        if (otherColllider.gameObject.CompareTag("CanBePickedUp"))
        {
            //GET A CONNECTION TO THE SCRIPTABLE OBJECT.
            Item hitObject = otherColllider.gameObject.GetComponent<Consumable>().consumableItem;
            if(hitObject != null)
            {
                //SET THE FLAG TO NORMAL, THEN CHECK WHAT WE COLLECTED
                bool shouldDisappear = false;
                switch (hitObject.itemType)
                {
                    case Item.ItemType.COIN:
                        shouldDisappear = inventory.AddItem(hitObject);
                        break;
                    case Item.ItemType.HEALTH:
                        //USE THE METHOD BELOW TO SEE IF WE ADDED ANY HEALTH.
                        shouldDisappear = AdjustHitPoints(hitObject.quantity);
                        break;
                    default:
                        break;
                }
                //ACT ON THE FLAG
                if (shouldDisappear)
                {
                    otherColllider.gameObject.SetActive(false);
                }
            }
        }
    }
    //HIT POINT WORK, IF LESS THAN THE MAX POINTS DO THE WORK, ELSE EXIT
    public bool AdjustHitPoints(int amount)
    {
        if(hitPoints.value < maxHitPoints)
        {
            hitPoints.value = hitPoints.value + amount;
            print("Adjusted hitpoints by: " + amount + ". New value: " + hitPoints.value);
            return true;
        }
        return false;
    }
}

