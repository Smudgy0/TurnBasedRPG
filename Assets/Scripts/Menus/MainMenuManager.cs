using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject OptionsMenu;
 
    public void StartButton()
    {
        SceneManager.LoadScene("Starting Village");
    }

    public void OptionsButton()
    {
        OptionsMenu.SetActive(true);
    }

    public void CloseOptions()
    {
        OptionsMenu.SetActive(false);
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
