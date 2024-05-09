using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnZone : MonoBehaviour
{

    public Transform respawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.gameObject.GetComponent<PlayerController>().getLives() == 1)
            {
                other.gameObject.GetComponent<PlayerController>().dead();
            }
            other.gameObject.GetComponent<PlayerController>().updateLives(-1);
            other.transform.position = respawnPoint.position;
        }
    }
}
