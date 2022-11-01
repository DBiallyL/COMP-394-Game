using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    // Global variables used to handle animation
    BoxCollider2D weaponCollider;
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        weaponCollider = GetComponent<BoxCollider2D>();
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
        // Down
        else {
            weaponCollider.offset = new Vector2(0f, -0.15f);
        }
    }

    void StationaryAttack(string lastDirection) {
        if (lastDirection == "Right") {
            weaponCollider.offset = new Vector2(0.15f, 0f);
            weaponCollider.size = new Vector2(0.3f, 0.6f);
        }
        else if (lastDirection == "Left") {
            weaponCollider.offset = new Vector2(-0.15f, 0f);
            weaponCollider.size = new Vector2(0.3f, 0.6f);
        }
        else if (lastDirection == "Up") {
            weaponCollider.offset = new Vector2(0f, 0.1f);
            weaponCollider.size = new Vector2(0.7f, 0.4f);
        }
        // Down
        else {
            weaponCollider.offset = new Vector2(0f, -0.15f);
            weaponCollider.size = new Vector2(0.8f, 0.4f);
        }
    }

    void ResetWeapon() {
        weaponCollider.offset = Vector2.zero;
        weaponCollider.size = new Vector2(0.5f, 0.4f);
    }
    
    void OnTriggerEnter2D(Collider2D coll) {
        if (coll.CompareTag("Enemy")) {
            print("Aaaaah I'm killing!!!!");
        }
    }
}
