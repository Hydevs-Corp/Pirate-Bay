using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // public GameObject Menu;
    // public GameObject CreditsMenu;
    private GameObject PauseText;
    private GameObject Resume;
    private GameObject Restart;
    private GameObject PauseMenu;
    private GameObject Quit;

    void Start()
    {
        PauseText = GameObject.Find("Pause");
        Resume = GameObject.Find("Resume");
        Restart = GameObject.Find("RestartPause");
        PauseMenu = GameObject.Find("MainMenuPause");
        Quit = GameObject.Find("QuitPause");

        PauseText.SetActive(false);
        Resume.SetActive(false);
        Restart.SetActive(false);
        PauseMenu.SetActive(false);
        Quit.SetActive(false);

    }


    public void PlayNowButton()
    {
        print("HELLO");
        // Play Now Button has been pressed, here you can initialize your game (For example Load a Scene called GameLevel etc.)
        UnityEngine.SceneManagement.SceneManager.LoadScene("Funcs");
    }

    public void RestartButton()
    {
        print("HELLO");
        // Play Now Button has been pressed, here you can initialize your game (For example Load a Scene called GameLevel etc.)
        UnityEngine.SceneManagement.SceneManager.LoadScene("Funcs");
    }

    public void MainMenuButton()
    {
        print("Menu");
        // Play Now Button has been pressed, here you can initialize your game (For example Load a Scene called GameLevel etc.)
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
    public void ResumeButton()
    {
        // Resume Game
        Time.timeScale = 1;
        PauseText.SetActive(false);
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