using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLightScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().isKinematic = true;
        GetComponent<Collider2D>().isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /**
    * If player has entered possible visible range, tell the enemy to look for the player
    */
    void OnTriggerStay2D(Collider2D collider) {
        if (collider.CompareTag("Player")) {
            transform.parent.SendMessage("GetAngry");
        }
    }

    /**
    * If player has left the possible visible range, tell the enemy to check if the player is still nearby (if they still remember the player)
    */
    void OnTriggerExit2D(Collider2D collider) {
        if (collider.CompareTag("Player")) {
            transform.parent.SendMessage("CalmDown");
        }
    }
}
