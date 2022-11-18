using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverlay : MonoBehaviour
{
    private Renderer render;
    private float alpha = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        render = this.GetComponent<Renderer>();
        render.material.SetColor("_Color", new Color(1.0f, 1.0f, 1.0f, alpha));
    }

    // Update is called once per frame
    void Update()
    {
        if(alpha > 0){
            alpha-=0.01f;
            render.material.SetColor("_Color", new Color(1.0f, 1.0f, 1.0f, alpha));
        }

    }
    public void MakeVisible(){
        alpha = 1.0f;
    }
}
