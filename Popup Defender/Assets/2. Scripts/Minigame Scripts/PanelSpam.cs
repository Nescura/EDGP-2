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
    public string SetPanelBG() => "sprBG_rpgForest";
    public int ObjectiveKeyTech() => 3;
    public string ObjectiveDesc() => "  to defeat the monster!";

    public void ResetMinigame(GameObject panelParent, GameObject displayParent)
    {
        myPanel = panelParent; myDisplay = displayParent;
        sizeX = 2f; sizeY = 2f;

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
        testObj.GetComponent<SpriteRenderer>().color = Color.red;
    }

    public void OnControlUp()
    {
    }

    public void OnTimeUp()
    {
    }

    public void MiniUpdate()
    {
        testObj.GetComponent<SpriteRenderer>().color = Color.white;
        testObj.GetComponentInChildren<TMPro.TextMeshPro>().text = clickCount.ToString();
    }
}
