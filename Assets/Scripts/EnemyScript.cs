using System.Collections;
using System.Collections.Generic;
// using System;
using UnityEngine;

// Paths coded with help from: https://www.youtube.com/watch?v=KoFDDp5W5p0 
// Animation coded with help from: https://www.youtube.com/watch?v=nBkiSJ5z-hE, https://answers.unity.com/questions/952558/how-to-flip-sprite-horizontally-in-unity-2d.html

public class EnemyScript : MonoBehaviour
{
    public Texture leftLight, upLight, rightLight, downLight;
    public Transform[] waypoints;
    public float speed = 0.01f;
    int waypointIndex = 1;
    bool pausing = false;
    float pauseTime = -1f;

    Animator animator;
    SpriteRenderer spriteRenderer;
    Material lightMaterial;
    Transform lightCollider;

    string currentState;
    Vector3 lastPos;
    string lastDirection;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Transform lightChild = transform.Find("Light2D");
        lightCollider = transform.Find("EnemyLight_Collider");

        transform.position = waypoints[0].position;
        lastPos = transform.position;
        lightMaterial = lightChild.GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        WalkPath();   
        ChangeWalkSprites();
        ChangeLightRotation();
    }

    void ChangeWalkSprites() {
        float xChange = transform.position.x - lastPos.x;
        float yChange = transform.position.y - lastPos.y;
        // Idle Sprites
        if (xChange == 0 && yChange == 0) {
            // Up
            if (lastDirection == "up") {
                ChangeAnimationState("EnemyIdleUp");
            }
            // Down
            if (lastDirection == "down") {
                ChangeAnimationState("EnemyIdleDown");
            }
            else {
                ChangeAnimationState("EnemyIdleLeft");
                // Right
                if (lastDirection == "right") {
                    spriteRenderer.flipX = true;
                }
                // Left
                if (lastDirection == "left") {
                    spriteRenderer.flipX = false;
                }
            }
        }
        // Walking Sprites
        else if (Mathf.Abs(xChange) > Mathf.Abs(yChange)) {
            ChangeAnimationState("EnemyWalkLeft");
            // Right
            if(xChange > 0) {
                spriteRenderer.flipX = true;
                lastDirection = "right";
            }
            // Left
            else {
                spriteRenderer.flipX = false;
                lastDirection = "left";
            }
        }
        else {
            // Up
            if (yChange > 0) {
                ChangeAnimationState("EnemyWalkUp");
                lastDirection = "up";
            }
            // Down
            else {
                // TODO: Change once enemy walk down exists
                ChangeAnimationState("EnemyWalkUp");
                lastDirection = "down";
            }
        }
        lastPos = transform.position;
    }

    void ChangeLightRotation() {
        if (lastDirection == "up") {
            lightMaterial.SetTexture("_MainTex", upLight);
            if (lightCollider.rotation != Quaternion.Euler(0f,0f,0f))
                lightCollider.rotation = Quaternion.Euler(0f,0f,0f);
        }
        // Down
        if (lastDirection == "down") {
            lightMaterial.SetTexture("_MainTex", downLight);
            if (lightCollider.rotation != Quaternion.Euler(0f,0f,180f))
                lightCollider.rotation = Quaternion.Euler(0f,0f,180f);
        }
        // Right
        if (lastDirection == "right") {
            lightMaterial.SetTexture("_MainTex", rightLight);
            if (lightCollider.rotation != Quaternion.Euler(0f,0f,-90f))
                lightCollider.rotation = Quaternion.Euler(0f,0f,-90f);
        }
        // Left
        if (lastDirection == "left") {
            lightMaterial.SetTexture("_MainTex", leftLight);
            if (lightCollider.rotation != Quaternion.Euler(0f,0f,90f))
                lightCollider.rotation = Quaternion.Euler(0f,0f,90f);
        }
    }

    /**
    * Moves the enemy along the waypoints in a pre-created path
    */
    void WalkPath() {
        // Reach first waypoint
        if (waypointIndex >= waypoints.Length) {
            waypointIndex = 0;
            pausing = true;
            pauseTime = Time.time;
        }
        // Walk to next waypoint
        if (!pausing) {
            Vector3 target = waypoints[waypointIndex].position;

            transform.position = Vector2.MoveTowards(transform.position, target, (speed * Time.deltaTime));
            if (transform.position.x == target.x && transform.position.y == target.y) {
                waypointIndex++;
            }
        }
        // Count down pause on first waypoint
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

    /**
    * Changes which animation the enemy is using
    */
    void ChangeAnimationState(string state) {
        if (currentState != state) {
            animator.Play(state);
            currentState = state;
        }
    }
}
