using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLightScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // NOT WORKING
    void OnCollisionEnter2D(Collision2D collision){
        print("Baller 1");
        if (collision.collider.CompareTag("Player")) {
            print("Baller player");
        }

    }
}
