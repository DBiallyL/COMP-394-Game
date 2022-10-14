using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Coded with help from: https://www.youtube.com/watch?v=KoFDDp5W5p0

public class EnemyScript : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 0.02f;
    int waypointIndex = 1;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = waypoints[0].position;
    }

    // Update is called once per frame
    void Update()
    {
        if (waypointIndex >= waypoints.Length) {
            waypointIndex = 0;
        }
        Vector3 target = waypoints[waypointIndex].position;

        transform.position = Vector2.MoveTowards(transform.position, target, (speed * Time.deltaTime));
        print("target: " + target + " actual: " + transform.position);
        if (transform.position.x == target.x && transform.position.y == target.y) {
            print(waypointIndex);
            waypointIndex++;
        }
        // if not frozen
        // continue along walk path
        // check if can see player
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.CompareTag("Weapon")) {
            // Enemy lose health
        }
        if (collision.collider.CompareTag("Purifier")) {
            // Freeze enemy (?)
            // Maybe start timer for purification
        }
        if (collision.collider.CompareTag("Waypoint")) {
            // print("happening");
            // waypointIndex++;
        }
    }
}
