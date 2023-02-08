using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelTest : IPanelStrategy
{
    GameObject myPanel, myDisplay;

    private GameObject testObj;
    int clickCount;

    public void ResetMinigame(GameObject panelParent, GameObject displayParent)
    {
        myPanel = panelParent;
        myDisplay = displayParent;

        if (testObj == null)
		{
            testObj = GameObject.Instantiate(Resources.Load("TestObj"), new Vector3(Random.Range(-0.7f, 0.7f), Random.Range(-0.7f, 0.7f), 1) + myDisplay.transform.position, Quaternion.identity, myDisplay.transform) as GameObject;

            //GetComponent();
            clickCount = 10;
        }
    }

    public void OnControlDown()
	{
		clickCount--;
        if (clickCount <= 0)
        {
            myPanel.GetComponent<Panel>().SetSuccess(true);
        }
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
