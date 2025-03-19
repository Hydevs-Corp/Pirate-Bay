using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // public GameObject Menu;
    // public GameObject CreditsMenu;
    private GameObject BGPause;
    private GameObject Resume;
    private GameObject Restart;
    private GameObject PauseMenu;
    private GameObject Quit;

    public GameObject SkinMenu;

    void Start()
    {
        BGPause = GameObject.Find("BGPause");
        Resume = GameObject.Find("Resume");
        Restart = GameObject.Find("RestartPause");
        PauseMenu = GameObject.Find("MainMenuPause");
        Quit = GameObject.Find("QuitPause");
    }

    public void ShowSkinMenu()
    {
        if (SkinMenu != null)
            SkinMenu.SetActive(true);
    }

    public void HideSkinMenu()
    {
        if (SkinMenu != null)
            SkinMenu.SetActive(false);
    }

    public void PlayNowButton()
    {
        // Play Now Button has been pressed, here you can initialize your game (For example Load a Scene called GameLevel etc.)
        UnityEngine.SceneManagement.SceneManager.LoadScene("Funcs");
    }

    public void RestartButton()
    {
        // Play Now Button has been pressed, here you can initialize your game (For example Load a Scene called GameLevel etc.)
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Funcs");
    }

    public void MainMenuButton()
    {
        // Play Now Button has been pressed, here you can initialize your game (For example Load a Scene called GameLevel etc.)
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
    public void ResumeButton()
    {
        // Resume Game
        Time.timeScale = 1;
        BGPause.SetActive(false);
        Resume.SetActive(false);
        Restart.SetActive(false);
        PauseMenu.SetActive(false);
        Quit.SetActive(false);

    }
    public void QuitButton()
    {
        // Quit Game
        Application.Quit();
    }
}