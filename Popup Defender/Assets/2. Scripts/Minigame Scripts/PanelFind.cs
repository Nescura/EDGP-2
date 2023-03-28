using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelFind : IPanelStrategy // DO NOT EDIT THIS TEMPLATE - Copy and paste it when making new minigames, and remember to make the class name and file name the same
{
    // MAKE SURE THESE VARIABLES ARE IN
    GameObject myPanel, myDisplay;
    float sizeX, sizeY;

    // Extra variables go under here
    private GameObject balloon, crosshair;
    private Vector3 balloonPos;

    public Vector2 SetPanelSize() => new Vector2(sizeX, sizeY); 
    public string SetPanelBG() => "";
    public int ObjectiveKeyTech() => 1;  // 0 for Press, 1 for Tap, 2 for Hold, 3 for Spam
    public string ObjectiveDesc() => "to do something!"; // tell the player what pressing the key does

    public void ResetMinigame(GameObject panelParent, GameObject displayParent)
    {
        myPanel = panelParent; myDisplay = displayParent;
        sizeX = 1.5f; sizeY = 1.5f; // You can do a lot with the size of panels and even vary it up like so

        Vector2 viewportZero = GameControlling.GetInstance().mainCamera.ViewportToWorldPoint(Vector2.zero);
        Vector2 viewportOne = GameControlling.GetInstance().mainCamera.ViewportToWorldPoint(Vector2.one);

        if (balloon == null) // this line is mostly to account for object pooling later, please make you do this
		{
            balloon = GameObject.Instantiate(Resources.Load("Balloon"), myDisplay.transform) as GameObject;
            crosshair = GameObject.Instantiate(Resources.Load("Crosshair"), myDisplay.transform) as GameObject;
        }

        balloonPos = new Vector3(Random.Range(viewportZero.x + 1, viewportOne.x - 1), Random.Range(viewportZero.y + 1, viewportOne.y - 1), 1);
    }

    public void OnControlDown() // Runs on the frame the key is pressed. Should happen only once per press
    {
        if (crosshair.GetComponent<Collider2D>().IsTouching(balloon.GetComponent<Collider2D>()))
        {
            balloon.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("BGSprites/SniperBoom");

            myPanel.GetComponent<Panel>().SetSuccess(true);
        }
    }

    public void OnControlHold() // Runs on every frame the button is held
    {
    }

    public void OnControlUp() // Runs on the frame the key is released. Should happen only once per press
    {
    }

    public void OnTimeUp() // Runs when the minigame is out of time. This function is typically for animations if any (like, a baby crying when the milk isn't finished or something idk)
    {
    }

    public void MiniUpdate() // Basically the Update() function but for these panels
    {
        balloon.transform.position = balloonPos;
    }
}
