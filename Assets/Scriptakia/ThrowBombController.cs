using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowBombController : MonoBehaviour
{
    //cached components refs
    public MovingBomb M_BOMB;
    Animator ANIM;

    //public vars
    [SerializeField] private Vector3 bombPositionOffset=new Vector3(-0.2f,0.2f,0);
    [SerializeField] private Vector2 bombDirection= new Vector2(-3.5f,5f);

    private void Start()
    {
        ANIM = GetComponent<Animator>();
    }
    //throw da fucking bomb
    private void ThrowBomb()
    {
        M_BOMB.direction = bombDirection;
        Instantiate(M_BOMB, transform.position + bombPositionOffset, Quaternion.identity);
    }
   
    private void CompleteDeath()
    {
        Destroy(gameObject);
    }
}
