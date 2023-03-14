using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsMainMenu : MonoBehaviour
{
    private Scene myScene;
    private RaycastHit2D hit;

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
                    GameObject.Find("Title").GetComponent<MainMenuTransistion>().playGame = true;
                }
                else if (hit.collider.gameObject.name == "Power Bttn")
                {
                    Debug.Log("Power Bttn Clicked");
                    myScene.Quit();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Return) && GameObject.Find("Title").GetComponent<MainMenuTransistion>().playGame == false)
        {
            GameObject.Find("Title").GetComponent<MainMenuTransistion>().playGame = true;
        }
    }
}
