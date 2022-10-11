using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    float speed = 2f;
    float diagSpeed;
    Rigidbody2D rigidBody;
    // Start is called before the first frame update
    void Start()
    {        
        rigidBody = GetComponent<Rigidbody2D>();  
        diagSpeed = (float) Math.Sqrt((speed * speed) / 2); 
    }

    // Update is called once per frame
    void Update()
    {
        CheckMvmt();
        CheckAction();
    }

    void CheckMvmt() {
        Vector2 movement = new Vector2(0, 0);

        // Not moving
        if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.A) &&
            !Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.D) && 
            !Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.W) &&
            !Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.S)) {
            movement = Vector2.zero;
        }
        // Only moving vertically
        else if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.A) &&
                 !Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.D)){
            movement.x = 0;
            // Up
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                movement.y = speed;
            }
            // Down
            else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                movement.y = -speed;
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
            }
            // Right
            else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
            movement.x = speed;
            }
        }   
        // Moving diagonally     
        else {
            // Up and left
            if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && 
                (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))) {
                    movement.x = -diagSpeed;
                    movement.y = diagSpeed;
            }
            // Up and right
            else if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && 
                    (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))) {
                    movement.x = diagSpeed;
                    movement.y = diagSpeed;
            }
            // Down and left
            else if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && 
                    (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))) {
                    movement.x = -diagSpeed;
                    movement.y = -diagSpeed;
            }
            // Down and right
            else if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && 
                    (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))) {
                    movement.x = diagSpeed;
                    movement.y = -diagSpeed;
            }
        }
        rigidBody.velocity = movement;
    }

    void CheckAction() {
        if (Input.GetKey(KeyCode.Z))
        {
            // Attack
        }
        else if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            // Doesn't work for diagonals moving up?????
            Vector2 movement = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y);
            movement.x *= 2;
            movement.y *= 2;
            rigidBody.velocity = movement;
            // Dash
        }
        else if (Input.GetKey(KeyCode.C))
        {
            // Purify
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

}
