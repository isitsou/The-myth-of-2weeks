using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aura : MonoBehaviour
{
    [HideInInspector] public float direction;
    [SerializeField] private float speed = 10;
    [SerializeField] private float lifetime = 1;
    private CapsuleCollider2D myCol;
    private SpriteRenderer mySR;
    private Rigidbody2D myRB;
    private Vector2 startingLocalPos;



    IEnumerator Start()
    {
        myCol = GetComponent<CapsuleCollider2D>();
        mySR = GetComponent<SpriteRenderer>();
        myRB = GetComponent<Rigidbody2D>();
        yield return null;
        myRB.AddForce(Vector2.right * speed * direction, ForceMode2D.Impulse);

        float stepX = transform.localScale.x / 100;
        float stepY = transform.localScale.y / 100;
        float startingScaleX = transform.localScale.x;
        float startingScaleY = transform.localScale.y;
        for (float i = 0; i < 1; i += 0.02f)
        {
            transform.localScale = new Vector3(Mathf.Lerp(startingScaleX, startingScaleX * 3, i),
                Mathf.Lerp(startingScaleY, startingScaleY * 3, i),
                1);
            mySR.color = new Color(mySR.color.r, mySR.color.g, mySR.color.b, Mathf.InverseLerp(1, 0.4f, i));
            yield return new WaitForSeconds(lifetime/100);
        }

        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {
            Animator ANIM = null;
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
        }
    }


}
