using System.Collections;
using System.Collections.Generic;
// using System;
using UnityEngine;

// Some animation help from: https://www.youtube.com/watch?v=hkaysu1Z-N8 

public class PlayerScript : MonoBehaviour
{
    // Global variables to deal with health and dying
    public int health = 5;
    bool dead = false;
    float immuneLength = 0.4f;
    float immuneTime = -1f;
    Color defaultColor;

    // Global variables used to handle movement
    float speed = 3f;
    float ogSpeed;
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

    // Global variables used to handle animation
    Animator animator;
    SpriteRenderer spriteRenderer;
    string currentState;
    string lastDirection = "Right";
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
        defaultColor = spriteRenderer.material.color;

        diagSpeed = (float) Mathf.Sqrt((speed * speed) / 2); 
        maxDashes = dashes;
        ogSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead) {
            if (canMove) CheckMvmt();
            CheckAction();
            Timers();
            CheckDead();
        }
    }

    /**
    * Handles the player and it's sprite as different keys are pressed
    */
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

    /**
    * Handles the player and it's animation when a movement key is pressed
    * TODO: Maybe split up diagonal and non-diagonal into different methods
    * TODO: See if I can factor it better
    */
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
                lastDirection = "Up";
            }
            // Down
            else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                movement.y = -speed;
                lastDirection = "Down";
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
                lastDirection = "Left";
            }
            // Right
            else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                movement.x = speed;
                lastDirection = "Right";
            }
        }   

        // Moving diagonally     
        else {
            // Up and left
            if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && 
                (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))) {
                    movement.x = -diagSpeed;
                    movement.y = diagSpeed;
                    lastDirection = "Left";
            }
            // Up and right
            else if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && 
                    (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))) {
                    movement.x = diagSpeed;
                    movement.y = diagSpeed;
                    lastDirection = "Right";
            }
            // Down and left
            else if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && 
                    (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))) {
                    movement.x = -diagSpeed;
                    movement.y = -diagSpeed;
                    lastDirection = "Left";
            }
            // Down and right
            else if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && 
                    (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))) {
                    movement.x = diagSpeed;
                    movement.y = -diagSpeed;
                    lastDirection = "Right";
            }
        }

        return movement;
    }

    /**
    * Changes the player's animation based on some direction it's facing
    */
    void ChangeToDirectionalAnimation(string spritePath) {
        spriteRenderer.flipX = false;
        // Left
        if (lastDirection == "Left") { 
            ChangeAnimationState(spritePath + "Right");
            spriteRenderer.flipX = !michaelJacksonMode; 
        }
        // Right
        else if (lastDirection == "Right") {
            ChangeAnimationState(spritePath + "Right");
            spriteRenderer.flipX = michaelJacksonMode;
        }
        // Up and Down
        else {
            ChangeAnimationState(spritePath + lastDirection);
        }
    }

    /**
    * Checks if the player hit a key for some non-movement action
    */
    void CheckAction() {
        // Attack
        if (Input.GetKey(KeyCode.Z))
        {
            string motion = "";
            if (rigidBody.velocity != Vector2.zero) {
                motion = "Running";
            }
            else {
                motion = "Stationary";
            }
            ChangeToDirectionalAnimation("Player" + motion + "Attack");
            weapon.SendMessage(motion + "Attack", lastDirection);
            canMove = false;
        }
        // Ritual
        else if (Input.GetKey(KeyCode.C))
        {
            if (!pressedC) {
                canMove = false;
                ChangeAnimationState("PlayerRitualStartUp");
                pressedC = true;
            }
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
    * Hard-coded translations so that attack animations line up with movement animations
    */
    void FixAttackPlacement() {
        if (lastDirection == "Up") {
            transform.Translate(new Vector3(.25f, .4f, 0f));
        }
        else if (lastDirection == "Right") {
            transform.Translate(new Vector3(.4f, .2f, 0f));
        }
        else if (lastDirection == "Left") {
            transform.Translate(new Vector3(-.4f, .2f, 0f));
        }
    }

    /**
    * Resets any translations done during attack animations
    */
    void ResetAttackPlacement() {
        if (lastDirection == "Up") {
            transform.Translate(new Vector3(-.25f, -.4f, 0f));
        }
        else if (lastDirection == "Right") {
            transform.Translate(new Vector3(-.4f, -.2f, 0f));
        }
        else if (lastDirection == "Left") {
            transform.Translate(new Vector3(.4f, -.2f, 0f));
        }
    }

    /**
    * Divides the velocity by 1.5
    */
    void SlowToStop() {
        float newXVel = rigidBody.velocity.x / 1.5f;
        float newYVel = rigidBody.velocity.y / 1.5f;
        rigidBody.velocity = new Vector2(newXVel,newYVel);
    }

    /**
    * Used at the end of any attack animation to re-allow movement and reset the weapon
    */
    void EndAttack() {
        canMove = true;
        weapon.SendMessage("ResetWeapon");
    }

    /**
    * Used at the end of the horizontal running attack animation, allows the player to move and moves the player back a
    * little so the animation looks smoother
    *
    * Looks good at least with base speed 3f
    */
    void EndRightRunAttack() {
        transform.position = new Vector2(transform.position.x - rigidBody.velocity.x, transform.position.y);
        EndAttack();
    }

    /**
    * Handles the player taking damage and temporary immunity
    */
    void TakeDamage() {
        if (immuneTime == -1f) {
            health--;
            spriteRenderer.material.SetColor("_Color", Color.red);
            immuneTime = Time.time;
        }
    }

    /**
    * Checks if the player's health is at or below 0, and carries out the appropriate actions 
    */
    void CheckDead() {
        if (health <= 0) {
            ChangeAnimationState("PlayerDying");
            dead = true;
        }
    }

    /**
    * Keeps track of all timers (for dash duration, dash reloading, and immunity)
    * TODO: Test if it's good timing for actual gameplay
    */
    void Timers() {
        float time = Time.time;

        if (dashTimer != -1f) {
            float elapsedTime = time - dashTimer;
            if (elapsedTime >= dashLength) {
                speed = ogSpeed;
                dashTimer = -1f;
            }
        }
        if (dashReloadTime != -1f) {
            float elapsedTime = time - dashReloadTime;
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

        if (immuneTime != -1f) {
            float elapsedTime = time - immuneTime;
            if (elapsedTime >= immuneLength) {
                immuneTime = -1f;
                spriteRenderer.material.SetColor("_Color", defaultColor);
            }
        }
    }

    /**
    * Changes which animation the player is using
    */
    void ChangeAnimationState(string state) {
        if (currentState != state) {
            animator.Play(state);
            currentState = state;
        }
    }
}
