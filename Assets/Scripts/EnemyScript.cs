using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Coded with help from: https://www.youtube.com/watch?v=KoFDDp5W5p0

public class EnemyScript : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 0.02f;
    int waypointIndex = 1;
    bool pausing = false;
    float pauseTime = -1f;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = waypoints[0].position;
    }

    // Update is called once per frame
    void Update()
    {   
        WalkPath();
    }

    void WalkPath() {
        if (!pausing) {
            if (waypointIndex >= waypoints.Length) {
                waypointIndex = 0;
                pausing = true;
                pauseTime = Time.time;
            }
            Vector3 target = waypoints[waypointIndex].position;

            transform.position = Vector2.MoveTowards(transform.position, target, (speed * Time.deltaTime));
            if (transform.position.x == target.x && transform.position.y == target.y) {
                waypointIndex++;
            }
        }
        else {
            float elapsedTime = Time.time - pauseTime;
            if (elapsedTime > 3) {
                pausing = false;
                pauseTime = -1f;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.CompareTag("Weapon")) {
            // Enemy lose health
        }
        if (collision.collider.CompareTag("Purifier")) {
            // Freeze enemy (?)
            // Maybe start timer for purification
        }
    }
}
