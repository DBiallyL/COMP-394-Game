using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    // Global variables used to handle animation
    Collider2D collider;
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void RunningAttack(string lastDirection) {
        if (lastDirection == "Right") {
            collider.offset = new Vector2(0.2f, 0f);
        }
        else if (lastDirection == "Left") {
            collider.offset = new Vector2(-0.2f, 0f);
        }
        else if (lastDirection == "Up") {
            collider.offset = new Vector2(0f, 0.25f);
        }
        else {
            collider.offset = new Vector2(0f, -0.15f);
        }
    }

    void ResetWeapon() {
        collider.offset = Vector2.zero;
    }
}
