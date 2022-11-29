using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RitualBar : MonoBehaviour
{
    float ritualLength = 5f;
    float ritualTimer = -1f;
    float lastTime = -1f;
    float totalPosChange = 0f;
    float xScale;
    float yScale;

    public GameObject outline;
    public GameObject backing;
    SpriteRenderer outlineRenderer;
    SpriteRenderer backingRenderer;
    // Start is called before the first frame update
    void Start()
    {
        outlineRenderer = outline.GetComponent<SpriteRenderer>();
        backingRenderer = backing.GetComponent<SpriteRenderer>();

        xScale = transform.localScale.x;
        yScale = transform.localScale.y;
        transform.localScale = new Vector2(0, yScale);
    }

    // Update is called once per frame
    void Update()
    {
        if (ritualTimer != -1f) {
            float time = Time.time;

            float totalPercent = (time - ritualTimer) / ritualLength;
            transform.localScale = new Vector3(totalPercent * xScale, yScale, 0);

            float recentPercent = (time - lastTime) / ritualLength;
            float posChange = 0.32f * recentPercent;
            transform.position += new Vector3(posChange, 0, 0);
            totalPosChange += posChange;
            lastTime = time;
        }
    }

    void StartRitual() {
        ritualTimer = Time.time;
        lastTime = ritualTimer;

        outlineRenderer.enabled = true;
        backingRenderer.enabled = true;
    }

    void StopRitual() {
        ritualTimer = -1f;
        transform.localScale = new Vector2(0, yScale);
        transform.position -= new Vector3(totalPosChange, 0, 0);
        totalPosChange = 0f;

        outlineRenderer.enabled = false;
        backingRenderer.enabled = false;
    }
}
