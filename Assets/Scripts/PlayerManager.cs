using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    private int playerCount = 0;
    public Camera followCam;
    public GameObject[] playerUi;
    private List<PlayerConfiguration> playerConfigs;

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
            playerCount++;
            pi.gameObject.name = "Player" + playerCount;
        }
        followCam.GetComponent<CameraControl>().AddTarget(pi.gameObject);
            EnableUIElements(pi.gameObject.name);
            EnableHealthIcons(pi.gameObject);
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

    public void EnableHealthIcons(GameObject currentPlayer)
    {
        string playerName = currentPlayer.name;

        switch (playerName)
        {
            case "Player1":
                currentPlayer.GetComponent<PlayerController>().healthIcons = playerUi[0].GetComponentsInChildren<Image>();
                break;
            case "Player2":
                currentPlayer.GetComponent<PlayerController>().healthIcons = playerUi[1].GetComponentsInChildren<Image>();
                break;
            case "Player3":
                currentPlayer.GetComponent<PlayerController>().healthIcons = playerUi[2].GetComponentsInChildren<Image>();
                break;
            case "Player4":
                currentPlayer.GetComponent<PlayerController>().healthIcons = playerUi[3].GetComponentsInChildren<Image>();
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