using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigInBoxController : MonoBehaviour
{
    //Cached components refs
    private Animator ANIM;
    public MovingBox M_BOX;

    //Serialized vars
    [SerializeField] private Vector3 boxPositionOffset = new Vector3(0.22f, 0.4f, 0);
    [SerializeField] private Vector2 boxDirection = new Vector2(-3.5f, 5f);
    [SerializeField] float pushingPowerModifier = 1f;
    [SerializeField] float raycastDistance = 10f;

    //Vars
    private float direction = -1f;    
    private bool iHaveVision = false;
    private RaycastHit2D hit;




    // Start is called before the first frame update
    void Start()
    {
        ANIM = GetComponent<Animator>();
        boxDirection = boxDirection * pushingPowerModifier;
    }

    // Update is called once per frame
    void Update()
    {
        //check for vision
        Vision();


    }
    private void ThrowBox()
    {
        //throw the box in the direction that the enemy is currently looking       
        M_BOX.direction = new Vector2(Mathf.Sign(transform.localScale.x) * boxDirection.x, boxDirection.y);
        //print(M_BOX.direction);

        //generate the moving box
        Instantiate(M_BOX, transform.position + new Vector3(Mathf.Sign(transform.localScale.x) * boxPositionOffset.x, boxPositionOffset.y, 0), Quaternion.identity);
    }
    private void Vision()
    {
        if (iHaveVision)
        {
            //CAST RAY
            Debug.DrawRay(transform.position, new Vector3(direction * raycastDistance, 0, 0), Color.green);
            hit = Physics2D.Raycast(transform.position, new Vector2(direction, 0), raycastDistance, LayerMask.GetMask("Player","Foreground"));
            //check if you hit anything
            if (hit.collider.gameObject.name == "Player")
            {
                ANIM.SetBool("SawPlayer", true);
            }
            else
            {
                ANIM.SetBool("SawPlayer", false);
            }
        }
    }
    private void ICanSee()
    {
        iHaveVision = true;
    }
    private void ICanNOTSee()
    {
        iHaveVision = false;
    }



    private void Flip()
    {
        transform.localScale = new Vector3(direction, 1, 1);
        direction = -direction;
    }
    private void CompleteDeath()
    {
        Destroy(gameObject);
    }

}
