using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScript : MonoBehaviour
{
    string currentState;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /**
    * Quits the game
    */
    public void QuitGame() {
        Application.Quit();
    }

    /**
    * Starts the animation to retry the game
    */
    public void Retry() {
        ChangeAnimationState("GameOverRetry");
    }

    /**
    * Changes which animation the object is using
    */
    void ChangeAnimationState(string state) {
        if (currentState != state) {
            animator.Play(state);
            currentState = state;
        }
    }
}
