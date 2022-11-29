using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GremlinBehavior : MonoBehaviour
{
    public GameObject player;
    Animator animator;
    SpriteRenderer spriteRenderer;
    string currentState;
    Color defaultColor;
    Rigidbody2D rigidBody;
    float knockbackSpeed = 6f;
    string currentAction;
    Vector3 lastPos;
    public float speed;
    public int timer;
    public int timersetter;
    public float xDif;
    public float yDif;
    public int timer2;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultColor = spriteRenderer.material.color;
        currentState = "GremlinEmergingAnimation";
        currentAction = "emerging";
        lastPos = transform.position;
        speed = .02f;
        timer = 0;
        timer2=0;
        timersetter = Random.Range(120,480);
    }

    // Update is called once per frame
    void Update()
    {   
        if((currentAction == "chasing") && timer >= 1){
            timer-=1;
            Move();
            InRange();
            
        } else if(currentAction == "chasing"){
            timer-=1;
            if(timer <= -timersetter){timer=timersetter;}
        }
        if(currentAction == "exploding"){
            transform.localScale += new Vector3(0.001f,.001f,0);
        }
        if(currentAction == "flying"){
            transform.eulerAngles += new Vector3 (0, 0, 2.0f);
            timer-=1;
            if(timer<=0){
                ChangeAnimationState("GremlinExplosionAnimation");
                currentAction="exploding";
            }
        }
    } 

    void OnTriggerEnter2D(Collider2D collision){
        if (collision.gameObject.tag == "Player")
        {
            xDif = player.transform.position.x - transform.position.x;
            yDif = player.transform.position.y - transform.position.y;
            if(Mathf.Abs(xDif)>=Mathf.Abs(yDif)){
                if(xDif>=0){
                    player.SendMessage("TakeDamage", "Right");
                } else {
                    player.SendMessage("TakeDamage", "Left");
                } 
            } else if(Mathf.Abs(yDif)>=Mathf.Abs(xDif)){
                if(yDif>=0){
                    player.SendMessage("TakeDamage", "Up");
                } else {
                    player.SendMessage("TakeDamage", "Down");
                }
            } else {
                player.SendMessage("TakeDamage", "Up");
            }
            
        }

    }

    void LoseHealth(string[] lhParams) {
        if(currentAction == "chasing" || currentAction=="detonating"){
        spriteRenderer.material.SetColor("_Color", Color.red);
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
            rigidBody.velocity = knockbackVelocity;
        currentAction="flying";
        timer2 = 150;
        }

    }
           

    void IsChasing(){
        currentAction= "chasing";
        timer = timersetter;
    }
    void IsExploding(){
        currentAction = "exploding";
    }
    void IsDetonating(){
        currentAction = "detonating";
        ChangeAnimationState("GremlinDetonationAnimation");
    }

    void Move(){
        lastPos=transform.position;
        transform.position = Vector3.MoveTowards(transform.position,player.transform.position,speed);
        float xChange = transform.position.x - lastPos.x;
        float yChange = transform.position.y - lastPos.y;
        if (xChange == 0 && yChange == 0) {
            ChangeAnimationState("GremlinRunningDownAnimation");
        }
        else if (Mathf.Abs(xChange) >= Mathf.Abs(yChange)){
            ChangeAnimationState("GremlinRunningLeftAnimation");
            if(xChange >= 0){
                spriteRenderer.flipX = true;
            } else {
                spriteRenderer.flipX = false;
            }
        }
        else {
            if(yChange >= 0){
                ChangeAnimationState("GremlinRunningUpAnimation");
            } else {
                ChangeAnimationState("GremlinRunningDownAnimation");
            }
        }
    }

    void InRange(){
        if(Vector3.Distance(transform.position, player.transform.position) <= 1){
            IsDetonating();
        }
    }

    void DestroyEnemy(){
        Destroy(gameObject);
    }


    void ChangeAnimationState(string state) {
        if (currentState != state) {
            animator.Play(state);
            currentState = state;
        }
    }
}
