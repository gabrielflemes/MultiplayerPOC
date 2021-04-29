using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageWorld : MonoBehaviour
{

    public GameObject messageGlobal;


    //singleton to make sure we have only one instance of inventory
    #region Singleton
    public static MessageWorld instance;

    private void Awake()
    {
        if (instance != null)
        {
            //send message to client log
            SendMessageWorld("More than one instance of Inventory found!");

            Debug.LogWarning("More than one instance of Inventory found!");
            return;
        }

        instance = this;
    }
    #endregion


    public void SendMessageWorld(string msg)
    {
        //active panel
        messageGlobal.SetActive(true);

        //text mesage
        messageGlobal.GetComponentInChildren<Text>().text += $"{msg}\n";
    }
}
