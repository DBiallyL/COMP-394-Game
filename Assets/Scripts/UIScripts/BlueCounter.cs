using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlueCounter : MonoBehaviour
{
    public int BlueSouls = 0;
    public GameObject BlueText;
    // public GameObject SoulsCounterText;
    Animator animator;
    string currentState;
    string BlueTextString;

    // Start is called before the first frame update
    void Start()
    {
        // SoulsCounterText.GetComponent<TMP_Text>().text = "0";
        currentState = "BlueCounterEmpty";
        animator = GetComponent<Animator>();
        BlueTextString = "0";
        BlueText.GetComponent<TMP_Text>().text = BlueTextString;   
    }

    // Update is called once per frame
    void Update()
    {
        BlueTextString = BlueSouls.ToString();
        BlueText.GetComponent<TMP_Text>().text = BlueTextString;
        // SoulsCounterText.GetComponent<TMP_Text>().text = BlueSouls.ToString();
        if(BlueSouls != 0){
            if (currentState != "BlueCounterFull") {
                animator.Play("BlueCounterFull");
                currentState = "BlueCounterFull";
            }
        }

    }
    public void AddSoulAmount(int Amount){
        BlueSouls+= 1;
        BlueText.GetComponent<TMP_Text>().text = BlueTextString;
    }
    public void RemoveSoulAmount(int Amount){
        BlueSouls-= 1;
        BlueText.GetComponent<TMP_Text>().text = BlueTextString;
    }
    }



