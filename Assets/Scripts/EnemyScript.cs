using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

// Paths coded with help from: https://www.youtube.com/watch?v=KoFDDp5W5p0 
// Animation coded with help from: https://www.youtube.com/watch?v=nBkiSJ5z-hE 

public class EnemyScript : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 0.01f;
    int waypointIndex = 1;
    bool pausing = false;
    float pauseTime = -1f;

    Animator animator;

    string currentState;
    Vector3 lastPos;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        transform.position = waypoints[0].position;
        lastPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        WalkPath();   
        float xChange = transform.position.x - lastPos.x;
        float yChange = transform.position.y - lastPos.y;
        if (Math.Abs(xChange) > Math.Abs(yChange)) {
            ChangeAnimationState("EnemyWalkLeft");
        }
        else {
            ChangeAnimationState("EnemyWalkUp");
        }
        lastPos = transform.position;
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

    void ChangeAnimationState(string state) {
        if (currentState != state) {
            animator.Play(state);
            currentState = state;
        }
    }
}
