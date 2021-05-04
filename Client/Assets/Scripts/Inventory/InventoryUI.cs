using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    //this is the GameObject that contains all my slots
    public Transform itemsParent;

    public Transform itemsParentEquip;

    //make reference to Inventory GameObject just to Active and Inactive when we press 'I' button
    public GameObject inventoryUI;

    //instance for my INventary
    private Inventory inventory;

    //countains my slots
    private InventorySlot[] slots;
    private EquipmentSlot[] equipSlots; 

    // Start is called before the first frame update
    void Start()
    {
        //get instance by singleton
        inventory = Inventory.instance;

        //subscribe UpdateUI() to be called every time we change our Inventory
        inventory.onInventoryChangeCallback += UpdateUI;

        //subscribe UpdateEquipmentUI() to be called every time we change our Inventory
        EquipmentManager.instance.onEquipmentChanged += UpdateEquipmentUI;

        //get all slots
        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
        equipSlots = itemsParentEquip.GetComponentsInChildren<EquipmentSlot>();

    }

   
    // Update is called once per frame
    void Update()
    {
        //when we press 'I' button the inventory will active/inactive
        if (Input.GetButtonDown("Inventory"))
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
        }
    }


    //is called when an item is added or removed from the inventory
    private void UpdateUI()
    {
        //while thare are items in my Inventory, we are  gonna fill the slots
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                slots[i].AddItemToSlot(inventory.items[i]);
            }
            else
            {
                slots[i].RemoveItemFromSlot();
            }
        }


       
    }


    /// <summary>
    /// Update UI
    /// </summary>
    /// <param name="newItem"></param>
    /// <param name="oldItem"></param>
    private void UpdateEquipmentUI(Equipment newItem, Equipment oldItem)
    {
        //
        for (int i = 0; i < equipSlots.Length; i++)
        {
            if (equipSlots[i].slot == newItem.equipSlot)
            {
                equipSlots[i].AddItemToSlot(newItem);
            }
           
        }
 

    }



}
