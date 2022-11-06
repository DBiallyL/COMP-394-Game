using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
    public GameObject player;
    private Vector3 change = new Vector3(0f, 0, 0);

    void Start()
    {
        change += transform.position;
    }

    void LateUpdate()
    {
        transform.position = player.transform.position + change;
    }
}