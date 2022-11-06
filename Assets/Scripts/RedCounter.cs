using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RedCounter : MonoBehaviour
{
    public int RedSouls = 0;
    public GameObject SoulsCounterText;
    Animator animator;
    string currentState;
    // Start is called before the first frame update
    void Start()
    {
        SoulsCounterText.GetComponent<TMP_Text>().text = "0";
        currentState = "RedCounterEmpty";
        animator = GetComponent<Animator>();   
    }

    // Update is called once per frame
    void Update()
    {
        SoulsCounterText.GetComponent<TMP_Text>().text = RedSouls.ToString();
        if(RedSouls != 0){
            if (currentState != "RedCounterFull") {
                animator.Play("RedCounterFull");
                currentState = "RedCounterFull";
            }
        }
    }
    public void AddSoulAmount(int Amount){
        RedSouls+= Amount;
    }
    }


