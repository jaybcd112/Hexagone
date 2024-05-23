using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    private int playerCount = 0;
    public Camera followCam;
    public GameObject[] playerUi;

    void Start()
    {
        var playerInputManager = FindObjectOfType<PlayerInputManager>();
        if (playerInputManager != null)
        {
            playerInputManager.onPlayerJoined += HandlePlayerJoined;
        }
    }

    public void HandlePlayerJoined(PlayerInput playerInput)
    {
        playerCount++;
        playerInput.gameObject.name = "Player" + playerCount;
        followCam.GetComponent<CameraControl>().AddTarget(playerInput.gameObject);   
        EnableUIElements(playerInput.gameObject.name);
        EnableHealthIcons(playerInput.gameObject);
    }

    void OnDestroy()
    {
        // Clean up to avoid memory leaks
        if (PlayerInputManager.instance != null)
        {
            PlayerInputManager.instance.onPlayerJoined -= HandlePlayerJoined;
        }
    }

    public void EnableHealthIcons(GameObject currentPlayer)
    {
        string playerName = currentPlayer.name;

        switch (playerName)
        {
            case "Player1":
                currentPlayer.GetComponent<NewPlayerController>().healthIcons = playerUi[0].GetComponentsInChildren<Image>();
                break;
            case "Player2":
                currentPlayer.GetComponent<NewPlayerController>().healthIcons = playerUi[1].GetComponentsInChildren<Image>();
                break;
            case "Player3":
                currentPlayer.GetComponent<NewPlayerController>().healthIcons = playerUi[2].GetComponentsInChildren<Image>();
                break;
            case "Player4":
                currentPlayer.GetComponent<NewPlayerController>().healthIcons = playerUi[3].GetComponentsInChildren<Image>();
                break;
            default:
                Debug.LogError("Invalid player number!");
                break;
        }
    }
    public void EnableUIElements(string currentPlayer)
    {
        switch (currentPlayer)
        {
            case "Player1":
                playerUi[0].SetActive(true);

                break;
            case "Player2":
                playerUi[1].SetActive(true);
                break;
            case "Player3":
                playerUi[2].SetActive(true);
                break;
            case "Player4":
                playerUi[3].SetActive(true);
                break;
            default:
                Debug.LogError("Invalid player number!");
                break;
        }
    }

}