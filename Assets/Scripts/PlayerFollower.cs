using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
    public GameObject player;
    private Vector3 change = new Vector3(0, 0, 0);
    bool follow = true;

    void Start()
    {
        transform.position = player.transform.position;
    }

    void LateUpdate()
    {
        if (follow) transform.position = player.transform.position + change;
    }

    void SendPlayerMessage(string message) {
        player.SendMessage(message);
    }

    void DontFollow() {
        follow = false;
    }

    void Follow() {
        follow = true;
    }
}