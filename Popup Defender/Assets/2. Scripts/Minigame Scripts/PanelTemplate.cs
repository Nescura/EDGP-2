using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelTemplate : IPanelStrategy // DO NOT EDIT THIS TEMPLATE - Copy and paste it when making new minigames, and remember to make the class name and file name the same
{
    // MAKE SURE THESE VARIABLES ARE IN
    GameObject myPanel, myDisplay;
    float sizeX, sizeY;

    // Extra variables go under here
    private GameObject testObj;
    int clickCount;

    public Vector2 SetPanelSize() => new Vector2(sizeX, sizeY); // You shouldn't need to change anything in this line, refer to line 22 on changing the panel's size
    public string SetPanelBG() => "";
    public int ObjectiveKeyTech() => 0;  // 0 for Press, 1 for Tap, 2 for Hold, 3 for Spam
    public string ObjectiveDesc() => "to do something!"; // tell the player what pressing the key does

    public void ResetMinigame(GameObject panelParent, GameObject displayParent)
    {
        myPanel = panelParent; myDisplay = displayParent;
        sizeX = Random.Range(1.5f, 2f); sizeY = Random.Range(1.5f, 2f); // You can do a lot with the size of panels and even vary it up like so

        // From here, you can do whatever you need to do here
        // When spawning objects, because this script does not inherit MonoBehaviour, you MUST use GameObject.Instantiate() as GameObject - it just is, sorry about that
        // Example here:
        if (testObj == null) // this line is mostly to account for object pooling later, please make you do this
		{
            testObj = GameObject.Instantiate(Resources.Load("TestObj"), new Vector3(Random.Range(-0.4f, 0.4f), Random.Range(-0.4f, 0.4f), 1) + myDisplay.transform.position, Quaternion.identity, myDisplay.transform) as GameObject;
        }

        clickCount = 10;
    }

    public void OnControlDown() // Runs on the frame the key is pressed. Should happen only once per press
    {
        if (clickCount > 0) clickCount--;
        if (clickCount <= 0) myPanel.GetComponent<Panel>().SetSuccess(true);
    }

    public void OnControlHold() // Runs on every frame the button is held
    {
        testObj.GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void OnControlUp() // Runs on the frame the key is released. Should happen only once per press
    {
        testObj.GetComponent<SpriteRenderer>().color = Color.red;
    }

    public void OnTimeUp() // Runs when the minigame is out of time. This function is typically for animations if any (like, a baby crying when the milk isn't finished or something idk)
    {
    }

    public void MiniUpdate() // Basically the Update() function but for these panels
    {
        testObj.GetComponentInChildren<TMPro.TextMeshPro>().text = clickCount.ToString();
    }
}
