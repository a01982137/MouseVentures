using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderDeath : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")){
            Destroy(gameObject.transform.parent.gameObject);
        }
    }
}
