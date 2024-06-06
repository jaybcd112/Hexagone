using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public Animator animator; // Animator for the transitions
    public int currentStep = 0; // Current step of the menu

    public GameObject startMenu;
    public GameObject playerSelectMenu;
    public GameObject mapSelectMenu;

    void Start()
    {
        // Set the current step to 0
        currentStep = 0;
        animator.SetInteger("currentStep", currentStep);
    }

    void Update()
    {
        // Update the UI
        UpdateUI();
    }

    public void changeStep (int step)
    {
        currentStep += step;
        animator.SetInteger("currentStep", currentStep);
    }

    public void UpdateUI ()
    {
        if (currentStep == 0)
        {
            DisableUI();

            // Enable start menu
            startMenu.SetActive(true);
        }
        if (currentStep == 1)
        {
            DisableUI();

            // Enable player select menu
            playerSelectMenu.SetActive(true);
        }
        if (currentStep == 2)
        {
            DisableUI();

            // Enable map select menu
            mapSelectMenu.SetActive(true);
        }
    }

    public void DisableUI ()
    {
        // Disable all menus
        startMenu.SetActive(false);
        playerSelectMenu.SetActive(false);
        mapSelectMenu.SetActive(false);
    }

    public void LoadScene (string scene)
    {
        SceneManager.LoadScene(scene);
    }
    public void ExitGame ()
    {
        Application.Quit();
    }
}
