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

    // TODO: Finish code
    void OnTriggerEnter2D(Collider2D collider) {
        // print("Baller 1");
        if (collider.CompareTag("Player")) {
            // print("Baller player");
        }
    }
}
