using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyInterface : MonoBehaviour
{
    // For animation
    public SpriteRenderer spriteRenderer;
    public string lastDirection;
    public Animator animator;
    public string currentState;

    // For knockback
    public float knockbackSpeed;
    public Rigidbody2D rigidBody;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnemyKnockback(string direction) {
        Vector2 knockbackVelocity = Vector2.zero;
        if (direction == "Down") {
            knockbackVelocity.y = -knockbackSpeed;
        }
        else if (direction == "Up") {
            knockbackVelocity.y = knockbackSpeed;
        }
        else if (direction == "Left") {
            knockbackVelocity.x = -knockbackSpeed;
        }
        else {
            knockbackVelocity.x = knockbackSpeed;
        }
        rigidBody.velocity = knockbackVelocity;
    }

    /**
    * Changes the enemy's animation based on some direction it's facing
    * spritePath is the file path of some enemy sprite which has Up/Down/Left options, without Up/Down/Left on the end
    */
    public void ChangeToDirectionalAnimation(string spritePath) {
        spriteRenderer.flipX = false;
        // Left
        if (lastDirection == "Left") { 
            ChangeAnimationState(spritePath + "Left");
            spriteRenderer.flipX = false; 
        }
        // Right
        else if (lastDirection == "Right") {
            ChangeAnimationState(spritePath + "Left");
            spriteRenderer.flipX = true;
        }
        // Up and Down
        else {
            ChangeAnimationState(spritePath + lastDirection);
        }
    }

    /**
    * Changes which animation the object is using
    */
    public void ChangeAnimationState(string state) {
        if (currentState != state) {
            animator.Play(state);
            currentState = state;
        }
    }

    /**
    * Called by the dead animation to destroy the enemy's game object
    */
    void DestroyEnemy() {
        Destroy(gameObject);
    }
}
