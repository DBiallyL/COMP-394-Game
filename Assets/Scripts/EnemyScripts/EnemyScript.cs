using System.Collections;
using System.Collections.Generic;
// using System;
using UnityEngine;

// Paths coded with help from: https://www.youtube.com/watch?v=KoFDDp5W5p0 
// Animation coded with help from: https://www.youtube.com/watch?v=nBkiSJ5z-hE, https://answers.unity.com/questions/952558/how-to-flip-sprite-horizontally-in-unity-2d.html
// Enemy light cone uses asset HardLight2D: https://assetstore.unity.com/packages/tools/particles-effects/hard-light-2d-152208

public class EnemyScript : MonoBehaviour
{
    // Global variables used to handle movement
    public float speed = 0.01f;
    float attackSpeed;
    float regSpeed;
    bool pausing = false;
    float pauseTime = -1f;
    Vector3 GremlinPos;


    // Global variables used to handle path walking
    public Transform[] waypoints;
    public GameObject Gremlin;
    int waypointIndex = 1;
    float pathPauseLength = 3f;
    
    // Global variables used to handle light cone
    public Texture leftLight, upLight, rightLight, downLight;
    Material lightMaterial;
    Transform lightCollider;
    Transform lightChild;
    MeshRenderer lightChildMesh;

    // Global variables used to keep track of direction and animation states
    Animator animator;
    SpriteRenderer spriteRenderer;
    string currentState;
    Vector3 lastPos;
    string lastDirection = "Left";

    // Global variables to handle losing health/getting hit
    int health = 4;
    public int statAttackStrength = 1;
    public int runAttackStrength = 2;
    bool dead = false;
    float immuneLength = 0.8f;
    float immuneTime = -1f;
    Color defaultColor;
    Rigidbody2D rigidBody;
    float knockbackSpeed = 6f;

    // Global variables to handle attacking player/being enraged
    public GameObject player;
    bool animationPlaying = false;
    bool canMove = true;
    // followingPLayer also corresponds to being enraged
    bool followingPlayer = false;
    int nextAttack = 0; 
    bool attacking = false;
    float tiredPauseLength = 1;

    // Global variables to handle calming down
    bool checkingCalm = false;
    float calmingTime = -1f;
    float calmingLength = 2f;
    float calmCheckDistance = 6f;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultColor = spriteRenderer.material.color;

        lightChild = transform.Find("Light2D");
        lightCollider = transform.Find("EnemyLight_Collider");
        lightChildMesh = lightChild.gameObject.GetComponent<MeshRenderer>();
        lightMaterial = lightChild.GetComponent<Renderer>().material;

        transform.position = waypoints[0].position;
        lastPos = transform.position;

        attackSpeed = speed * 1.5f;
        regSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead) {
            if (!animationPlaying && canMove) {
                if (followingPlayer && !pausing) FollowPlayer();
                else WalkPath();
                ChangeWalkSprites();
            }
            // if (!animationPlaying) ChangeWalkSprites(followingPlayer);
            if (checkingCalm) CalmDown();
            if (attacking) CheckHitPlayer();
            Timers();
            ChangeLightRotation();
            CheckDead();
        }
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    //                                                Code for Timers
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    /**
    * Takes care of timers 
    *
    * Timers:
    *   Immunity (after taking damage)
    *   Knockback
    *   Calming down
    *   Pausing
    */
    void Timers(){
        float time = Time.time;

        // Immunity timer
        if (immuneTime != -1f) {
            float elapsedTime = time - immuneTime;
            if (elapsedTime >= immuneLength) {
                immuneTime = -1f;
                spriteRenderer.material.SetColor("_Color", defaultColor);
                canMove = true;
            }
            // Knockback timer
            else if (elapsedTime >= (immuneLength / 4)) {
                if (rigidBody.velocity.x == knockbackSpeed || rigidBody.velocity.x == -knockbackSpeed 
                || rigidBody.velocity.y == knockbackSpeed || rigidBody.velocity.y == -knockbackSpeed) {
                    rigidBody.velocity = Vector2.zero;
                    animationPlaying = false;
                }
            }
        }

        // Calm timer
        if (calmingTime != -1f) {
            float elapsedTime = time - calmingTime;
            if (elapsedTime >= calmingLength) {
                calmingTime = -1f;
                checkingCalm = false;
                followingPlayer = false;
            }
        }

        // Pause timer
        if (pausing) {
            float elapsedTime = time - pauseTime;
            if (elapsedTime > pathPauseLength) {
                // pathPauseLength should be greater than tiredPauseLength, so this should apply to both
                pausing = false;
                pauseTime = -1f;
            }
            else if (followingPlayer && elapsedTime > tiredPauseLength) {
                pausing = false;
                pauseTime = -1f;
            }
        }
    }
    

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    //                                                Code for Default Animations
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    /**
    * Animates the enemy appropriately
    */
    void ChangeWalkSprites() {
        float xChange = transform.position.x - lastPos.x;
        float yChange = transform.position.y - lastPos.y;
        string spritePath = "Enemy";

        string prevDirection = lastDirection;

        // Idle Sprites
        if (xChange == 0 && yChange == 0) {
            spritePath += "Idle";
        }
        // Walking Sprites
        else {
            if (Mathf.Abs(xChange) > Mathf.Abs(yChange)) {
                // Right
                if(xChange > 0) {
                    lastDirection = "Right";
                }
                // Left
                else {
                    lastDirection = "Left";
                }
            }
            else {
                // Up
                if (yChange > 0) {
                    lastDirection = "Up";
                }
                // Down
                else {
                    lastDirection = "Down";
                }
            }

            // Used to prevent attack animation bug upon enemy changing directions
            if (attacking && lastDirection != prevDirection) {
                StopAttack();
            }

            // Non-attacking animation
            if (!attacking) {
                spritePath += "Walk";
            }
            // Attack animation
            else {
                spritePath += ("Attack" + (nextAttack % 2));
            }
        }

        // Enraged non-attacking animation
        if (followingPlayer && !attacking) {
            spritePath += "Enraged";
        }

        ChangeToDirectionalAnimation(spritePath);

        lastPos = transform.position;
    }

    /**
    * Changes the rotation of the HardLight2D light and the light collider in update
    */
    void ChangeLightRotation() {
        // Up
        if (lastDirection == "Up") {
            lightMaterial.SetTexture("_MainTex", upLight);
            if (lightCollider.rotation != Quaternion.Euler(0f,0f,0f))
                lightCollider.rotation = Quaternion.Euler(0f,0f,0f);
        }
        // Down
        if (lastDirection == "Down") {
            lightMaterial.SetTexture("_MainTex", downLight);
            if (lightCollider.rotation != Quaternion.Euler(0f,0f,180f))
                lightCollider.rotation = Quaternion.Euler(0f,0f,180f);
        }
        // Right
        if (lastDirection == "Right") {
            lightMaterial.SetTexture("_MainTex", rightLight);
            if (lightCollider.rotation != Quaternion.Euler(0f,0f,-90f))
                lightCollider.rotation = Quaternion.Euler(0f,0f,-90f);
        }
        // Left
        if (lastDirection == "Left") {
            lightMaterial.SetTexture("_MainTex", leftLight);
            if (lightCollider.rotation != Quaternion.Euler(0f,0f,90f))
                lightCollider.rotation = Quaternion.Euler(0f,0f,90f);
        }
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    //                                                Code for Movement
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -


    /**
    * Moves the enemy along the waypoints in a pre-created path
    * 
    * Only occurs if the enemy is calm
    */
    void WalkPath() {
        // Reach first waypoint
        if (waypointIndex >= waypoints.Length) {
            waypointIndex = 0;
            pausing = true;
            pauseTime = Time.time;
        }
        // Walk to next waypoint
        else {
            Vector3 target = waypoints[waypointIndex].position;

            transform.position = Vector2.MoveTowards(transform.position, target, (speed * Time.deltaTime));
            if (transform.position.x == target.x && transform.position.y == target.y) {
                waypointIndex++;
            }
        }
    }

    /**
    * Moves the enemy towards the player, attacks the player if in range
    *
    * Only occurs if the enemy is enraged
    */
    void FollowPlayer() {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, (speed * Time.deltaTime));

        // Checks if the player is in range, and attacks if so
        float distFromPlayer = Vector2.Distance(player.transform.position, transform.position); 
        if (distFromPlayer > -3f && distFromPlayer < 3f && !attacking) {
            attacking = true;
            speed = attackSpeed;
        }         
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    //                                                Code for Enemy Enraged/Calm States
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    /**
    * Checks to see if the player is out of range for the enemy, and starts forgetting the playe (returning to a calm state) if so
    */
    void CalmDown() {
        checkingCalm = true;
        // Can't calm down if still becoming enraged
        if (!animationPlaying) {
            float distFromPlayer = Vector2.Distance(player.transform.position, transform.position); 
            // Player is out of range, start forgetting
            if (distFromPlayer > calmCheckDistance && calmingTime == -1f) {
                calmingTime = Time.time;
            }
            // Player is back in range
            else if (distFromPlayer < calmCheckDistance && calmingTime != -1f) {
                calmingTime = -1f;
            }
        }
    }

    /**
    * Handles how the enemy responds to the player entering their viewcone collider 
    * and transitions into attack mode if the player is not behind another collider
    *
    * TODO: Make raycast thing a different method so it can be used for calmdown too (so enemy loses player behind collider)
    */
    void GetAngry() {
        // Won't look for player if already angry
        if (!followingPlayer) {
            Vector2 direction = player.transform.position - transform.position;
            RaycastHit2D[] ray = Physics2D.RaycastAll(transform.position, direction);
            // TODO: Remove debug line when no longer needed
            Debug.DrawRay(transform.position, direction, Color.green, 10f);
            // In playtests the player would show up at index 1 or 2 of the ray array (enemy's own colliders would fill earlier spots)
            if ((ray.Length > 2 && ray[2].collider.CompareTag("Player")) || ray[1].collider.CompareTag("Player")) {
                ChangeAnimationState("EnemyBecomingEnraged");
                followingPlayer = true;
                animationPlaying = true;
                GremlinMaker(Random.Range(5,8));
                
            }
        }
        checkingCalm = false;
    }

    void GremlinMaker(int number){
        
        for(int i=0; i< number; i++){
            print("aha");
            GremlinPos = transform.position + new Vector3(Random.Range(-3f,3f),Random.Range(-3f,3f),0);
            Instantiate(Gremlin, GremlinPos, transform.rotation);
        }

    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    //                                                Code for Enemy Attacks
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    /**
    * Handles the variable changes upon the end of a single attack
    */
    void StopAttack() {
        // Checking if three attacks are over to pause
        // TODO: Make it so this is actually about consecutive attacks (may not be rn)
        if (nextAttack % 3 == 0) {
            pausing = true;
            pauseTime = Time.time;
        }
        attacking = false;
        nextAttack++;
        speed = regSpeed;
    }

    /**
    * Checks the distance from the enemy to the player to see if the enemy actually damaged the player
    */
    void CheckHitPlayer() {
        float distFromPlayer = Vector2.Distance(player.transform.position, transform.position); 
        if (distFromPlayer > -1f && distFromPlayer < 1f) {
            player.SendMessage("TakeDamage", lastDirection);
        }       
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    //                                                Code for Health/Damage
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    /**
    * Handles what happens to the enemy when attacked by the player
    *
    * lhParams is an array with two strings contained: 
    *   The first string is true or false, where true means the player did a running attack and false is a stationary attack
    *   The second string is the direction the player was moving
    */
    void LoseHealth(string[] lhParams) {
        // If not immune
        if (immuneTime == -1f) {
            // Enrage enemy
            if(!followingPlayer){
                GremlinMaker(Random.Range(5,8));
            }
            followingPlayer=true;
            player.SendMessage("GainHealth");

            // Turn red and gain temporary immunity
            canMove=false;
            spriteRenderer.material.SetColor("_Color", Color.red);
            immuneTime = Time.time;

            // Lose health
            if (lhParams[0] == "true") {
                health -= runAttackStrength;
            }
            else {
                health -= statAttackStrength;
            }

            // Knockback
            
            Vector2 knockbackVelocity = Vector2.zero;
            if (lhParams[1] == "Down") {
                knockbackVelocity.y = -knockbackSpeed;
            }
            else if (lhParams[1] == "Up") {
                knockbackVelocity.y = knockbackSpeed;
            }
            else if (lhParams[1] == "Left") {
                knockbackVelocity.x = -knockbackSpeed;
            }
            else {
                knockbackVelocity.x = knockbackSpeed;
            }
            animationPlaying = true;
            ChangeToDirectionalAnimation("EnemyKnockback");
            rigidBody.velocity = knockbackVelocity;
            
        }
    }

    /**
    * Checks if the enemy has no health and starts killing enemy if so
    */
    void CheckDead() {
        if (health <= 0) {
            ChangeAnimationState("EnemyDying");
            // Both need to be here or else the enemy can revive itself through the power of rage (aka collider finds the player again)
            Destroy(lightCollider.gameObject);
            Destroy(lightChild.gameObject);
            dead = true;
            rigidBody.velocity = Vector2.zero;
        }
    }

    /**
    * Called by the dead animation to destroy the enemy's game object
    */
    void DestroyEnemy() {
        Destroy(gameObject);
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    //                                                Code for Rituals
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    /**
    * Called by the ritual game object once the ritual is started to stop the enemy and start the ritual animation
    */
    void StartRitual() {
        ChangeAnimationState("EnemyTrappedInRitual");
        animationPlaying = true;
        lightChildMesh.enabled = false;
    }

    /**
    * Called by the player if the ritual is stopped prematurely to anger enemy and return enemy to normal
    */
    void StopRitualPremature() {
        animationPlaying = false;
        followingPlayer = true;
        lightChildMesh.enabled = true;
    }

    /**
    * Called by the ritual game object once the ritual is over to start the process of destroying the enemy
    */
    void RitualEnd() {
        // Both need to be here or else the enemy can revive itself through the power of rage (aka collider finds the player again)
        Destroy(lightCollider.gameObject);
        Destroy(lightChild.gameObject);
        ChangeAnimationState("EnemyBecomingExorcised");
        dead = true;
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    //                                                Widely Used Helper Methods
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    
    /**
    * Changes the enemy's animation based on some direction it's facing
    * spritePath is the file path of some enemy sprite which has Up/Down/Left options, without Up/Down/Left on the end
    */
    void ChangeToDirectionalAnimation(string spritePath) {
        spriteRenderer.flipX = false;
        // Left
        if (lastDirection == "Left") { 
            ChangeAnimationState(spritePath + "Left");
            spriteRenderer.flipX = false; 
        }
        // Right
        else if (lastDirection == "Right") {
            ChangeAnimationState(spritePath + "Left");
            spriteRenderer.flipX = true;
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

    /**
    * Allows the enemy to start moving again after an animation ends 
    */
    void Unpause() {
        animationPlaying = false;
    }
}
