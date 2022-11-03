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
    bool followingPlayer = false;
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
    string lastDirection = "Left";

    // Global variables to handle losing health
    public int health = 2;
    public int statAttackStrength = 1;
    public int runAttackStrength = 2;

    // Global variables to handle attacking player
    public GameObject player;

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
        if (!followingPlayer) WalkPath();  
        else FollowPlayer(); 
        ChangeWalkSprites(followingPlayer);
        ChangeLightRotation();
        CheckDead();
    }

    /**
    * Changes the sprites to be idle or walk sprites based on the change in position in update
    */
    void ChangeWalkSprites(bool followingPlayer) {
        float xChange = transform.position.x - lastPos.x;
        float yChange = transform.position.y - lastPos.y;
        string spritePath = "Enemy";

        // Idle Sprites
        if (xChange == 0 && yChange == 0) {
            spritePath += "Idle";
        }
        // Walking Sprites
        else {
            spritePath += "Walk";
            if (Mathf.Abs(xChange) > Mathf.Abs(yChange)) {
                // Right
                if(xChange > 0) {
                    lastDirection = "Right";
                }
                // Left
                else {
                    lastDirection = "Left";
                }
            }
            else {
                // Up
                if (yChange > 0) {
                    lastDirection = "Up";
                }
                // Down
                else {
                    lastDirection = "Down";
                }
            }
        }

        if (followingPlayer) {
            spritePath += "Enraged";
        }

        // Right
        if (lastDirection == "Right") {
            ChangeAnimationState(spritePath + "Left");
            spriteRenderer.flipX = true;
        }
        // Left
        else if (lastDirection == "Left") {
            ChangeAnimationState(spritePath + "Left");
            spriteRenderer.flipX = false;
        }
        // Up and Down
        else {
            ChangeAnimationState(spritePath + lastDirection);
        }

        lastPos = transform.position;
    }

    /**
    * Changes the rotation of the HardLight2D light and the light collider in update
    */
    void ChangeLightRotation() {
        // Up
        if (lastDirection == "Up") {
            lightMaterial.SetTexture("_MainTex", upLight);
            if (lightCollider.rotation != Quaternion.Euler(0f,0f,0f))
                lightCollider.rotation = Quaternion.Euler(0f,0f,0f);
        }
        // Down
        if (lastDirection == "Down") {
            lightMaterial.SetTexture("_MainTex", downLight);
            if (lightCollider.rotation != Quaternion.Euler(0f,0f,180f))
                lightCollider.rotation = Quaternion.Euler(0f,0f,180f);
        }
        // Right
        if (lastDirection == "Right") {
            lightMaterial.SetTexture("_MainTex", rightLight);
            if (lightCollider.rotation != Quaternion.Euler(0f,0f,-90f))
                lightCollider.rotation = Quaternion.Euler(0f,0f,-90f);
        }
        // Left
        if (lastDirection == "Left") {
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

    /**
    * Handles how the enemy follows the player when enraged
    */
    void FollowPlayer() {
        if (!pausing) {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, (speed * Time.deltaTime));         
        }
        else {
            float elapsedTime = Time.time - pauseTime;
            if (elapsedTime > 1) {
                pausing = false;
                pauseTime = -1f;
            }
        }

        float distFromPlayer = Vector2.Distance(player.transform.position, transform.position); 
        if (distFromPlayer > -1f && distFromPlayer < 1f) {
            player.SendMessage("TakeDamage");
            pausing = true;
            pauseTime = Time.time;
        }
        // else if (distFromPlayer > 10f) {
        //     followingPlayer = false;
        // }
    }

    /**
    * Handles how much health the enemy loses when attacked by the player
    */
    void LoseHealth(bool isRunning) {
        if (isRunning) {
            health -= runAttackStrength;
        }
        else {
            health -= statAttackStrength;
        }
    }

    /**
    * Checks if the enemy has died
    */
    void CheckDead() {
        if (health <= 0) {
            Destroy(gameObject);
        }
    }

    /**
    * Handles how the enemy responds to the player entering their viewcone collider
    */
    void GetAngry() {
        print("Get Angry");
        Vector2 direction = player.transform.position - transform.position;
        RaycastHit2D[] ray = Physics2D.RaycastAll(transform.position, direction);
        if (ray[2].collider.CompareTag("Player")) {
            followingPlayer = true;
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
