using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item")]
public class Item : ScriptableObject
{
    //CONFIG PARAMS
    public string objectName;
    public Sprite sprite;
    public int quantity;
    public bool stackable;

    public enum ItemType
    {
        COIN,
        HEALTH
    }
    //A PROPERTY SETTING WE SET IN THE UNITY INSPECTOR FOR EACH OBJECT OR ITEM WE CREATED.
    public ItemType itemType;

}
