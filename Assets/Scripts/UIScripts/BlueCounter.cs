using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlueCounter : MonoBehaviour
{
    public int BlueSouls = 0;
    public GameObject SoulsCounterText;
    Animator animator;
    string currentState;
    // Start is called before the first frame update
    void Start()
    {
        SoulsCounterText.GetComponent<TMP_Text>().text = "0";
        currentState = "BlueCounterEmpty";
        animator = GetComponent<Animator>();   
    }

    // Update is called once per frame
    void Update()
    {
        SoulsCounterText.GetComponent<TMP_Text>().text = BlueSouls.ToString();
        if(BlueSouls != 0){
            if (currentState != "BlueCounterFull") {
                animator.Play("BlueCounterFull");
                currentState = "BlueCounterFull";
            }
        }
    }
    public void AddSoulAmount(int Amount){
        BlueSouls+= Amount;
    }
    }



