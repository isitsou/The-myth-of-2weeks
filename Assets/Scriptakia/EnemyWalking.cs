using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalking : MonoBehaviour
{
    //Cached component refs
    private Rigidbody2D RB;
    private Animator ANIM;
    private AudioSource MY_AUDIOSOURCE;

    //Public vars
    [SerializeField] float horizontalVelocity = 1f;
    [SerializeField] float waitingTime = 1f;
    [SerializeField] AudioClip runningSound = null;

    //Vars
    public bool runningOutOfPlatform = false;
    private float timer;
    private RaycastHit2D hit;




    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        ANIM = GetComponent<Animator>();
        MY_AUDIOSOURCE = GetComponent<AudioSource>();
        //play running sound
        MY_AUDIOSOURCE.clip = runningSound;
        MY_AUDIOSOURCE.Play();
        timer = waitingTime;
    }

    void Update()
    {

        if (!runningOutOfPlatform)
        {
            Run();
        }
        else
        {
            Stay();
        }
    }

    public void Flip()
    {
        //pause running sound
        MY_AUDIOSOURCE.Stop();

        //flip
        transform.localScale = new Vector3(-Mathf.Sign(RB.velocity.x), 1, 1);
    }
    private void Run()
    {
        //Play walking anim
        ANIM.SetBool("IsRunning", true);

        //run accordingly to flip()
        if (transform.localScale.x > 0)
        {
            RB.velocity = new Vector2(horizontalVelocity, 0);
        }
        else
        {
            RB.velocity = new Vector2(-horizontalVelocity, 0);
        }
    }
    private void Stay()
    {

        //Play idle animation
        ANIM.SetBool("IsRunning", false);
        RB.velocity = Vector2.zero;


        //wait for 5 seconds
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            return;

        }
        runningOutOfPlatform = false;
        MY_AUDIOSOURCE.Play();
        timer = waitingTime;

    }
    private void CompleteDeath()
    {
        Destroy(gameObject);
    }
}
