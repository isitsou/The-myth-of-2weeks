using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticBomb : MonoBehaviour
{
    //cached stuff
    Animator ANIM;
    AudioSource MY_AUDIOSOURCE;

    //public vars
    [SerializeField] private AudioClip[] bombSounds=null;
    
    private void Start()
    {
        ANIM = GetComponent<Animator>();

        //play sound fuse
        MY_AUDIOSOURCE = GetComponent<AudioSource>();
       // AudioClip temp = bombSounds[0];
        MY_AUDIOSOURCE.clip = bombSounds[0];
        MY_AUDIOSOURCE.Play();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            ANIM.SetTrigger("TouchedPlayer");
        }
    }
    private void AfterKaboom()
    {
        Destroy(gameObject);
    }
    private void BoomSound()
    {
        AudioSource.PlayClipAtPoint(bombSounds[1], transform.position, 0.3f);
    }
}
