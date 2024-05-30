using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using TMPro;

public class RespawnZone : MonoBehaviour
{
    public float playerAnnouncementTime = 5f;
    public Transform respawnPoint;
    public GameObject[] playerUi;
    public TextMeshProUGUI playerAnnouncementText;
    public GameObject cameraControls;
    public AudioSource deathRah;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            deathRah.Play();
            if (other.gameObject.GetComponent<PlayerController>().GetLives() == 1)
            {
                StartCoroutine(PlayerDead(other.gameObject));
                return;
            }
            other.gameObject.GetComponent<PlayerController>().UpdateLives(-1);
            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            other.transform.position = respawnPoint.position;
        }
    }

    IEnumerator PlayerDead(GameObject player)
    {
        string playerName = player.name;
        cameraControls.GetComponent<CameraControl>().RemoveTarget(player);
        Destroy(player);
        DisableUi(playerName);
        DisplayAnnouncementText(playerName);

        yield return new WaitForSeconds(playerAnnouncementTime);

        DisableAnnouncementText();
    }

    private void DisableUi(string currentPlayer)
    {
        switch (currentPlayer)
        {
            case "Player1":
                playerUi[0].SetActive(false);
                break;
            case "Player2":
                playerUi[1].SetActive(false);
                break;
            case "Player3":
                playerUi[2].SetActive(false);
                break;
            case "Player4":
                playerUi[3].SetActive(false);
                break;
            default:
                Debug.LogError("Invalid player number!");
                break;
        }
    }

    private void DisplayAnnouncementText(string currentPlayer)
    {
        switch (currentPlayer)
        {
            case "Player1":
                playerAnnouncementText.text = "Player 1 Is Out!";
                break;
            case "Player2":
                playerAnnouncementText.text = "Player 2 Is Out!";
                break;
            case "Player3":
                playerAnnouncementText.text = "Player 3 Is Out!";
                break;
            case "Player4":
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
