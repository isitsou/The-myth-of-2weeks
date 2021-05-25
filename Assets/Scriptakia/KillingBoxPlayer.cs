using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillingBoxPlayer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {
            Animator ANIM=null;
            //check if the gameObject that you collide with has a component Animator, if it doesnt move on
            if (collision.gameObject.TryGetComponent(out Animator anim))
            {
                ANIM = collision.gameObject.GetComponent<Animator>();                
            }
            // check if null because "Animator" is reference not a variable and references can not be null when you try to access them. If you try to access them you get referencing error
            if (ANIM != null)
            {                
                ANIM.SetTrigger("AttackedByPlayer");
            }
            
            //push player upwards on Attack
            GetComponentInParent<Rigidbody2D>().AddForce(new Vector2(0,15), ForceMode2D.Impulse);
        }
        
    }
}
