using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject[] playerUI;
    public TextMeshProUGUI percentageText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleUIElement(string playerName, bool toggle)
    {
        int playerNumber = convertPlayerName(playerName);
        if (playerNumber < 0 | playerNumber > 3)
            Debug.LogError("Invalid playerNumber!");
        else
            playerUI[playerNumber].SetActive(toggle);
    }

    public void DisableHealthIcon(string playerName, int index)
    {
        GameObject p = playerUI[convertPlayerName(playerName)];
        Image[] healthIcons = p.transform.GetChild(1).GetComponentsInChildren<Image>();
        healthIcons[index].enabled = false;
    }

    public void UpdatePercentage(string playerName, float percentage)
    {
        GameObject p = playerUI[convertPlayerName(playerName)];
        int index = (int)percentage / 20;
        if(index < 7)
        {
            Image[] percentageIcons = p.transform.GetChild(0).GetComponentsInChildren<Image>(true);
            Debug.Log(index);
            percentageIcons[index].enabled = true;
        }
        TextMeshProUGUI percentageText = p.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        percentageText.text = percentage.ToString() + "%";
    }

    public void ResetPercentage(string playerName)
    {
        GameObject p = playerUI[convertPlayerName(playerName)];
        Image[] percentageIcons = p.transform.GetChild(0).GetComponentsInChildren<Image>(true);
        for(int i = 1; i < percentageIcons.Length; i++)
            percentageIcons[i].enabled = false;
        TextMeshProUGUI percentageText = p.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        percentageText.text = "0%";
    }

    private int convertPlayerName(string playerName)
    {
        return Int32.Parse(playerName[6..]);
    }
}
