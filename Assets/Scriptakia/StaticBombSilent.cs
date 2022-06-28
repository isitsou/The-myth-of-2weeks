using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticBombSilent : MonoBehaviour
{
    //cached stuff
    Animator ANIM;
    [SerializeField] AudioClip booomSound=null;
    

    private void Start()
    {
        ANIM = GetComponent<Animator>();
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
        Destroy(gameObject);
    }
    private void BoomSound()
    {
        AudioSource.PlayClipAtPoint(booomSound, transform.position, 0.1f);
    }
}
