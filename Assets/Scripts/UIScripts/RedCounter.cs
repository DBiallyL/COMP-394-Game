using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RedCounter : MonoBehaviour
{
 public int RedSouls = 0;
    public GameObject RedText;
    // public GameObject SoulsCounterText;
    Animator animator;
    string currentState;
    string RedTextString;

    // Start is called before the first frame update
    void Start()
    {
        // SoulsCounterText.GetComponent<TMP_Text>().text = "0";
        currentState = "RedCounterEmpty";
        animator = GetComponent<Animator>();
        RedTextString = "10";
        RedText.GetComponent<TMP_Text>().text = RedTextString;   
    }

    // Update is called once per frame
    void Update()
    {
        RedTextString = RedSouls.ToString();
        RedText.GetComponent<TMP_Text>().text = RedTextString;
        // SoulsCounterText.GetComponent<TMP_Text>().text = BlueSouls.ToString();
        if(RedSouls != 0){
            if (currentState != "RedCounterFull") {
                animator.Play("RedCounterFull");
                currentState = "RedCounterFull";
            }
        }

    }
    public void AddSoulAmount(int Amount){
        RedSouls+= 1;
        RedText.GetComponent<TMP_Text>().text = RedTextString;
    }
    public void RemoveSoulAmount(int Amount){
        RedSouls-= 1;
        RedText.GetComponent<TMP_Text>().text = RedTextString;
    }
    }


