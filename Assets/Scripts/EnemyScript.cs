using System.Collections;
using System.Collections.Generic;
// using System;
using UnityEngine;

// Paths coded with help from: https://www.youtube.com/watch?v=KoFDDp5W5p0 
// Animation coded with help from: https://www.youtube.com/watch?v=nBkiSJ5z-hE, https://answers.unity.com/questions/952558/how-to-flip-sprite-horizontally-in-unity-2d.html
// Enemy light cone uses asset HardLight2D: https://assetstore.unity.com/packages/tools/particles-effects/hard-light-2d-152208

public class EnemyScript : MonoBehaviour
{
    // Global variables used to handle path walking
    public Transform[] waypoints;
    public float speed = 0.01f;
    int waypointIndex = 1;
    bool pausing = false;
    float pauseTime = -1f;
    
    // Global variables used to handle light cone
    public Texture leftLight, upLight, rightLight, downLight;
    Material lightMaterial;
    Transform lightCollider;

    // Global variables used to keep track of direction and animation states
    Animator animator;
    SpriteRenderer spriteRenderer;
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

    /**
    * Changes the sprites to be idle or walk sprites based on the change in position in update
    */
    void ChangeWalkSprites() {
        float xChange = transform.position.x - lastPos.x;
        float yChange = transform.position.y - lastPos.y;
        string spritePath = "Enemy";

        // Idle Sprites
        if (xChange == 0 && yChange == 0) {
            spritePath += "Idle";
        }
        // Walking Sprites
        else if (Mathf.Abs(xChange) > Mathf.Abs(yChange)) {
            spritePath += "Walking";
            // Right
            if(xChange > 0) {
                lastDirection = "right";
            }
            // Left
            else {
                lastDirection = "left";
            }
        }
        else {
            // Up
            if (yChange > 0) {
                lastDirection = "up";
            }
            // Down
            else {
                lastDirection = "down";
            }
        }

        // Up
        if (lastDirection == "up") {
            ChangeAnimationState(spritePath + "Up");
        }
        // Down
        if (lastDirection == "down") {
            ChangeAnimationState(spritePath + "Down");
        }
        else {
            ChangeAnimationState(spritePath + "Left");
            // Right
            if (lastDirection == "right") {
                spriteRenderer.flipX = true;
            }
            // Left
            if (lastDirection == "left") {
                spriteRenderer.flipX = false;
            }
        }

        lastPos = transform.position;
    }

    /**
    * Changes the rotation of the HardLight2D light and the light collider in update
    */
    void ChangeLightRotation() {
        // Up
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
