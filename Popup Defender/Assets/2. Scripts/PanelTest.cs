using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelTest : MonoBehaviour, IPanelStrategy
{
    GameObject myDad;

    private GameObject testObj;
    int clickCount;

    public void ResetMinigame(GameObject parent)
    {
        myDad = parent;

        if (testObj == null)
		{
            Debug.LogWarning("cock");
            testObj = Instantiate(Resources.Load("TestObj"), new Vector3(Random.Range(-0.7f, 0.7f), Random.Range(-0.7f, 0.7f), 1) + parent.transform.position, Quaternion.identity, parent.transform) as GameObject;

            //GetComponent();
            clickCount = 0;
        }
    }

    public void OnControlDown()
    {
        clickCount++;
        if (clickCount >= 10)
        {
            myDad.GetComponent<Panel>().SetSuccess(true);
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
        testObj.GetComponentInChildren<TMPro.TextMeshPro>().text = "Count: " + clickCount;

    }
}
