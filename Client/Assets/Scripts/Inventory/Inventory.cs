using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//inventory
public class Inventory : MonoBehaviour
{

    //singleton to make sure we have only one instance of inventory
    #region Singleton
    public static Inventory instance;

    private void Awake()
    {
        if (instance != null)
        {
            //send message to client log
            MessageWorld.instance.SendMessageWorld("More than one instance of Inventory found!");

            Debug.LogWarning("More than one instance of Inventory found!");
            return;
        }

        instance = this;
    }
    #endregion

    //we are create a new event that will be called at every time we inventory change (add/remove item for instance)
    public delegate void OnInventoryChange();
    public OnInventoryChange onInventoryChangeCallback;


    //inventory space by default is 5
    //we are control the amount of items we are able to add in our inventory
    public int inventorySpace = 5;

    //list items on our inventory
    public List<Item> items = new List<Item>();


    //add item to our inventory
    public bool Add(Item item)
    {
        //add only not default items
        if (!item.isDefaultItem)
        {
            //just add item if we have room in our inventory
            //otherwise, send a message to UI
            if (items.Count < inventorySpace)
            {
                //add item to item list
                items.Add(item);

                //call callback
                if (onInventoryChangeCallback != null)
                {
                    onInventoryChangeCallback.Invoke();
                }
        
                //item added successfuly
                return true;
            }
            else
            {
                //send message to client log
                MessageWorld.instance.SendMessageWorld("Inventory is Full!");

                Debug.Log("Inventory is Full.");
                return false;
            }

        }
        else
        {
            return false;
        }
       
    }


    //remove item from our inventory
    public void Remove(Item item)
    {
        //remove item from item list
        items.Remove(item);

        //call callback
        if (onInventoryChangeCallback != null)
        {
            onInventoryChangeCallback.Invoke();
        }
    }

}
