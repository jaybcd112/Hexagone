using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using TMPro;

public class RespawnZone : MonoBehaviour
{
    public float playerAnnouncementTime = 5f;
    public Transform respawnPoint;
    public TextMeshProUGUI playerAnnouncementText;
    public GameObject cameraControls;
    public AudioSource deathRah;

    public UIManager um;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
            deathRah.Play();
            playerController.UpdateLives(-1);
            if (playerController.GetLives() <= 0)
            {
                StartCoroutine(PlayerDead(other.gameObject));
                return;
            }

            playerController.SetStunned(false);
            playerController.stunParticle.Stop();
            ParticleSystem.MainModule mainModule = playerController.stunParticle.main;
            mainModule.startLifetime = 1f;

            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            other.transform.position = respawnPoint.position;
        }
    }

    IEnumerator PlayerDead(GameObject player)
    {
        string playerName = player.name;
        cameraControls.GetComponent<CameraControl>().RemoveTarget(player);
        um.ToggleUIElement(playerName, false);
        DisplayAnnouncementText(playerName);
        yield return new WaitForSeconds(playerAnnouncementTime);
        PlayerController pc = player.GetComponent<PlayerController>();
        pc.Freeze();
        DisableAnnouncementText();
    }

    private void DisplayAnnouncementText(string currentPlayer)
    {
        switch (currentPlayer)
        {
            case "Player0":
                playerAnnouncementText.text = "Player 1 Is Out!";
                break;
            case "Player1":
                playerAnnouncementText.text = "Player 2 Is Out!";
                break;
            case "Player2":
                playerAnnouncementText.text = "Player 3 Is Out!";
                break;
            case "Player3":
                playerAnnouncementText.text = "Player 4 Is Out!";
                break;
            default:
                Debug.LogError("Invalid player number!");
                break;
        }
    }

    private void DisableAnnouncementText()
    {
        playerAnnouncementText.text = "";
    }

}
