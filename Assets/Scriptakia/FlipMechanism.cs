using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipMechanism : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Foreground"))
        {
            GetComponentInParent<EnemyWalking>().Flip();
            GetComponentInParent<EnemyWalking>().runningOutOfPlatform = true;
        }
    }
}
