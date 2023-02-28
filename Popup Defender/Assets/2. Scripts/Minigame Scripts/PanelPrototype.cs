using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelPrototype : IPanelStrategy
{
    GameObject myPanel, myDisplay;
    private GameObject testObj;

    public Vector2 SetPanelSize() => new Vector2(1f, 1f);
    public int ObjectiveKeyTech() => 0;
    public string ObjectiveDesc() => "ligma";

    public void ResetMinigame(GameObject panelParent, GameObject displayParent)
    {
        myPanel = panelParent;
        myDisplay = displayParent;

        if (testObj == null)
		{
            Debug.LogWarning("cock");
            testObj = GameObject.Instantiate(Resources.Load("TestObj"), new Vector3(Random.Range(-0.7f, 0.7f), Random.Range(-0.7f, 0.7f), 1) + myDisplay.transform.position, Quaternion.identity, myDisplay.transform) as GameObject;
        }

    }

    public void OnControlDown()
    {
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
    }
}
