using System.Collections;
using System.Collections.Generic;
// using System;
using UnityEngine;

// Some animation help from: https://www.youtube.com/watch?v=hkaysu1Z-N8 

public class PlayerScript : MonoBehaviour
{
    public int dashes = 3;
    float speed = 2f;
    float dashTimer = -1f;
    float dashReloadTime = -1f;

    Rigidbody2D rigidBody;
    Animator animator;
    SpriteRenderer spriteRenderer;

    float diagSpeed;
    string currentState;

    public GameObject weapon;
    string lastDirection;
    // Start is called before the first frame update
    void Start()
    {        
        rigidBody = GetComponent<Rigidbody2D>();  
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        diagSpeed = (float) Mathf.Sqrt((speed * speed) / 2); 
    }

    // Update is called once per frame
    void Update()
    {
        CheckMvmt();
        CheckAction();
        DashTimers();
    }

    void CheckMvmt() {
        Vector2 movement = new Vector2(0, 0);

        // Not moving
        if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.A) &&
            !Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.D) && 
            !Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.W) &&
            !Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.S)) {
            movement = Vector2.zero;
            if (lastDirection == "up") {
                ChangeAnimationState("PlayerIdleUp");
            }
            else if (lastDirection == "down") {
                ChangeAnimationState("PlayerIdleDown");
            }
            else {
                ChangeAnimationState("PlayerIdleLeft");
                if (lastDirection == "right") {
                    spriteRenderer.flipX = true;
                }
                else {
                    spriteRenderer.flipX = false;
                }
            }  
        }
        // Only moving vertically
        else if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.A) &&
                 !Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.D)){
            movement.x = 0;
            // Up
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                // ChangeAnimationState("PlayerRunningUp");
                movement.y = speed;
                lastDirection = "up";
            }
            // Down
            else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                ChangeAnimationState("PlayerRunningDown");
                movement.y = -speed;
                lastDirection = "down";
            }
        }
        // Only moving horizontally
        else if (!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.W) &&
                 !Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.S)){
            movement.y = 0;
            ChangeAnimationState("PlayerRunningRight");
            // Left
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                spriteRenderer.flipX = true;
                movement.x = -speed;
                lastDirection = "left";
            }
            // Right
            else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                spriteRenderer.flipX = false;
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
        rigidBody.velocity = movement;
    }

    void CheckAction() {
        if (Input.GetKey(KeyCode.Z))
        {
            weapon.transform.position = new Vector2(transform.position.x + .1f, transform.position.y + .1f);
            weapon.GetComponent<Rigidbody2D>().velocity = rigidBody.velocity;
            weapon.SetActive(true);
            
            // Attack
        }
        else if (Input.GetKey(KeyCode.C))
        {
            // Purify
        }

        // Dash
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            if (dashes > 0 && (rigidBody.velocity.x == 0 || rigidBody.velocity.y == 0)) {
                speed *= 2;
                dashTimer = Time.time;
                dashes--;
                dashReloadTime = dashTimer;
            }
        }
    }

    void DashTimers() {
        if (dashTimer != -1f) {
            float elapsedTime = Time.time - dashTimer;
            if (elapsedTime >= 0.4f) {
                speed /= 2f;
                dashTimer = -1f;
            }
        }
        if (dashReloadTime != -1f) {
            float elapsedTime = Time.time - dashReloadTime;
            if (elapsedTime >= 5f) {
                dashes++;
                if (dashes < 3) {
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

    void ChangeAnimationState(string state) {
        if (currentState != state) {
            animator.Play(state);
            currentState = state;
        }
    }
}
