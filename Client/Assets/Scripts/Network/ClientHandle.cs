using Player;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{

    public static void Welcome(Packet _packet)
    {

        //IMPORTANT: we are using TCP, so we have make sure to receive the packed in the same order
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();

        //send message to client log
        MessageWorld.instance.SendMessageWorld($"{_myId} : {_msg}");

        Debug.Log($"{_myId} : {_msg}");
        Client.instance.myId = _myId;       
        ClientSend.WelcomeReceived(); //send welcome received packet


        // Now that we have the client's id, connect UDP
        Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
    }

    public static void SpawnPlayer(Packet _packet)
    {
        int id = _packet.ReadInt();
        string username = _packet.ReadString();
        Vector3 position = _packet.ReadVector3();
        Quaternion rotation = _packet.ReadQuaternion();


        GameManager.instance.SpawnPlayer(id, username, position, rotation);

    }

    public static void PlayerPosition(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();

        if (GameManager.players.TryGetValue(_id, out PlayerManager _player))
        {
            _player.transform.position = _position;
        }
    }

    public static void PlayerRotation(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Quaternion _rotation = _packet.ReadQuaternion();

        if (GameManager.players.TryGetValue(_id, out PlayerManager _player))
        {
            _player.transform.rotation = _rotation;
        }
    }

    public static void MoveTo(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector3 _point = _packet.ReadVector3();

        GameManager.players[_id].MoveTo(_point);
    }

    public static void PlayerDisconnected(Packet _packet)
    {
        int _id = _packet.ReadInt();

        Destroy(GameManager.players[_id].gameObject);
        GameManager.players.Remove(_id);
    }

}
