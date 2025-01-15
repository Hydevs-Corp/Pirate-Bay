using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // public GameObject Menu;
    // public GameObject CreditsMenu;


    public void PlayNowButton()
    {
        print("HELLO");
        // Play Now Button has been pressed, here you can initialize your game (For example Load a Scene called GameLevel etc.)
        UnityEngine.SceneManagement.SceneManager.LoadScene("Funcs");
    }

    // public void CreditsButton()
    // {
    //     // Show Credits Menu
    //     Menu.SetActive(false);
    //     CreditsMenu.SetActive(true);
    // }

    // public void MainMenuButton()
    // {
    //     // Show Main Menu
    //     Menu.SetActive(true);
    //     CreditsMenu.SetActive(false);
    // }

    public void QuitButton()
    {
        // Quit Game
        Application.Quit();
    }
}