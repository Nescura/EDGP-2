using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scene : MonoBehaviour
{
    public GameObject desktopPage, browserPage, websitePage;
    public GameObject browserBttn, websiteBttn, submitBttn;

    public GameObject dayPage;
    public Text dayText;
    public bool canInteract;

    private GameObject myController;

    private void Start()
    {
        myController = GameObject.Find("GameCuntroller");
    }

    #region Bttn Functions
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

    public void Retry()
    {
        FindObjectOfType<AudioManager>().Play("Button");

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        FindObjectOfType<AudioManager>().Play("Button");
        FindObjectOfType<AudioManager>().Stop("GameBGM");
        FindObjectOfType<AudioManager>().Play("MenuBGM");
        
        SceneManager.LoadScene(0);
    }

    public void BrowserButton()
    {
        desktopPage.SetActive(false);
        browserPage.SetActive(true);

        myController.GetComponent<GameControlling>().spawnPanelTime = 0f;
    }

    public void WebsiteButton()
    {
        browserPage.SetActive(false);
        websitePage.SetActive(true);

        myController.GetComponent<GameControlling>().spawnPanelTime = 0f;
    }

    public void SubmitButton()
    {
        websitePage.SetActive(false);
        dayPage.SetActive(true);
        myController.GetComponent<GameTimer>().myDeadLineTxt.enabled = false;
        myController.GetComponent<GameControlling>().spawnPanelTime = 99f;
        myController.GetComponent<Levels>().CheckCurrentLevel();
        StartCoroutine(DesktopPage());
    }
    #endregion

    IEnumerator DesktopPage()
    {
        yield return new WaitForSeconds(1.5f);

        dayPage.SetActive(false);
        myController.GetComponent<GameTimer>().ResetSystemTimer();
        myController.GetComponent<GameTimer>().myDeadLineTxt.enabled = true;
        desktopPage.SetActive(true);
        myController.GetComponent<GameControlling>().spawnPanelTime = 0f;
    }

    public void ButtonCanInteract()
    {
        if (GameControlling.GetInstance().GetActivePanelCount() > 0)
        {
            canInteract = false;

            browserBttn.GetComponent<SpriteRenderer>().color = new Color(125f / 225f, 125f / 225f, 125f / 225f);
            websiteBttn.GetComponent<SpriteRenderer>().color = new Color(125f / 225f, 125f / 225f, 125f / 225f);
            submitBttn.GetComponent<SpriteRenderer>().color = new Color(125f / 225f, 125f / 225f, 125f / 225f);
        }
        else
        {
            canInteract = true;

            browserBttn.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
            websiteBttn.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
            submitBttn.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
        }
    }

    private void Update()
    {
        ButtonCanInteract();
    }
}
