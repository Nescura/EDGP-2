using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scene : MonoBehaviour
{
    //public string levelToLoad;

    public GameObject ui;
    public static bool isPaused;
    public static string difficulty;

    public void Play()
    {
        SceneManager.LoadScene(1);
        FindObjectOfType<AudioManager>().Stop("MenuBGM");
        FindObjectOfType<AudioManager>().Play("GameBGM");
    }

    public void Quit()
    {
        FindObjectOfType<AudioManager>().Play("Button");

        Application.Quit();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Toggle();
        }
    }

    public void Toggle()
    {
        ui.SetActive(!ui.activeSelf);

        if (ui.activeSelf)
        {
            Time.timeScale = 0f;
            isPaused = true;
        }
        else
        {
            Time.timeScale = 1f;
            isPaused = false;
        }
    }

    public void Retry()
    {
        FindObjectOfType<AudioManager>().Play("Button");

        Toggle();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        FindObjectOfType<AudioManager>().Play("Button");
        FindObjectOfType<AudioManager>().Stop("GameBGM");
        FindObjectOfType<AudioManager>().Play("MenuBGM");

        // just in case this function was accessed from PauseMenu
        Time.timeScale = 1f;
        isPaused = false;
        
        SceneManager.LoadScene(0);
    }
}
