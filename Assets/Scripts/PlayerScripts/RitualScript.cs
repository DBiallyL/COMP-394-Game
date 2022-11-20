using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RitualScript : MonoBehaviour
{
    List<Collider2D> trappedEnemies;
    BoxCollider2D ritualCollider;
    float ritualTimer = -1f;
    float ritualLength = 5f;

    // Start is called before the first frame update
    void Start()
    {
        trappedEnemies = new List<Collider2D>();
        ritualCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ritualTimer != -1f) {
            float elapsedTime = Time.time - ritualTimer;
            if (elapsedTime >= ritualLength) {
                ritualTimer = -1f;
                for (int i = 0; i < trappedEnemies.Count; i++) {
                    trappedEnemies[i].SendMessage("RitualEnd");
                }
            }
        }
    }

    void StartRitual() {
        ritualCollider.enabled = true;
        ritualTimer = Time.time;
    }

    void StopPremature() {
        ritualCollider.enabled = false;
        for (int i = 0; i < trappedEnemies.Count; i++) {
            trappedEnemies[i].SendMessage("StopRitualPremature");
        }
        // TODO: Test Clear worked
        trappedEnemies.Clear();
    }

    /**
    * If collided with the enemy, tell the enemy to lose some health
    */
    void OnTriggerEnter2D(Collider2D coll) {
        // TODO: Make it so only enemies in range when ritual *starts* get affected, right now I believe enemies can walk in and be affected
        if (coll.CompareTag("Enemy")) {
            coll.SendMessage("StartRitual"); 
            trappedEnemies.Add(coll);
        }
    }
}
