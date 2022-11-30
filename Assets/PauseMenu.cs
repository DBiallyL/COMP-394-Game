using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
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
        if(Time.timeScale <= 0){
            render.material.SetColor("_Color", new Color(1.0f, 1.0f, 1.0f, 1.0f));
        } else {
            render.material.SetColor("_Color", new Color(1.0f, 1.0f, 1.0f, 0.0f));
        }

    }
}
