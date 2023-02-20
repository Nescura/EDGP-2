using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scene : MonoBehaviour
{
    public GameObject desktopPage, browserPage, websitePage;
    public GameObject browserButton, websiteButton, submitButton;

    public GameObject dayPage;
    public Text dayText;

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
    }

    public void WebsiteButton()
    {
        browserPage.SetActive(false);
        websitePage.SetActive(true);
    }

    public void SubmitButton()
    {
        websitePage.SetActive(false);
        dayPage.SetActive(true);
        myController.GetComponent<GameTimer>().myDeadLineTxt.enabled = false;

        StartCoroutine(DesktopPage());
    }
    #endregion

    IEnumerator DesktopPage()
    {
        yield return new WaitForSeconds(3);

        dayPage.SetActive(false);
        myController.GetComponent<GameTimer>().ResetTimer();
        myController.GetComponent<GameTimer>().myDeadLineTxt.enabled = true;
        desktopPage.SetActive(true);
    }

    public void ButtonCanInteract()
    {
        if (GameControlling.GetInstance().GetActivePanelCount() > 0)
        {
            browserButton.GetComponent<Button>().interactable = false;
            websiteButton.GetComponent<Button>().interactable = false;
            submitButton.GetComponent<Button>().interactable = false;
        }
        else
        {
            browserButton.GetComponent<Button>().interactable = true;
            websiteButton.GetComponent<Button>().interactable = true;
            submitButton.GetComponent<Button>().interactable = true;
        }
    }

    private void Update()
    {
        ButtonCanInteract();
    }
}
