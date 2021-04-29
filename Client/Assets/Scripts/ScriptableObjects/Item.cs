using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//ScriptableObject
[CreateAssetMenu(fileName ="New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName = "New Item";
    public Sprite itemIcon = null;
    public bool isDefaultItem = false;


    //is called when the slot button is clicked from UI
    public virtual void Use()
    {
        //Use the Item
        //send message to client log
        MessageWorld.instance.SendMessageWorld("Use the Item" + itemName);
        Debug.Log("Use the Item" + itemName);
    }
}
