using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gateway : MonoBehaviour
{
    Animator animator;
    string currentState;
    int enemies;

    AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        enemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        audioSource = GetComponent<AudioSource>();
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
            ChangeAnimationState("PortalOpeningRed");
            gameObject.tag = "Finish";
        }
    }

    void ChangeAnimationState(string state) {
        if (currentState != state) {
            animator.Play(state);
            currentState = state;
            audioSource.Play(0);
        }
    }
}
