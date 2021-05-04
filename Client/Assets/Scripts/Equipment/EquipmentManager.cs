using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    //singleton to make sure we have only one instance of inventory
    #region Singleton
    public static EquipmentManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            //send message to client log
            MessageWorld.instance.SendMessageWorld("More than one instance of Equipment Manager found!");

            Debug.LogWarning("More than one instance of Equipment Manager found!");
            return;
        }

        instance = this;
    }
    #endregion

    //itms currentlu equiped
    Equipment[] currentEquipment;

    //reference to inventory
    Inventory inventory;

    //this method will be called when we add or remove equipment
    //always you add/remove item, you'll be able to do things, such as add atk stats, or remove def stats or whatever you want
    public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);
    public OnEquipmentChanged onEquipmentChanged;

    private void Start()
    {
        inventory = Inventory.instance;

        //get the the enum length
        int numSlots = System.Enum.GetNames(typeof(EquipmentSlotEnum)).Length;

        //instantiate an array of Equipment with the same length of EquipmentSlot Enum
        currentEquipment = new Equipment[numSlots];
    }

    /// <summary>
    /// Add in Equipment Array a new Equipment
    /// </summary>
    /// <param name="newItem"></param>
    public void Equip(Equipment newItem)
    {
        int slotIndex = (int)newItem.equipSlot;

        if (currentEquipment[slotIndex] != null)
        {
            //add back to inventory
            inventory.Add(currentEquipment[slotIndex]);
        }

        //call callback
        if (onEquipmentChanged != null)
        {
            onEquipmentChanged.Invoke(newItem, currentEquipment[slotIndex]);
        }

        currentEquipment[slotIndex] = newItem;
    }


    /// <summary>
    /// Remove from Equipment Array an Item
    /// </summary>
    /// <param name="slotIndex"></param>
    public void Unequip(int slotIndex)
    {
        if (currentEquipment[slotIndex] != null)
        {
            //add back to inventory
            inventory.Add(currentEquipment[slotIndex]);

            //set equipment slot empty again
            currentEquipment[slotIndex] = null;

            //call callback
            if (onEquipmentChanged != null)
            {
                onEquipmentChanged.Invoke(null, currentEquipment[slotIndex]);
            }
        }


    }
}
