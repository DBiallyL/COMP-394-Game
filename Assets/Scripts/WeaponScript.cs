using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    // Global variables used to handle animation
    Collider2D weaponCollider;
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        weaponCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void RunningAttack(string lastDirection) {
        if (lastDirection == "Right") {
            weaponCollider.offset = new Vector2(0.2f, 0f);
        }
        else if (lastDirection == "Left") {
            weaponCollider.offset = new Vector2(-0.2f, 0f);
        }
        else if (lastDirection == "Up") {
            weaponCollider.offset = new Vector2(0f, 0.25f);
        }
        else {
            weaponCollider.offset = new Vector2(0f, -0.15f);
        }
    }

    void ResetWeapon() {
        weaponCollider.offset = Vector2.zero;
    }
    
    void OnTriggerEnter2D(Collider2D coll) {
        if (coll.CompareTag("Enemy")) {
            print("Aaaaah I'm killing!!!!");
        }
    }
}
