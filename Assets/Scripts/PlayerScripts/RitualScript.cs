using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RitualScript : MonoBehaviour
{
    public GameObject ritualBar;
    List<Collider2D> trappedEnemies;
    BoxCollider2D ritualCollider;
    SpriteRenderer spriteRenderer;
    float ritualTimer = -1f;

    // Only allows enemies in range at very start of ritual to be affected
    float detectionLength = 0.1f;
    float ritualLength = 5f;
    bool canDetect = true;

    // Start is called before the first frame update
    void Start()
    {
        trappedEnemies = new List<Collider2D>();
        ritualCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ritualTimer != -1f) {
            float elapsedTime = Time.time - ritualTimer;
            if (elapsedTime >= ritualLength) {
                ResetRitualObject("RitualEnd");
                transform.parent.SendMessage("StopRitual");
            }
            else if (elapsedTime >= detectionLength) {
                canDetect = false;
            }
        }
    }

    /**
    * Starts the ritual process by enabling the enemy detection and starting a timer
    */
    void StartRitual() {
        ritualCollider.enabled = true;
        spriteRenderer.enabled = true;
        ritualTimer = Time.time;
    }

    /**
    * Resets the enemies and all the variables the Ritual uses to keep track of the ritual 
    * Called when a ritual is naturally or prematurally stopped - behavior determined by the enemyMessage param
    */
    void ResetRitualObject(string enemyMessage) {
        for (int i = 0; i < trappedEnemies.Count; i++) {
            trappedEnemies[i].SendMessage(enemyMessage);
        }
        trappedEnemies.Clear();

        ritualTimer = -1f;
        canDetect = true;
        ritualCollider.enabled = false;
        spriteRenderer.enabled = false;
        ritualBar.SendMessage("StopRitual");
    }

    /**
    * If collided with the enemy, tell the enemy to lose some health
    */
    void OnTriggerEnter2D(Collider2D coll) {
        if (canDetect && coll.CompareTag("Enemy")) {
            coll.SendMessage("StartRitual"); 
            trappedEnemies.Add(coll);
        }
    }
}
