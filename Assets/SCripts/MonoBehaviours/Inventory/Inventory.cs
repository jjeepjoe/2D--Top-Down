using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    //CONFIG PARAMS
    public GameObject slotPrefab;
    public const int numSlots = 5;
    /*
     * The arrays are set for the ScriptableObject we made for collectables 1. the Image, 2. type of object
     * the GameObject array is for the slots.
     */
    Image[] itemImages = new Image[numSlots];
    Item[] items = new Item[numSlots];
    GameObject[] slots = new GameObject[numSlots];

    private void Start()
    {
        CreateSlots();
    }
    //THIS IS HOW WE FILL THE SLOTS
    public void CreateSlots()
    {
        if(slotPrefab != null)
        {
            for(int i = 0; i < numSlots; i++)
            {
                GameObject newSlot = Instantiate(slotPrefab);
                newSlot.name = "ItemSlot_" + i;
                newSlot.transform.SetParent(gameObject.transform.GetChild(0).transform);
                slots[i] = newSlot;
                itemImages[i] = newSlot.transform.GetChild(1).GetComponent<Image>();
            }
        }
    }
    /*
     * WE WILL ADD A SINGLE ITEM TO OUR INVENTORY AND RETURN TRUE WHEN SUCCESSFULL.
     * WE WILL LOOP THRU ALL THE ITEMS ARRAY TO SEE IF ANY MATCHES, OR EMPTY THEN FILL SLOT.
     * IF STACKING THEN INCREASE THE QTY AND DISPLAY THE TEXT.
     */
    public bool AddItem(Item itemToAdd)
    {
        for(int i = 0; i < items.Length; i++)
        {
            if(items[i] != null && items[i].itemType == itemToAdd.itemType && 
                itemToAdd.stackable == true)
            {
                items[i].quantity = items[i].quantity + 1;
                Slot slotScript = slots[i].gameObject.GetComponent<Slot>();
                Text quantityText = slotScript.qtyText;
                quantityText.enabled = true;
                quantityText.text = items[i].quantity.ToString();
                return true;
            }
            if(items[i] == null)
            {
                items[i] = Instantiate(itemToAdd);
                items[i].quantity = 1;
                itemImages[i].sprite = itemToAdd.sprite;
                itemImages[i].enabled = true;
                return true;
            }
        }
        return false;
    }

}
