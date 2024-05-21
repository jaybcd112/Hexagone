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
            if (other.gameObject.GetComponent<NewPlayerController>().GetLives() == 1)
            {
                other.gameObject.GetComponent<NewPlayerController>().Dead();
            }
            other.gameObject.GetComponent<NewPlayerController>().UpdateLives(-1);
            other.transform.position = respawnPoint.position;
        }
    }
}
