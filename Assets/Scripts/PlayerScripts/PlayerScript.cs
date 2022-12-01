using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
// using System;
using UnityEngine;

// Some animation help from: https://www.youtube.com/watch?v=hkaysu1Z-N8 

public class PlayerScript : MonoBehaviour
{
    // Global variables to deal with being attacked/health and dying
    public float health = 5f;
    public GameObject HealthBar;
    bool dead = false;
    float immuneLength = 0.4f;
    float immuneTime = -1f;
    Color defaultColor;
    float knockbackSpeed = 6f;
    public GameObject RedCounter;
    public GameObject BlueCounter;

    // Global variables used to handle movement
    float speed = 3f;
    float regSpeed;
    float diagSpeed;
    Rigidbody2D rigidBody;
    GameObject[] waterColliders;

    // Global variables used to handle dashes
    int dashes;
    int maxDashes;
    public GameObject[] dashUIArray;
    // Will be greater than -1f if the player is dashing
    float dashTimer = -1f;
    int dashMultiplier = 3;
    float dashLength = 0.5f;

    // Global variables used to handle animation
    Animator animator;
    SpriteRenderer spriteRenderer;
    string currentState;
    string lastDirection = "Right";
    // Michael Jackson Mode is an easter egg used to handle whether animations are flipping left/right correctly
    bool michaelJacksonMode = false;

    // Global variables used to handle attacking 
    public GameObject weapon;
    public GameObject mainCamera;
    public GameObject DamageOverlay;
    bool canMove = true;
    string attackString = "Attack";
    int nextStationaryAttack = 0;

    // Global variables used to handle rituals
    bool pressedR = false;
    public GameObject ritualCircle;
    public GameObject ritualBar;

    // Start is called before the first frame update
    void Start()
    {        
        rigidBody = GetComponent<Rigidbody2D>();  
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultColor = spriteRenderer.material.color;

        diagSpeed = (float) Mathf.Sqrt((speed * speed) / 2); 
        regSpeed = speed;

        dashes = dashUIArray.Length;
        maxDashes = dashes;
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead) {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if(Time.timeScale >= 1){
                    PauseGame();
                } else {
                    ResumeGame();
                }
            }
            if(Time.timeScale >= 1){
            if (canMove) CheckMvmt();
            CheckAction();
            Timers();
            CheckDead();
            }
        }
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    //                                                Code for Movement/Idle Animation
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    /**
    * Checks if the player is moving based on keys pressed, and changes the animation as appropriate
    */
    void CheckMvmt() {
        Vector2 movement = Vector2.zero;
        string spritePath = "Player";

        // Not moving
        if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.A) &&
            !Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.D) && 
            !Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.W) &&
            !Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.S)) {
            movement = Vector2.zero;
            spritePath += "Idle";
        }
        // Moving
        else {
            movement = AnimateRunning();
            if (dashTimer == -1f) {
                spritePath += "Running";
            }
            else {
                spritePath += "Dashing";
            }
        }

        // Set Animation State
        ChangeToDirectionalAnimation(spritePath);

        rigidBody.velocity = movement;
    }

    /**
    * Helper function for CheckMvmt()
    *
    * Moves the player according to keys pressed, and changes the animation as appropriate
    */
    Vector2 AnimateRunning() {
        Vector2 movement = Vector2.zero;

        // Moving left
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))  {
            lastDirection = "Left";
            // Up and left
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {
                movement.x = -diagSpeed;
                movement.y = diagSpeed;
            }
            // Down and left
            else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) {
                movement.x = -diagSpeed;
                movement.y = -diagSpeed;
            }
            // Just left
            else {
                movement.x = -speed;
            }
        }
        // Moving right
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
            lastDirection = "Right";
            // Up and right
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {
                movement.x = diagSpeed;
                movement.y = diagSpeed;
            }
            // Down and right
            else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) {
                movement.x = diagSpeed;
                movement.y = -diagSpeed;
            }
            // Just right
            else {
                movement.x = speed;
            }
        }
        // Only moving vertically
        else {
            // Up
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                movement.y = speed;
                lastDirection = "Up";
            }
            // Down
            else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                movement.y = -speed;
                lastDirection = "Down";
            }
        }

        return movement;
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    //                                                Code for Detecting Water
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    void disableWaterColliders() {
        waterColliders = GameObject.FindGameObjectsWithTag("Water");
        foreach (GameObject water in waterColliders) {
            water.SetActive(false);
        }
    }

    void enableWaterColliders() {
        foreach (GameObject water in waterColliders) {
            water.SetActive(true);
        }
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    //                                                Code for Key Actions
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -


    /**
    * Checks if the player hit a key for some non-movement action
    * Handles: Attacks, Rituals, Dashes, Micheal Jackson Mode
    */
    void CheckAction() {
        // Attack
        if (Input.GetKey(KeyCode.Q) && canMove)
        {
            string motion = "";
            // Running attack
            if (dashTimer != -1f) {
                motion = "Running";
                attackString = "Attack";
            }
            // Stationary attack
            else {
                motion = "Stationary";
                rigidBody.velocity = Vector2.zero;
                attackString = "Attack" + (nextStationaryAttack % 3);
                nextStationaryAttack++;
                if (rigidBody.velocity.x == knockbackSpeed || rigidBody.velocity.x == -knockbackSpeed 
                    || rigidBody.velocity.y == knockbackSpeed || rigidBody.velocity.y == -knockbackSpeed) {
                    if(lastDirection == "Down"){
                        transform.position+= new Vector3(0f, -.3f, 0f);
                    }
                    if(lastDirection == "Up"){
                        transform.position+= new Vector3(0f, .3f, 0f);
                    }
                    if(lastDirection == "Left"){
                        transform.position+= new Vector3(-.3f, 0f, 0f);
                    }
                    if(lastDirection == "Right"){
                        transform.position+= new Vector3(.3f, 0f, 0f);
                    }
                }
            }
            ChangeToDirectionalAnimation("Player" + motion + attackString);
            weapon.SendMessage(motion + "Attack", lastDirection);
            canMove = false;
        }
        // Ritual
        else if (Input.GetKey(KeyCode.R))
        {
            if (!pressedR) {
                canMove = false;
                ChangeAnimationState("PlayerRitualStartUp");
                pressedR = true;
                rigidBody.velocity = Vector2.zero;
                ritualCircle.SendMessage("StartRitual");
                ritualBar.SendMessage("StartRitual");
            }
        }

        // Stop Ritual
        if (Input.GetKeyUp(KeyCode.R)) {
            pressedR = false;
            if (!canMove) {
                canMove = true;
                ritualCircle.SendMessage("ResetRitualObject", "StopRitualPremature");
            }
        }

        // Dash
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            if (dashes > 0 && (rigidBody.velocity.x == 0 || rigidBody.velocity.y == 0) && dashTimer == -1f) {
                disableWaterColliders();
                speed *= dashMultiplier;
                dashTimer = Time.time;
                dashes--;
                DecrementDashUI();
            }
        }

        // Michael Jackson Mode
        if (Input.GetKeyDown(KeyCode.M)) {
            michaelJacksonMode = true;
        }
    }

    /**
    * Stops the player's ritual animation once the ritual has naturally ended (timer completed, enemies exorcised)
    */
    void StopRitual() {
        canMove = true;
    }

    /**
    * Animates the Dash UI according to the amount of dashes remaining
    */
    void DecrementDashUI() {
        if (dashes < maxDashes) {
            dashUIArray[dashes].GetComponent<Animator>().Play("DashReload");
            for (int i = dashes + 1; i < maxDashes; i++) {
                dashUIArray[i].GetComponent<Animator>().Play("DashEmpty");
            }
        }
    }

    /**
    * Increments the number of dashes (if player doesn't have the maximum) and animates the UI appropriately
    *
    * Called upon the start of the loaded dash animation
    */
    void IncrementDashes() {
        if (dashes < maxDashes) dashes++;
        if (dashes < maxDashes) dashUIArray[dashes].GetComponent<Animator>().Play("DashReload");
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    //                                                Code for Timers
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -


    /**
    * Keeps track of all timers 
    * 
    * Timers: 
    *  Dash duration
    *  Immunity (after getting attacked)
    *  Knockback
    */
    void Timers() {
        float time = Time.time;

        // Dash duration
        if (dashTimer != -1f) {
            float elapsedTime = time - dashTimer;
            if (elapsedTime >= dashLength) {
                speed = regSpeed;
                dashTimer = -1f;
                enableWaterColliders();
            }
        }

        // Immunity
        if (immuneTime != -1f) {
            float elapsedTime = time - immuneTime;
            if (elapsedTime >= immuneLength) {
                immuneTime = -1f;
                spriteRenderer.material.SetColor("_Color", defaultColor);
            }
            // Knockback
            else if (elapsedTime >= (immuneLength / 2)) {
                canMove = true;
                if (rigidBody.velocity.x == knockbackSpeed || rigidBody.velocity.x == -knockbackSpeed 
                    || rigidBody.velocity.y == knockbackSpeed || rigidBody.velocity.y == -knockbackSpeed) {
                        rigidBody.velocity = Vector2.zero;
                    }
            }
        }
    }


    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    //                                                Code for Attacking
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    /**
    * Hard-coded translations so that attack animations line up with movement animations
    *
    * Numbers should align with those in the below method (ResetAttackPlacement)
    */
    void FixAttackPlacement() {
        if (lastDirection == "Up") {
            transform.Translate(new Vector3(.25f, .4f, 0f));
        }
        else if (lastDirection == "Right") {
            transform.Translate(new Vector3(.4f, .2f, 0f));
        }
        else if (lastDirection == "Left") {
            transform.Translate(new Vector3(-.4f, .2f, 0f));
        }
        // Prevents camera weirdly jerking around during attack
        mainCamera.SendMessage("DontFollow");
    }

    /**
    * Resets any translations done during attack animations
    *
    * Numbers should align with those in the above method (FixAttackPlacement)
    */
    void ResetAttackPlacement() {
        if (lastDirection == "Up") {
            transform.Translate(new Vector3(-.25f, -.4f, 0f));
        }
        else if (lastDirection == "Right") {
            transform.Translate(new Vector3(-.4f, -.2f, 0f));
        }
        else if (lastDirection == "Left") {
            transform.Translate(new Vector3(.4f, -.2f, 0f));
        }
        mainCamera.SendMessage("Follow");
    }

    /**
    * Used at the end of the horizontal running attack animation, allows the player to move and moves the player back a
    * little so the animation looks smoother
    *
    * TODO: Remove and replace all individual right run stuff if never add back anything
    */
    void EndRightRunAttack() {
        // TODO: Currently commented out cause it will shake the entire camera at the cost of animation lookin a bit funky
        //       is there a better way to fix it?
        // transform.position = new Vector2(transform.position.x - rigidBody.velocity.x, transform.position.y);
        EndAttack();
    }

    
    /**
    * Used as moving attack animations play
    *
    * Divides the velocity by 1.5
    */
    void SlowToStop() {
        float newXVel = rigidBody.velocity.x / 1.5f;
        float newYVel = rigidBody.velocity.y / 1.5f;
        rigidBody.velocity = new Vector2(newXVel,newYVel);
    }

    /**
    * Used at the end of any attack animation to re-allow key movement and reset the weapon
    */
    void EndAttack() {
        canMove = true;
        weapon.SendMessage("ResetWeapon");
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    //                                                Code for Health/Damage
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    /**
    * Handles the player taking damage 
    */
    void GainHealth(){
        HealthBar.SendMessage("GainHealth");
    }

    void Heal(float percent){
        health+= percent * 5f;
    }

    void TakeDamage(string knockback) {
        if (immuneTime == -1f) {
            HealthBar.SendMessage("LoseHealth", 0.2f);
            health--;

            DamageOverlay.SendMessage("MakeVisible");
            spriteRenderer.material.SetColor("_Color", Color.red);

            immuneTime = Time.time;
            canMove = false;

            // Knockback
            Vector2 knockbackVelocity = Vector2.zero;
            if (knockback == "Down") {
                knockbackVelocity.y = -knockbackSpeed;
            }
            else if (knockback == "Up") {
                knockbackVelocity.y = knockbackSpeed;
            }
            else if (knockback == "Left") {
                knockbackVelocity.x = -knockbackSpeed;
            }
            else {
                knockbackVelocity.x = knockbackSpeed;
            }
            rigidBody.velocity = knockbackVelocity;
        }
    }

    /**
    * Checks if the player's health is at or below 0, and kills the player if so 
    */
    void CheckDead() {
        if (health <= 0f) {
            ChangeAnimationState("PlayerDying");
            dead = true;
            rigidBody.velocity = Vector2.zero;
        }
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    //                                                Widely Used Helper Methods
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -


    /**
    * Changes the player's animation based on some direction it's facing
    * spritePath is the file path of some player sprite which has Up/Down/Right options, without Up/Down/Right on the end
    */
    void ChangeToDirectionalAnimation(string spritePath) {
        spriteRenderer.flipX = false;
        // Left
        if (lastDirection == "Left") { 
            ChangeAnimationState(spritePath + "Right");
            spriteRenderer.flipX = !michaelJacksonMode; 
        }
        // Right
        else if (lastDirection == "Right") {
            ChangeAnimationState(spritePath + "Right");
            spriteRenderer.flipX = michaelJacksonMode;
        }
        // Up and Down
        else {
            ChangeAnimationState(spritePath + lastDirection);
        }
    }

    /**
    * Changes which animation the object is using
    */
    void ChangeAnimationState(string state) {
        if (currentState != state) {
            animator.Play(state);
            currentState = state;
        }
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    //                                                Code for Changing Levels
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    /**
    * Changes the scene when colliding with portal with "Finish" tag.
    */
    void OnCollisionEnter2D(Collision2D coll) {
        if(coll.collider.CompareTag("Finish")) {
            SceneManager.LoadScene("GameOver");
            // TODO: Put in right level
        }
    }

    void PauseGame ()
    {
        Time.timeScale = 0;
    }
    void ResumeGame ()
    {
        Time.timeScale = 1;
    }
    void AwardRedSoul(){
        RedCounter.SendMessage("AddSoulAmount",1);
    }
    void AwardBlueSoul(){
        BlueCounter.SendMessage("AddSoulAmount",1);
    }
}

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    //                                                Michael Jackson <3
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                                                                                
    //                                 ,*#&&&&&&&#%%((*(. , .                      
    //                              ./*&@@@@@&@@@@@@@@@@@&&%%(*./*,                
    //                          *(#&@@@@@@@@@@@@@@@@@@@@@@@@@@@@@&%/* .            
    //                        (@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@&@@@@@&& .            
    //                      .#%@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@&@@@@@%*           
    //                     *%@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@&@&@@@&@@@@&#          
    //                   ,/&@@@@@@@@@@@@@@@@@@&@@@@@@@@@@@@@@@@@@@@@@@@@@%*.       
    //                  ..@@@@@@@@@@@@@@@@&@&&&@@%%&@&@@@@@@@&@@@@@@@@@@@&         
    //                 /#@@@@@@@@@@@@@@@@@@@@@@@@&&#((#&@@@@@@@@@@@@@@@@&(.        
    //               .#@@@@@@@@@@@@@@@@&&#(((##(/**,,..*&@&&/#%&@@%@&@@@@&         
    //               /&@@@@@@@@@@@@@@@@@&((////****,,,.*&&*,,**/(&&@@@@@@#,        
    //              ,@@@@@@@@@@@@@@@@@&(((////***,,,,,,,*#@,,,**/((@&@&@@@*        
    //             ,%@@@@@@@@@@@@@@@&((((((###(/**,,,,,*(%/,,,,**#@&@@@@%(         
    //             ,@@@@@@@@@@@@@@@@#((##((////#&%&##&&*,,,,,**//%@@@@&&&.         
    //             ,%@@@@@@@@&@&&@@%((///%@#&@%/(##%%/*,,/(%&%%#/(&@&@&&%          
    //            .@&&@@@@@@@@@@@@@%((/***/(###//*/(((*,**/,&((,%/@@&.             
    //           ,.%&@@@@@@@@@&&@@@%#(//**,,,....**//(/.*,.,****,*.                
    //           .&@@@@@@@@@@@@@@&%%%#((//*,,,,,,//(((/,*,,...,,,                  
    //          ,#%@@@@@@@@@@@@@@&%####((//**,,,*#(((((*,*,,,,,*,                  
    //         /.&@&@@@@@@@@@@@@@&&%##(##((/*********/(**,,,,,*                    
    //         (@@@@@@@@@@@@@@@@@@&&%####((//////********,,*,*                     
    //         /@@@@@@@@@@@@@@@@@@&&&%####((///((%%%#(((##/**                      
    //          .#&@@@@@@&@@&@@@@&&&&%%%%%#(/////(((((///**#&*                     
    //       &@&%@&@@@@@@@@/@@@&%%%%%%%%%%%#(//***********@@@@@*                   
    // ,(@@@&@@@@@@@@@@@@@@(#%######%%%%%%%%%##(/////**(%@@@@@@@@#//*              
    // /@@@@&&@@@@@@@@@@@@@@,(((((((####%%%%%%%####(((%@@@@@@@@@@@@@@@@@@@.        
    // /@&&&&%&@@&@@@&@@@@@@& .///(((((((#######(((((%@@@@@@@@@@@&@@@&&&&@@&#      
    // /@@@@@&%@@@@@@@@@@@@@@*   *//(((((((((((((((((@@@@@@@@@@@@@@@@@@@@@@@@@@    
    // /@@@@@@&%@@@@@@@@@@@@@@.    ,///////((((////*.@@@@@@@@@@@@@@@@@@@@@@@@@@    
    // /@@@@@@@%&@@@@@@@@@@@@@&       *(///////*//  *@@@@@@@@@@@@@@@@@@@@@@@@@@    
    // /@@@@@@@&%@@@@@@@@@@@@@@*         .,,,,,,    /@@@@@@@@@@@@@@@@@@@@@@@@@@    