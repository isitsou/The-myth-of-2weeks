using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBomb : MonoBehaviour
{
    //cached stuff
    Animator ANIM;
    Rigidbody2D RB;
    AudioSource MY_AUDIOSOURCE;

    //public vars
    public Vector2 direction;//= new Vector2(-3.5f,5f);
    [SerializeField] AudioClip[] bombSounds=null;


    // Start is called before the first frame update
    private void Start()
    {
        //start playinf sound of fuse
        MY_AUDIOSOURCE = GetComponent<AudioSource>();
        MY_AUDIOSOURCE.clip = bombSounds[0];
        MY_AUDIOSOURCE.playOnAwake = true;
        MY_AUDIOSOURCE.Play();
        ANIM = GetComponent<Animator>();
        RB = GetComponent<Rigidbody2D>();
        //Add throwing force
        RB.AddForce(direction, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.name == "Player")
        {
            ANIM.SetTrigger("TouchedPlayer");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Aura"))
        {
            ANIM.SetTrigger("TouchedPlayer");
        }
    }
    private void AfterKaboom()
    {
        Destroy(gameObject,0.1f);
    }
    private void BoomSound()
    {        
        MY_AUDIOSOURCE.PlayOneShot(bombSounds[1]);
    }
}
