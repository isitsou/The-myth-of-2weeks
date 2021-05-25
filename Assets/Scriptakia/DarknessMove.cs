using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarknessMove : MonoBehaviour
{
    //public vars
    [SerializeField] float upSpeed=0.2f;


    

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector2(0, upSpeed * Time.deltaTime));
    }
}
