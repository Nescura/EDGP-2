using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelSpam : IPanelStrategy
{
    // MAKE SURE THESE VARIABLES ARE IN
    GameObject myPanel, myDisplay;
    float sizeX, sizeY;

    // Extra variables go under here
    private GameObject testObj;
    int clickCount;

    public Vector2 SetPanelSize() => new Vector2(sizeX, sizeY);
    public int ObjectiveKeyTech() => 3;
    public string ObjectiveDesc() => "to defeat the monster!";

    public void ResetMinigame(GameObject panelParent, GameObject displayParent)
    {
        myPanel = panelParent; myDisplay = displayParent;
        sizeX = Random.Range(1.5f, 2f); sizeY = Random.Range(1.5f, 2f); // You can do a lot with the size of panels and even vary it up like so

        if (testObj == null)
		{
            testObj = GameObject.Instantiate(Resources.Load("TestObj"), new Vector3(Random.Range(-0.4f, 0.4f), Random.Range(-0.4f, 0.4f), 1) + myDisplay.transform.position, Quaternion.identity, myDisplay.transform) as GameObject;
        }

        clickCount = 10;
    }

    public void OnControlDown()
    {
        if (clickCount > 0) clickCount--;
        if (clickCount <= 0) myPanel.GetComponent<Panel>().SetSuccess(true);
    }

    public void OnControlHold()
    {
        testObj.GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void OnControlUp()
    {
    }

    public void OnTimeUp()
    {
        /*GameObject myController = GameObject.Find("GameCuntroller");
        GameTimer myTimerScript = myController.GetComponent<GameTimer>();

        myTimerScript.deadlineTimer += 1;
        
        Refer to PanelDonutTouch.cs at line 84 to see why this is commented out */
    }

    public void MiniUpdate()
    {
        testObj.GetComponent<SpriteRenderer>().color = Color.red;
        testObj.GetComponentInChildren<TMPro.TextMeshPro>().text = clickCount.ToString();
    }
}
