using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scene : MonoBehaviour
{
    public GameObject desktopPage;
    public GameObject browserPage;
    public GameObject websitePage;
    public GameObject dayPage;

    public Text dayText;

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

        StartCoroutine(DesktopPage());
    }

    IEnumerator DesktopPage()
    {
        yield return new WaitForSeconds(5);

        dayPage.SetActive(false);
        desktopPage.SetActive(true);
    }
}
