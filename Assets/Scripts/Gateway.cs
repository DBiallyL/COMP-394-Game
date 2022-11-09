using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gateway : MonoBehaviour
{
    Animator animator;
    string currentState;
    int enemies;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        enemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
    }

    // Update is called once per frame
    void Update()
    {
        open();
    }

    void open() {
        if(enemies > 0) {
            enemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        } else if(enemies == 0){
            enemies = -1;
            Debug.Log("Portal Opening");
            ChangeAnimationState("PortalOpeningRed");
            gameObject.tag = "Finish";
        }
    }

    void ChangeAnimationState(string state) {
        Debug.Log("Changing Animation State");
        if (currentState != state) {
            animator.Play(state);
            currentState = state;
        }
    }
}
