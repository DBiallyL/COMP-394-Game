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

    string currentState;
    Vector3 lastPos;
    string lastDirection;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Transform child = transform.Find("Light2D");

        transform.position = waypoints[0].position;
        lastPos = transform.position;
        lightMaterial = child.GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        WalkPath();   
        float xChange = transform.position.x - lastPos.x;
        float yChange = transform.position.y - lastPos.y;
        if (xChange == 0 && yChange == 0) {
            if (lastDirection == "up") {
                ChangeAnimationState("EnemyIdleUp");
                lightMaterial.SetTexture("_MainTex", upLight);
            }
            if (lastDirection == "down") {
                ChangeAnimationState("EnemyIdleDown");
                lightMaterial.SetTexture("_MainTex", downLight);
            }
            else {
                ChangeAnimationState("EnemyIdleLeft");
                if (lastDirection == "left") {
                    spriteRenderer.flipX = false;
                    lightMaterial.SetTexture("_MainTex", leftLight);
                }
                if (lastDirection == "right") {
                    spriteRenderer.flipX = true;
                    lightMaterial.SetTexture("_MainTex", rightLight);
                }
            }

        }
        else if (Mathf.Abs(xChange) > Mathf.Abs(yChange)) {
            ChangeAnimationState("EnemyWalkLeft");
            if(xChange > 0) {
                spriteRenderer.flipX = true;
                lastDirection = "right";
                lightMaterial.SetTexture("_MainTex", rightLight);
            }
            else {
                spriteRenderer.flipX = false;
                lastDirection = "left";
                lightMaterial.SetTexture("_MainTex", leftLight);
            }
        }
        else {
            ChangeAnimationState("EnemyWalkUp");
            if (yChange > 0) {
                lastDirection = "up";
                lightMaterial.SetTexture("_MainTex", upLight);
            }
            else {
                lastDirection = "down";
                lightMaterial.SetTexture("_MainTex", downLight);
            }
        }
        lastPos = transform.position;
    }

    void WalkPath() {
        if (waypointIndex >= waypoints.Length) {
            waypointIndex = 0;
            pausing = true;
            pauseTime = Time.time;
        }
        if (!pausing) {
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
