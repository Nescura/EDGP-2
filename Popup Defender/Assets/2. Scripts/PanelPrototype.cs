using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelPrototype : MonoBehaviour, IPanelStrategy
{
    private GameObject testObj;

    public void ResetMinigame(GameObject parent)
    {
        if (testObj == null)
		{
            Debug.LogWarning("cock");
            testObj = Instantiate(Resources.Load("TestObj"), new Vector3(Random.Range(-0.7f, 0.7f), Random.Range(-0.7f, 0.7f), 1) + parent.transform.position, Quaternion.identity, parent.transform) as GameObject;
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
