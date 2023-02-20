using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelHold : IPanelStrategy
{
    // MAKE SURE THESE VARIABLES ARE IN
    GameObject myPanel, myDisplay;
    float sizeX, sizeY;

    // Extra variables go under here
    private GameObject downloadBar, fillBar;
    float progress;

    public Vector2 SetPanelSize() => new Vector2(sizeX, sizeY);
    public string ObjectiveDesc() => "Hold to download!";

    public void ResetMinigame(GameObject panelParent, GameObject displayParent)
    {
        myPanel = panelParent; myDisplay = displayParent;
        sizeX = 2f; sizeY = 1f;

        if (downloadBar == null)
		{
            downloadBar = GameObject.Instantiate(Resources.Load("DownloadBar"), new Vector3(0, 0.25f, 1) + myDisplay.transform.position, Quaternion.identity, myDisplay.transform) as GameObject;
            fillBar = downloadBar.transform.Find("Bar").gameObject;
        }
    }

    public void OnControlDown()
	{
    }

    public void OnControlHold()
    {
        if (progress <= 15f)
        {
            progress += Time.deltaTime * 3f;
            myPanel.GetComponent<Panel>().ForceTimeLeft(Time.deltaTime / 2, false);
        }
        else
		{
            myPanel.GetComponent<Panel>().SetSuccess(true);
        }
    }

    public void OnControlUp()
    {
    }

    public void OnTimeUp()
    {
        GameObject myController = GameObject.Find("GameCuntroller");
        GameTimer myTimerScript = myController.GetComponent<GameTimer>();

        myTimerScript.deadlineTimer += 1;
    }

    public void MiniUpdate()
    {
        fillBar.transform.localScale = new Vector3(progress / 15f, 1, 1);
    }
}
