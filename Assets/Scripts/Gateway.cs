using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gateway : MonoBehaviour
{
    Animator animator;
    string currentState;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        open();
    }

    void open() {
        if(GameObject.FindGameObjectsWithTag("Enemy").Length == 0) {
            ChangeAnimationState("PortalOpeningRed");
            gameObject.tag = "Finish";
        }
    }

    void ChangeAnimationState(string state) {
        if (currentState != state) {
            animator.Play(state);
            currentState = state;
        }
    }
}
