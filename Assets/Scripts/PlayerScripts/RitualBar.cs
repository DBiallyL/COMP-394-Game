using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RitualBar : MonoBehaviour
{
    float ritualLength = 5f;
    float ritualTimer = -1f;
    float xScale;
    float yScale;
    // Start is called before the first frame update
    void Start()
    {
        xScale = transform.localScale.x;
        yScale = transform.localScale.y;
        transform.localScale = new Vector2(0, yScale);
    }

    // Update is called once per frame
    void Update()
    {
        if (ritualTimer != -1f) {
            float percent = (Time.time - ritualTimer) / ritualLength;
            transform.localScale = new Vector2(percent * xScale, yScale);
        }
    }

    void StartRitual() {
        ritualTimer = Time.time;
    }

    void StopRitual() {
        ritualTimer = -1f;
    }
}
