using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GremlinBehavior : MonoBehaviour
{
    public GameObject player;
    Animator animator;
    SpriteRenderer spriteRenderer;
    string currentState;
    string lastDirection = "Left";
    Color defaultColor;
    Rigidbody2D rigidBody;
    float knockbackSpeed = 6f;
    bool animationPlaying = false;
    bool canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultColor = spriteRenderer.material.color;
        currentState = "chasing";
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    void ChangeAnimationState(string state) {
        if (currentState != state) {
            animator.Play(state);
            currentState = state;
        }
    }
}
