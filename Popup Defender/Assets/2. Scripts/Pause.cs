using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    private GameState myState;

    public GameObject myPauseBttn;
    public GameObject myResumeBttn;
    public GameObject pauseMenu;
    public GameObject myHowToPlay;

    private void Start()
    {
        myState = GetComponent<GameState>();
    }

    private void Update()
    {
        if (GetComponent<GameState>().state == GameCurrentState.END)
        {
            myPauseBttn.SetActive(false);
        }
    }

    public void PauseClicked()
    {
        FindObjectOfType<AudioManager>().Play("Button");
        myState.state = GameCurrentState.PAUSED;
        myPauseBttn.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void ResumeClicked()
    {
        FindObjectOfType<AudioManager>().Play("Button");
        myState.state = GameCurrentState.START;
        pauseMenu.SetActive(false);
        myPauseBttn.SetActive(true);
    }

    public void QuitClicked()
    {
        FindObjectOfType<AudioManager>().Play("Button");
        SceneManager.LoadScene(0);
    }

    public void HelpClicked()
    {
        FindObjectOfType<AudioManager>().Play("Button");
        pauseMenu.SetActive(false);
        myHowToPlay.SetActive(true);
    }

    public void CloseHelpClicked()
    {
        FindObjectOfType<AudioManager>().Play("Button");
        myHowToPlay.SetActive(false);
        pauseMenu.SetActive(true);
    }
}
