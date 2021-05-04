using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EquipmentSlot : MonoBehaviour
{
    public EquipmentSlotEnum slot; //define slot type

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

        //
        
        
    }

    public void RemoveItemFromSlot()
    {
        item = null;

        icon.sprite = null;
        icon.enabled = false;

        //make this button ineterable to be clicked
        removeButton.interactable = false;

    }


    //is called when the remove button on Equipment UI is cleckd
    public void OnRemoveButton()
    {
        //EquipmentManager.instance.Unequip();
    }


    
}
