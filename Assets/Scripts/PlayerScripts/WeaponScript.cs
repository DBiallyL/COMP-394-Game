using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    BoxCollider2D weaponCollider;
    string isRunning = "false";
    string lastDirec;

    // Start is called before the first frame update
    void Start()
    {
        weaponCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /**
    * Sets the appropriate position/scale for the weapon collider if the attack is a running attack
    *
    * Values are hard-coded
    */
    void RunningAttack(string lastDirection) {
        lastDirec = lastDirection;
        weaponCollider.enabled = true;
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
        isRunning = "true";
    }

    /**
    * Sets the appropriate position/scale for the weapon collider if the attack is a stationary attack
    *
    * Values are hard-coded
    */
    void StationaryAttack(string lastDirection) {
        lastDirec = lastDirection;
        weaponCollider.enabled = true;
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
        isRunning = "false";
    }

    /**
    * Resets the weapon to its default parameters
    */
    void ResetWeapon() {
        weaponCollider.enabled = false;
        weaponCollider.offset = Vector2.zero;
        weaponCollider.size = new Vector2(0.5f, 0.4f);
    }
    
    /**
    * If collided with the enemy, tell the enemy to lose some health
    */
    void OnTriggerEnter2D(Collider2D coll) {
        if (coll.CompareTag("Enemy") || coll.CompareTag("Enemy2")) {
            string[] lhParams = {isRunning, lastDirec};
            coll.SendMessage("LoseHealth", lhParams);
        }
    }
}
