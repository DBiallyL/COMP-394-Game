using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LoseHealth(float percent){
        transform.localScale -= new Vector3(3 * percent, 0, 0);
        transform.position -= new Vector3(1.46f * percent,0,0);
    }
}
