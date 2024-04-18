using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public string selectScreen; // change this string in the start button to the tile select screen
    public GameObject rules;
    public GameObject title;
    
    public void StartGame ()
    {
        SceneManager.LoadScene("TileSelect");
    }
    public void ExitGame ()
    {
        Application.Quit();
    }
    public void ShowMenu ()
    {
        rules.SetActive(true);
        title.SetActive(false);
    }
    public void HideMenu ()
    {
        rules.SetActive(false);
        title.SetActive(true);
    }
}
