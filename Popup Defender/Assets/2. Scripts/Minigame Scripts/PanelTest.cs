using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelTest : IPanelStrategy
{
    // MAKE SURE THESE VARIABLES ARE IN
    GameObject myPanel, myDisplay;
    float sizeX, sizeY;

    // Extra variables go under here
    private GameObject testObj;
    int clickCount;

    public Vector2 SetPanelSize() => new Vector2(sizeX, sizeY);
    public string ObjectiveDesc() => "Hit until the counter hits zero!";

    public void ResetMinigame(GameObject panelParent, GameObject displayParent)
    {
        myPanel = panelParent; myDisplay = displayParent;
        sizeX = Random.Range(1f, 1.5f); sizeY = Random.Range(1f, 1.5f); // You can do a lot with the size of panels and even vary it up like so

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
    }

    public void MiniUpdate()
    {
        testObj.GetComponent<SpriteRenderer>().color = Color.red;
        testObj.GetComponentInChildren<TMPro.TextMeshPro>().text = clickCount.ToString();
    }
}
