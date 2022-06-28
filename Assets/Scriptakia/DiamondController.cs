using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondController : MonoBehaviour
{
    //Cached component refs
    [SerializeField] AudioClip coinPickupSFX=null;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Player" && collision is CapsuleCollider2D)
        {           
            AudioSource.PlayClipAtPoint(coinPickupSFX, Camera.main.transform.position);            
            GetComponent<Animator>().SetTrigger("TouchedByPlayer");
            GetComponent<PolygonCollider2D>().enabled = false;
            GameSessionController.instance.AddDiamond();
        }
    }
    private void CompleteDeath()
    {
        Destroy(gameObject);
    }
}
