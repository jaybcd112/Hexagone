using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Globalization;
using System;

public class PlayerManager : MonoBehaviour
{
    private int playerCount = 0;
    public Camera followCam;
    public GameObject[] playerUi;
    public UIManager um;
    private List<PlayerConfiguration> playerConfigs;
    public List<string> playerNames;

    [SerializeField]
    public static PlayerManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.Log("[Singleton] Trying to instantiate a second instance of a singleton class.");
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
            playerConfigs = new List<PlayerConfiguration>();
        }
        playerNames = new List<string>();
    }

    void Start()
    {
        var playerInputManager = FindObjectOfType<PlayerInputManager>();
        if (playerInputManager != null)
        {
            playerInputManager.onPlayerJoined += HandlePlayerJoined;
        }
    }

    public void HandlePlayerJoined(PlayerInput pi)
    {
        Debug.Log("player joined " + pi.playerIndex);
        //pi.transform.SetParent(transform);

        if (!playerConfigs.Any(p => p.PlayerIndex == pi.playerIndex))
        {
            playerConfigs.Add(new PlayerConfiguration(pi));
            pi.gameObject.name = "Player" + playerCount;
            playerCount++;
            playerNames.Add(pi.gameObject.name);
        }
        followCam.GetComponent<CameraControl>().AddTarget(pi.gameObject);
        Debug.Log(pi.gameObject.name);
        um.ToggleUIElement(pi.gameObject.name, true);
    }

    public List<PlayerConfiguration> GetPlayerConfigs()
    {
        return playerConfigs;
    }

    void OnDestroy()
    {
        // Clean up to avoid memory leaks
        if (PlayerInputManager.instance != null)
        {
            PlayerInputManager.instance.onPlayerJoined -= HandlePlayerJoined;
        }
    }
    
    public int GetPlayerCount()
    {
        return playerCount;
    }

    public string GetLastPlayer()
    {
        return playerNames[0];
    }

    public void PlayerKilled(string name)
    {
        playerNames.Remove(name);
    }
}

public class PlayerConfiguration
{
    public PlayerConfiguration(PlayerInput pi)
    {
        PlayerIndex = pi.playerIndex;
        Input = pi;
    }

    public PlayerInput Input { get; private set; }
    public int PlayerIndex { get; private set; }
    public Material playerMaterial {get; set;}
}