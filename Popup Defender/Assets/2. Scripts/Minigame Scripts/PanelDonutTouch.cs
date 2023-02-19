using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelDonutTouch : IPanelStrategy
{
    // MAKE SURE THESE VARIABLES ARE IN
    GameObject myPanel, myDisplay;
    float sizeX, sizeY;

    // Extra variables go under here
    private GameObject bigOlButton;
    int shouldYouPress = 0; // 0 means NO
    string buttonWord = "cum", objective = "test";
    Color buttonColour = new Color (188f / 255f, 20f / 255f, 20f / 255f);
    bool youFuckedIt = false; // extra check

    string[] buttonWords = {"EXPLODE", "PRESS", "FREE\nMONEY", "MAIDENS", "SHUT\nDOWN", "DO NOT\nPRESS", "DOWNLOAD", "SHARE", "DELETE" };
    Color[] buttonColours = { new Color(20f / 255f, 20f / 255f, 20f / 255f), new Color(188f / 255f, 20f / 255f, 20f / 255f), new Color(188f / 255, 188f / 255f, 20f / 255f), new Color(20f / 255f, 188f / 255f, 20f / 255f), new Color(20f / 255f, 188f / 255f, 188f / 255f), new Color(20f / 255f, 20f / 255f, 188f / 255f), new Color(188f / 255f, 20f / 255f, 188f / 255f) };
    string[] noPress = {"Don't press the button!", "Press the button to fail!", "Do not press to close!" };
    string[] yesPress = {"Don't not press the button!", "Press the button to close!", "Do not press to fail!"}; // is this evil LMAO

    public Vector2 SetPanelSize() => new Vector2(sizeX, sizeY);
    public string ObjectiveDesc() => objective;

    public void ResetMinigame(GameObject panelParent, GameObject displayParent)
    {
        myPanel = panelParent; myDisplay = displayParent;
        sizeX = 1.3f; sizeY = 1.3f;

        if (bigOlButton == null)
		{
            bigOlButton = GameObject.Instantiate(Resources.Load("DoNotButton"), new Vector3(0, 0, 1) + myDisplay.transform.position, Quaternion.identity, myDisplay.transform) as GameObject;
        }

        shouldYouPress = Random.Range(0, 2); // Randomly set whether button is safe to press
        buttonColour = buttonColours[Random.Range(0, buttonColours.Length)];
        buttonWord = buttonWords[Random.Range(0, buttonWords.Length)];
        if (shouldYouPress == 0)
        {
            objective = noPress[Random.Range(0, noPress.Length)];
        }
        else
        {
            objective = yesPress[Random.Range(0, yesPress.Length)];
        }
    }

    public void OnControlDown()
	{
        if (shouldYouPress == 0)
        {
            youFuckedIt = true;
            myPanel.GetComponent<Panel>().ForceTimeLeft(0f, true);
        }
        else
        {
            myPanel.GetComponent<Panel>().SetSuccess(true);
        }
    }

    public void OnControlHold()
    {
    }

    public void OnControlUp()
    {
    }

    public void OnTimeUp()
    {
        if (shouldYouPress == 0 && !youFuckedIt)
        {
            myPanel.GetComponent<Panel>().SetSuccess(true);
        }
    }

    public void MiniUpdate()
    {
        bigOlButton.GetComponent<SpriteRenderer>().color = buttonColour;
        bigOlButton.GetComponentInChildren<TMPro.TextMeshPro>().text = buttonWord;
    }
}
