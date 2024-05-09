using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    private int playerCount = 0;
    public Camera camera; 

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
        playerCount++;  // Increment the player count
        playerInput.gameObject.name = "Player" + playerCount;  // Assign the new name
        camera.GetComponent<CameraControl>().addTarget(playerInput.gameObject); // Add the new player to the camera targets   

        // Find all of the health icons for the new player and assign them to the player controller
        playerInput.gameObject.GetComponent<PlayerController>().healthIcons = GameObject.Find("Canvas/Player" + playerCount + "Icon").GetComponentsInChildren<UnityEngine.UI.Image>();
    }

    void OnDestroy()
    {
        // Clean up to avoid memory leaks
        if (PlayerInputManager.instance != null)
        {
            PlayerInputManager.instance.onPlayerJoined -= HandlePlayerJoined;
        }
    }
}