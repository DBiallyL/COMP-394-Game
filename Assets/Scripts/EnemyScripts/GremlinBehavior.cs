using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GremlinBehavior : EnemyInterface
{
    public GameObject player;
    Color defaultColor;
    string currentAction;
    Vector3 lastPos;
    public float speed;
    public int timer;
    public int timersetter;
    public float xDif;
    public float yDif;
    public int timer2;
    AudioSource audioSource;
    public AudioClip splat;
    public AudioClip explode;
    public AudioClip scuttle;

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
        audioSource = GetComponent<AudioSource>();

        knockbackSpeed = 6f;
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
        if (collision.gameObject.tag == "Player" && currentAction != "emerging")
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
            EnemyKnockback(lhParams[1]);
            currentAction="flying";
            timer2 = 150;
        }
    }
           

    void IsChasing(){
        currentAction= "chasing";
        timer = timersetter;
        audioSource.PlayOneShot(scuttle, 0.7f);
        audioSource.loop = true;
    }
    void IsExploding(){
        currentAction = "exploding";
        audioSource.PlayOneShot(explode, 0.7f);
        audioSource.loop = false;
    }
    void IsDetonating(){
        currentAction = "detonating";
        ChangeAnimationState("GremlinDetonationAnimation");
        audioSource.PlayOneShot(splat, 0.7f);
        audioSource.loop = false;
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
}
