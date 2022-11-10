using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RitualScript : MonoBehaviour
{
    List<GameObject> trappedEnemies;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /**
    * If collided with the enemy, tell the enemy to lose some health
    */
    void OnTriggerEnter2D(Collider2D coll) {
        if (coll.CompareTag("Enemy")) {
            // coll.SendMessage("StartRitual"); Uncomment when ritual implemented
            // trappedEnemies.Add(coll.gameObject);
        }
    }
}
