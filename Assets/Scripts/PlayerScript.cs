using System.Collections;
using System.Collections.Generic;
// using System;
using UnityEngine;

// Some animation help from: https://www.youtube.com/watch?v=hkaysu1Z-N8 

public class PlayerScript : MonoBehaviour
{
    // Global variables used to handle path walking

    float speed = 3f;
    float diagSpeed;
    Rigidbody2D rigidBody;

    // Global variables used to handle dashes

    //                                                                                                TODO: Change back to 3 in Unity Editor
    public int dashes = 3;
    int maxDashes;
    // Will be greater than -1f if the player is dashing
    float dashTimer = -1f;
    // Will be greater than -1f if the player has less than the maximum number of dashes
    float dashReloadTime = -1f;
    int dashMultiplier = 3;
    float dashLength = 0.3f;
    float dashReloadLength = 4f;

    // Global variables used to handle dashes

    Animator animator;
    SpriteRenderer spriteRenderer;
    string currentState;
    string lastDirection;
    // Michael Jackson Mode
    bool michaelJacksonMode = false;

    // Global variables used to handle weapons and rituals

    public GameObject weapon;
    bool canMove = true;
    bool pressedC = false;

    // Start is called before the first frame update
    void Start()
    {        
        rigidBody = GetComponent<Rigidbody2D>();  
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        diagSpeed = (float) Mathf.Sqrt((speed * speed) / 2); 
        maxDashes = dashes;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove) CheckMvmt();
        CheckAction();
        DashTimers();
    }

    void CheckMvmt() {
        Vector2 movement = new Vector2(0, 0);
        string spritePath = "Player";

        // Not moving
        if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.A) &&
            !Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.D) && 
            !Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.W) &&
            !Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.S)) {
            movement = Vector2.zero;
            spritePath += "Idle";
        }
        // Moving
        else {
            movement = AnimateRunning();
            if (dashTimer == -1f) {
                spritePath += "Running";
            }
            else {
                spritePath += "Dashing";
            }
        }

        // Set Animation State
        ChangeToDirectionalAnimation(spritePath);

        rigidBody.velocity = movement;
    }

    Vector2 AnimateRunning() {
        Vector2 movement = new Vector2(0,0);

        // Only moving vertically
        if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.A) &&
            !Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.D)){
            movement.x = 0;
            // Up
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                movement.y = speed;
                lastDirection = "up";
            }
            // Down
            else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                movement.y = -speed;
                lastDirection = "down";
            }
        }
        // Only moving horizontally
        else if (!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.W) &&
                 !Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.S)){
            movement.y = 0;
            // Left
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                movement.x = -speed;
                lastDirection = "left";
            }
            // Right
            else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                movement.x = speed;
                lastDirection = "right";
            }
        }   
        // Moving diagonally     
        else {
            // Up and left
            if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && 
                (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))) {
                    movement.x = -diagSpeed;
                    movement.y = diagSpeed;
                    lastDirection = "left";
            }
            // Up and right
            else if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && 
                    (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))) {
                    movement.x = diagSpeed;
                    movement.y = diagSpeed;
                    lastDirection = "right";
            }
            // Down and left
            else if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && 
                    (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))) {
                    movement.x = -diagSpeed;
                    movement.y = -diagSpeed;
                    lastDirection = "left";
            }
            // Down and right
            else if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && 
                    (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))) {
                    movement.x = diagSpeed;
                    movement.y = -diagSpeed;
                    lastDirection = "right";
            }
        }

        return movement;
    }

    /**
    * Checks if the player hit a key for some non-movement action
    */
    void CheckAction() {
        
        if (Input.GetKey(KeyCode.Z))
        {
            weapon.transform.position = new Vector2(transform.position.x + .1f, transform.position.y + .1f);
            weapon.GetComponent<Rigidbody2D>().velocity = rigidBody.velocity;
            weapon.SetActive(true);
            
            // Attack
        }
        else if (!Input.GetKey(KeyCode.C)) {
            // canMove = true;
            // ChangeToDirectionalAnimation("PlayerIdle");
        }
        else if (Input.GetKey(KeyCode.C))
        {
            if (!pressedC) {
                print("Not pressed C");
                canMove = false;
                ChangeAnimationState("PlayerRitualStartUp");
                pressedC = true;
            }
            // Purify
        }

        // Stop Ritual
        if (Input.GetKeyUp(KeyCode.C)) {
            pressedC = false;
            canMove = true;
        }

        // Dash
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            if (dashes > 0 && (rigidBody.velocity.x == 0 || rigidBody.velocity.y == 0)) {
                speed *= dashMultiplier;
                dashTimer = Time.time;
                dashes--;
                dashReloadTime = dashTimer;
            }
        }

        if (Input.GetKeyDown(KeyCode.M)) {
            michaelJacksonMode = true;
        }
    }

    /**
    * Keeps track of the timers for the dash duration and dash reloading
    * TODO: Test if it's good timing for actual gameplay
    */
    void DashTimers() {
        if (dashTimer != -1f) {
            float elapsedTime = Time.time - dashTimer;
            if (elapsedTime >= dashLength) {
                speed /= dashMultiplier;
                dashTimer = -1f;
            }
        }
        if (dashReloadTime != -1f) {
            float elapsedTime = Time.time - dashReloadTime;
            if (elapsedTime >= dashReloadLength) {
                dashes++;
                if (dashes < maxDashes) {
                    dashReloadTime = Time.time;
                }
                else {
                    dashReloadTime = -1f;
                }
            }
        }
    }

    // For attacks/healing there will probably be separate game object to handle collisions
    // attacks can be sword object (visible), purifier can be invisible object always in front of player?
    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.CompareTag("Enemy")) {
            // Check if shielded or not
            // if not player loses health
        }
        if (collision.collider.CompareTag("Wall")) {
            rigidBody.velocity = Vector2.zero;
        }
    }

    void ChangeToDirectionalAnimation(string spritePath) {
        if (lastDirection == "up") {
            ChangeAnimationState(spritePath + "Up");
        }
        else if (lastDirection == "down") {
            ChangeAnimationState(spritePath + "Down");
        }
        else {
            ChangeAnimationState(spritePath + "Right");
            if (lastDirection == "left") { 
                spriteRenderer.flipX = !michaelJacksonMode; 
            }
            else {
                spriteRenderer.flipX = michaelJacksonMode;
            }
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
