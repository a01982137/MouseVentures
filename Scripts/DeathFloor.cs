using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathFloor : MonoBehaviour
{
    public Transform player, destination;
    public GameObject playerg;

    void OnCollisionEnter(Collision collision)
    {
        playerg.SetActive(false);
        player.position = destination.position;
        playerg.SetActive(true);
    }


}
