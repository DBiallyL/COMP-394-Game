using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunningAttackScript : StateMachineBehaviour
{
    float timeRemaining = 0.25f;
    float timeBetweenFrames = 0.25f;
    float lastTime = -1f;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Slide function is called at each animation frame
        if (timeRemaining <= 0) {
            animator.SendMessageUpwards("SlowToStop");
            timeRemaining = timeBetweenFrames;
            lastTime = Time.time;
        }
        // Counts down the timer to the next time the slide function should be called
        else {
            float timePassed = Time.time - lastTime;
            timeRemaining -= timePassed;
            lastTime = Time.time;
        }
       
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        lastTime = -1f;
        timeRemaining = timeBetweenFrames;
        animator.SendMessageUpwards("EndAttack");
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
