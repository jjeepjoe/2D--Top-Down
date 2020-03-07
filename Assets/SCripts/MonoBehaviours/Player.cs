using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    //CONFIG PARAMS
    public HitPoints hitPoints;  //HANDLE TO SCRIPTABLE OBJECT
    public HealthBar healthBarPrefab;
    HealthBar healthBar;  //HANDLE TO THE HEALTH BAR SCRIPT
    public Inventory inventoryPrefab;
    Inventory inventory; // Handle to the Inventory script.

    private void Start()
    {
        hitPoints.value = startingHitPoints;
        ResetCharacter();
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
            return true;
        }
        return false;
    }

    //METHOD needs to be included:
    public override IEnumerator DamageCharacter(int damage, float interval)
    {
        while (true)
        {
            hitPoints.value = hitPoints.value - damage;
            if(hitPoints.value <= float.Epsilon)
            {
                KillCharacter();
                break;
            }
            if(interval > float.Epsilon)
            {
                yield return new WaitForSeconds(interval);
            }
            else
            {
                break;
            }
        }
    }
    //THE BASE KEYWORD USES SOME OF THE ORIGINAL METHOD PLUS WHAT WE ADD.
    public override void KillCharacter()
    {
        base.KillCharacter();
        Destroy(healthBar.gameObject);
        Destroy(inventory.gameObject);
    }
    //METHOD needs to be included:
    public override void ResetCharacter()
    {
        healthBar = Instantiate(healthBarPrefab);
        inventory = Instantiate(inventoryPrefab);
        healthBar.character = this;  //THE PLAYER CONNECTION TO THE HEALTH BAR SCRIPT

        hitPoints.value = startingHitPoints;
    }
}

