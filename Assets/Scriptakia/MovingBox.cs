using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBox : MonoBehaviour
{
    //cached components refs
    Animator ANIM;
    Rigidbody2D RB;
    CircleCollider2D CIRCLE;
    AudioSource MY_AUDIOSOURCE;

    //public vars
    public Vector2 direction;
    [SerializeField] private float bounceLimiter = 5f;
    [SerializeField] private AudioClip fallingBox=null;

    //vars
    private float bounceCounter = 0f;


    // Start is called before the first frame update
    private void Start()
    {
        ANIM = GetComponent<Animator>();
        RB = GetComponent<Rigidbody2D>();
        CIRCLE = GetComponent<CircleCollider2D>();
        MY_AUDIOSOURCE = GetComponent<AudioSource>();
        //flip according to the current direction of the enemyBoxThrower
        if (Mathf.Sign(direction.x) > 0)
        {
            transform.Rotate(0, 180, 0);
        }
        
        //Add throwing force
        RB.AddForce(direction, ForceMode2D.Impulse);
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if you touch the ground(layer 8) play bounce animation        
        if ((collision.gameObject.layer == LayerMask.NameToLayer("Foreground")) && (bounceCounter <= bounceLimiter))
        {
            MY_AUDIOSOURCE.PlayOneShot(fallingBox);
            ANIM.SetTrigger("Bounce");
            bounceCounter += 1;
        }
        if (bounceCounter == 1)
        {
            CIRCLE.enabled = false;
        }

        //if you touch player destroy yourself
        if (collision.gameObject.name == "Player")
        {
            ANIM.SetTrigger("TouchedPlayer");
        }
    }
    private void CompleteDeath()
    {
        Destroy(gameObject);
    }
}
