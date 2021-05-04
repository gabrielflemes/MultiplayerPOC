using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//ScriptableObject
[CreateAssetMenu(fileName ="New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName = "New Item";  //name of the item
    public Sprite itemIcon = null;        // item icon
    public bool isDefaultItem = false;    // Is teh item default wear?



    /// <summary>
    /// is called when the slot button is clicked from UI
    /// </summary>
    public virtual void Use()
    {
        //Use the Item
        //send message to client log
        MessageWorld.instance.SendMessageWorld("Use the Item" + itemName);
        Debug.Log("Use the Item" + itemName);
    }

    public void RemoveFromInventory()
    {
        Inventory.instance.Remove(this);
    }
}
