using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{

    public Image icon;
    public Button removeButton;

    private Item item;


    public void AddItemToSlot(Item newItem)
    {
        item = newItem;

        icon.sprite = item.itemIcon;
        icon.enabled = true;

        //make this button interable to be clicked
        removeButton.interactable = true;
    }

    public void RemoveItemFromSlot()
    {
        item = null;

        icon.sprite = null;
        icon.enabled = false;

        //make this button ineterable to be clicked
        removeButton.interactable = false;

    }


    //is called when the remove button on Inventory UI is cleckd
    public void OnRemoveButton()
    {
        Inventory.instance.Remove(item);
    }


    //is called when de slot button is clicked
    public void UseItem()
    {
        //whe have to very fy because we are able to click on an empty slot
        //if the slot is not equal to null, call User() method from Item class
        if (item != null)
        {
            item.Use();
        }
    }
    
}
