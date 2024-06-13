using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.EventSystems;
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
    public PlayerManager pm;
    public GameObject gameEndMenu;
    public EventSystem eventSystem;
    private int playersDead = 0;
    private string[] playerNames = {"Player 1", "Player 2", "Player 3", "Player 4"};

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
        playersDead++;
        pm.PlayerKilled(playerName);
        if(lastPlayer())
        {
            DisplayGameEndText(pm.GetLastPlayer());
            gameEndMenu.SetActive(true);
            eventSystem.SetSelectedGameObject(gameEndMenu.transform.GetChild(1).gameObject);
        }
    }

    private void DisplayAnnouncementText(string currentPlayer)
    {
        int pnum = um.convertPlayerName(currentPlayer);
        playerAnnouncementText.text = playerNames[pnum] + " Is Out!";
    }

    private void DisplayGameEndText(string currentPlayer)
    {
        int pnum = um.convertPlayerName(currentPlayer);
        playerAnnouncementText.text = playerNames[pnum] + " Wins!";
    }

    private void DisableAnnouncementText()
    {
        playerAnnouncementText.text = "";
    }

    private bool lastPlayer()
    {
        return pm.GetPlayerCount() - playersDead <= 1;
    }

}
