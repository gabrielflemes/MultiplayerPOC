using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerManager : MonoBehaviour
{
    public int id;
    public string username;


    public void Initialize(int _id, string _username)
    {
        id = _id;
        username = _username;

    }

    public void MoveTo(Vector3 _point)
    {
        GetComponentInChildren<NavMeshAgent>().SetDestination(_point);
    }

}
