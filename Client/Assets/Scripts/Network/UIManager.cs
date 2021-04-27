using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject startMenu;
    public InputField usernameField;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Debug.Log("Instance already exists");
            Destroy(this);
        }
    }

    private void Start()
    {
        startMenu.SetActive(true);
    }


    public void ConnectToServer()
    {
        startMenu.SetActive(false);
        usernameField.interactable = false;

        Client.instance.ConnectToServer();
    }



}
