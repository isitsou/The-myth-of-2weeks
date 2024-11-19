using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Cached component refs
    private Rigidbody2D RB;
    private Animator ANIM;
    private CapsuleCollider2D CAPSULE_COLLI;
    private PolygonCollider2D POLYG_COLLI;
    private BoxCollider2D BOX_CLIMB_COLLI;
    private AudioSource MY_AUDIOSOURCE;


    //Vars
    private float velX, velY;
    private bool isAlive = true, soundFlag = true;
    LayerMask deathStuff;

    //Public Vars
    [SerializeField] float x_velocityModifier = 5f;
    [SerializeField] float jumpModifier = 13.2f;
    [SerializeField] float verticalClimbVelocityModifier = 3.5f;
    [SerializeField] float horizontalClimbVelocityModifier = 0.5f;
    [SerializeField] GameObject aura;
    //[SerializeField] float deathPushingPower = 3f;
    [SerializeField] AudioClip runSound = null;
    [SerializeField] AudioClip jumpSound = null;
    private bool canShoot = false;
    private bool canTakeDamage_DEBUG = true;

    private bool CanShoot
    {
        get { return canShoot; }
        set
        {
            if (value == true && canShoot == false)
            {
                if (ANIM != null) ANIM.SetTrigger("HammerTime");
            }
            canShoot = value;
        }
    }




    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        ANIM = GetComponent<Animator>();
        CAPSULE_COLLI = GetComponent<CapsuleCollider2D>();
        POLYG_COLLI = GetComponent<PolygonCollider2D>();
        BOX_CLIMB_COLLI = GetComponent<BoxCollider2D>();
        MY_AUDIOSOURCE = GetComponent<AudioSource>();       
        deathStuff = LayerMask.GetMask("Enemies", "Hazards");
    }


    void Update()
    {
        //Debug
        if (Input.GetKeyDown(KeyCode.L)) canTakeDamage_DEBUG = !canTakeDamage_DEBUG;

        //check if player is alive
        if (!isAlive) return;
        if(canTakeDamage_DEBUG) Death();
        if (!isAlive) return;

        Run();
        Jump();
        ClimbLadder();
        Attack();
    }

    private void Attack()
    {
        bool playerIsTouchingLadder = POLYG_COLLI.IsTouchingLayers(LayerMask.GetMask("Ladders"));

        if (Input.GetMouseButtonDown(1) && canShoot && !playerIsTouchingLadder)
        {
            ANIM.SetTrigger("Attack");
            GameSessionController.instance.SubDiamonds(3);

            Transform auraTemp = Instantiate(aura, transform.GetChild(0).position, Quaternion.identity).transform;
            auraTemp.localScale = new Vector3(transform.localScale.x * auraTemp.localScale.x, auraTemp.localScale.y, 1);
            auraTemp.GetComponent<Aura>().direction = transform.localScale.x;
        }
    }

    private void Run()
    {
        //INPUT
        //velX = Input.GetAxis("Horizontal"); //smooth movement
        velX = Input.GetAxisRaw("Horizontal"); //snappy movement


        //Horizontal Movement
        RB.linearVelocity = new Vector2(velX * x_velocityModifier, RB.linearVelocity.y);

        //Flip accordingly && running anims
        if (Mathf.Abs(RB.linearVelocity.x) > Mathf.Epsilon && POLYG_COLLI.IsTouchingLayers(LayerMask.GetMask("Foreground")))
        {
            //play running anims because we have velocityX greater than something small and touching platform
            ANIM.SetBool("Running", true);
            ANIM.SetBool("Climbing", false);
            if (soundFlag)
            {
                MY_AUDIOSOURCE.clip = runSound;
                MY_AUDIOSOURCE.loop = true;
                MY_AUDIOSOURCE.Play();
                soundFlag = false;
            }

            //Flip
            transform.localScale = new Vector3(Mathf.Sign(velX), 1, 1);
        }
        else
        {
            if (!soundFlag)
            {
                //print("audio stop");
                MY_AUDIOSOURCE.Stop();
            }
            soundFlag = true;
            MY_AUDIOSOURCE.loop = false;
            ANIM.SetBool("Running", false);
        }


    }

    private void Jump()
    {
        bool playerIsTouchingWalls = POLYG_COLLI.IsTouchingLayers(LayerMask.GetMask("Foreground"));

        //INPUT
        bool jumpButtonPressed = Input.GetButtonDown("Jump");

        //Normal jump
        if (jumpButtonPressed && playerIsTouchingWalls)
        {
            //Add jump velocity
            Vector2 jumpspeed = new Vector2(0, jumpModifier);
            RB.linearVelocity = RB.linearVelocity + jumpspeed;
            ANIM.SetBool("IsJumping", true);
            AudioSource.PlayClipAtPoint(jumpSound, transform.position, 0.02f);

        }

        //Set anims for falling, not falling and not jumping
        if (RB.linearVelocity.y < -0.01f)
        {
            ANIM.SetBool("IsJumping", false);
            ANIM.SetBool("IsFalling", true);
        }
        else
        {
            ANIM.SetBool("IsFalling", false);
        }

    }

    private void ClimbLadder()
    {
        bool playerIsTouchingLadder = POLYG_COLLI.IsTouchingLayers(LayerMask.GetMask("Ladders"));

        if (playerIsTouchingLadder)
        {
            //Play climbStart Anim
            ANIM.SetBool("GrabLadder", true);
            ANIM.SetBool("IsFalling", false);
            RB.gravityScale = 0f;

            //INPUT
            velY = Input.GetAxis("Vertical");

            //Add vertical velocity
            RB.linearVelocity = new Vector2(RB.linearVelocity.x * horizontalClimbVelocityModifier, velY * verticalClimbVelocityModifier);

            //Play Climbing OR Idle Anim according to velocity
            if (Mathf.Abs(RB.linearVelocity.y) > Mathf.Epsilon)
            {
                ANIM.SetBool("Climbing", true);
            }
            else
            {
                ANIM.SetBool("Climbing", false);
            }

        }
        else
        {
            ANIM.SetBool("GrabLadder", false);
            RB.gravityScale = 1f;
        }

    }

    //Death related stuff
    private void Death()
    {
        
        //if one of the colliders on the player touches another collider on layer Enemies or Hazards then Die
        if (CAPSULE_COLLI.IsTouchingLayers(deathStuff) || POLYG_COLLI.IsTouchingLayers(deathStuff) || BOX_CLIMB_COLLI.IsTouchingLayers(deathStuff))
        {
            //disable colliders
            POLYG_COLLI.enabled = false;

            /*push player 
            Vector2 deathPush = new Vector2(0f, deathPushingPower);
            RB.velocity += deathPush;*/

            //stand still
            RB.bodyType = RigidbodyType2D.Static;

            //Play death animation
            ANIM.SetTrigger("Death");

            //dont let the player move
            isAlive = false;

            //Reduce the number of lives that the player has in this session and reload the scene
            FindObjectOfType<GameSessionController>().PlayersDeathController();
        }
    }
    private void CompleteDeath()
    {
        Destroy(gameObject);
    }

    public void CheckDiamonds(bool diamondsAreEnough)
    {
        CanShoot = diamondsAreEnough;
    }
}
