using Cam;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    //all player in the client
    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();

    public GameObject localPlayerPrefab;
    public GameObject playerPrefab;



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            //send message to client log
            MessageWorld.instance.SendMessageWorld("Instance already exists, destroying object.");

            Debug.Log("Instance already exists, destroying object.");
            Destroy(this);
        }
    }

    /// <summary>Spawns a player.</summary>
    /// <param name="_id">The player's ID.</param>
    /// <param name="_name">The player's name.</param>
    /// <param name="_position">The player's starting position.</param>
    /// <param name="_rotation">The player's starting rotation.</param>
    public void SpawnPlayer(int _id, string _username, Vector3 _position, Quaternion _rotation)
    {
        GameObject player;

        if (_id == Client.instance.myId)
        {
            //spawn local player
            player = Instantiate(localPlayerPrefab, _position, _rotation);

            //set agent to target camera
            Camera.main.gameObject.GetComponent<CameraController>().target = player.GetComponentInChildren<NavMeshAgent>().transform;
        }
        else
        {
            player = Instantiate(playerPrefab, _position, _rotation);

        }
      

        // I use GetComponentInChildren, cos my PlayerManager is in the GameObject inside "Player"
        //to access my PlayerManager component, I neet to acess where the component is located
        player.GetComponent<PlayerManager>().Initialize(_id, _username);

        //same here, I need to Add the component that PlayerManager in located
        players.Add(_id, player.GetComponentInChildren<PlayerManager>());
    }


}
