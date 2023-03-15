using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsMainMenu : MonoBehaviour
{
    private Scene myScene;
    private RaycastHit2D hit;
    public GameObject settingsBttn;
    public GameObject optionMenu;
    public GameObject how2play;
    private MainMenuTransistion myMainMenuStuff;

    // Start is called before the first frame update
    void Start()
    {
        myScene = GameObject.Find("Scene").GetComponent<Scene>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
        hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

        if (Input.GetMouseButtonDown(0))
        {
            if (hit.collider != null && hit.collider.gameObject.tag == "Buttons")
            {
                if (hit.collider.gameObject.name == "Play Bttn")
                {
                    Debug.Log("Play Bttn Clicked");
                    if (myScene.canClick == true)
                    {
                        GameObject.Find("Title").GetComponent<MainMenuTransistion>().playGame = true;
                    }
                    
                }
                else if (hit.collider.gameObject.name == "Power Bttn")
                {
                    Debug.Log("Power Bttn Clicked");
                    if (myScene.canClick == true)
                    {
                        myScene.Quit();
                    }
                }
                else if (hit.collider.gameObject.name == "Setting Bttn")
                {
                    Debug.Log("Settings Bttn Clicked");
                    hit.collider.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
                    settingsBttn.SetActive(false);
                    optionMenu.SetActive(true);
                    myScene.canClick = false;
                }
                else if (hit.collider.gameObject.name == "Close Button")
                {
                    Debug.Log("Close Bttn Clicked");
                    hit.collider.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
                    settingsBttn.SetActive(true);
                    optionMenu.SetActive(false);
                    myScene.canClick = true;
                }
                else if (hit.collider.gameObject.name == "Option Button")
                {
                    Debug.Log("Option Bttn Clicked");
                    hit.collider.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
                }
                else if (hit.collider.gameObject.name == "How To Play Button")
                {
                    Debug.Log("How To Play Bttn Clicked");
                    hit.collider.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
                    how2play.SetActive(true);
                    optionMenu.SetActive(false);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (GameObject.Find("Title").GetComponent<MainMenuTransistion>().playGame == false)
            {
                GameObject.Find("Title").GetComponent<MainMenuTransistion>().playGame = true;
            }
        }
    }

    void OnMouseOver()
    {
        // Change the color of the GameObject to red when the mouse is over GameObject

        if (gameObject.tag == "Buttons")
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f);
        }
    }

    void OnMouseExit()
    {
        if (gameObject.tag == "Buttons")
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
        }
    }

    public void CloseHow2Play()
    {
        how2play.SetActive(false);
        myScene.canClick = true;
        settingsBttn.SetActive(true);
    }
}
