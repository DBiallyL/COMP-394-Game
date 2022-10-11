using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckMovement();
        CheckAction();
    }

    void CheckMovement() {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
           
        }
        else if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            
        }
        else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            
        }
    }

    void CheckAction() {
        if (Input.GetKey(KeyCode.Z))
        {
            // Attack
        }
        else if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            // Dash
        }
        else if (Input.GetKey(KeyCode.C))
        {
            // Purify
        }
    }

    // For attacks/healing will it be handled with collisions or is there a better way to do it?
    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.CompareTag("Enemy")) {
            // Check if in attack position or not (maybe?)
            // if in attack position enemy loses health/dies
            // else if in healing position instantiate healing process (maybe? does the player always win out then? does the enemy need
                // a special condition (back turned) for this to work?)
            // else player loses health
        }
    }

}
