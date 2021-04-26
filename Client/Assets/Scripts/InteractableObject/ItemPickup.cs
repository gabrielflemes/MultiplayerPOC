using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Interactable {

    //inherit InteractableObject to override Interact() method
    public class ItemPickup : InteractableObject
    {
        //ScriptableObject Item
        public Item item;

        public override void Interact()
        {
            base.Interact();

            PickUp();
        }

        private void PickUp()
        {
            Debug.Log("picking up item " + item.itemName);

            //add to inventory by Singleton 
            //PS: If we had not used Singleton Pattern, we would've have to instanciate or inject Inventory component
            //but, how we are using Singleton, it's not necessary.
            //the Add() will return true if the item add succesfuly
            bool isAdd = Inventory.instance.Add(item);

            //if the responde is True, it means that the item was add succesfuly, so we can destroy it
            if (isAdd)
            {
                //After we have add the item in out inventory, lets destroy it.
                Destroy(gameObject);
            }
          
        }
    }

}
