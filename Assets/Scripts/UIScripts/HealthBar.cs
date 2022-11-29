using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{

    public GameObject BloodborneBar;
    public float BloodPercent;
    public float HealthPercent;
    Vector3 originalPos;
    Vector3 originalRot;
    public float change;

    // Start is called before the first frame update
    void Start()
    {
        BloodPercent = 1.0f;
        HealthPercent = 1.0f;
        originalPos = transform.position;
        originalRot = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(BloodPercent>1.0f){
            change = BloodPercent - 1.0f;
            BloodPercent=1.0f;
            BloodborneBar.transform.localScale -= new Vector3(3 * (change), 0, 0);
            BloodborneBar.transform.position -= new Vector3(1.46f * (change),0,0);
        }
        if(HealthPercent>1.0f){
            change = HealthPercent - 1.0f;
            HealthPercent=1.0f;
            transform.localScale -= new Vector3(3 * (change), 0, 0);
            transform.position -= new Vector3(1.46f * (change),0,0);
        }
        if(BloodPercent<HealthPercent){
            BloodPercent=HealthPercent;
            BloodborneBar.transform.localScale = transform.localScale;
            BloodborneBar.transform.position = transform.position;
        }
        if(BloodPercent>HealthPercent){
            BloodPercent-=0.0001f;
            BloodborneBar.transform.localScale -= new Vector3(3 * (.0001f), 0, 0);
            BloodborneBar.transform.position -= new Vector3(1.46f * (.0001f),0,0);
        }
    }

    public void LoseHealth(float percent){
        HealthPercent-=percent;
        transform.localScale -= new Vector3(3 * percent, 0, 0);
        transform.position -= new Vector3(1.46f * percent,0,0);
    }
    public void GainHealth(){
        if(HealthPercent < BloodPercent){
            if(HealthPercent+0.1f < BloodPercent){
            transform.localScale += new Vector3(3 * 0.1f, 0, 0);
            transform.position += new Vector3(1.46f * 0.1f,0,0);
            } else {
                change = BloodPercent - HealthPercent;
                transform.localScale += new Vector3(3 * (change), 0, 0);
                transform.position += new Vector3(1.46f * (change),0,0);
            }
    
            
        }
    }
}
