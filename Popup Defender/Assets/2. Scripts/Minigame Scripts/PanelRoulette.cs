using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelRoulette: IPanelStrategy // DO NOT EDIT THIS TEMPLATE - Copy and paste it when making new minigames, and remember to make the class name and file name the same
{
    // MAKE SURE THESE VARIABLES ARE IN
    GameObject myPanel, myDisplay;
    float sizeX, sizeY;

    // Extra variables go under here
    private GameObject wheel, arrow;
    private float rotateSpeed = 15f;
    private int finalAngle;

    public Vector2 SetPanelSize() => new Vector2(sizeX, sizeY); // You shouldn't need to change anything in this line, refer to line 22 on changing the panel's size
    public int ObjectiveKeyTech() => 1;  // 0 for Press, 1 for Tap, 2 for Hold, 3 for Spam
    public string ObjectiveDesc() => "to STOP SPINNING"; // tell the player what pressing the key does

    public void ResetMinigame(GameObject panelParent, GameObject displayParent)
    {
        myPanel = panelParent; myDisplay = displayParent;
        sizeX = Random.Range(1.5f, 2f); sizeY = Random.Range(1.5f, 2f); // You can do a lot with the size of panels and even vary it up like so

        // From here, you can do whatever you need to do here
        // When spawning objects, because this script does not inherit MonoBehaviour, you MUST use GameObject.Instantiate() as GameObject - it just is, sorry about that
        // Example here:
        if (wheel == null) // load the wheel
        {
            wheel = GameObject.Instantiate(Resources.Load("Wheel"), new Vector3(Random.Range(-0.4f, 0.4f), Random.Range(-0.4f, 0.4f), 1) + myDisplay.transform.position, Quaternion.identity, myDisplay.transform) as GameObject;
        }
        if (arrow == null) // load the arrow
        {
            arrow = GameObject.Instantiate(Resources.Load("Arrow"), new Vector3(Random.Range(-0.4f, 0.4f), Random.Range(-0.4f, 0.4f), 1) + myDisplay.transform.position, Quaternion.identity, myDisplay.transform) as GameObject;
        }

        rotateSpeed = Random.Range(13f, 20f);
    }

    public void OnControlDown() // Runs on the frame the key is pressed. Should happen only once per press
    {
        //stop arrow
        //angle stuff here
        switch (finalAngle)
        {
            case 45:
                rotateSpeed = 0;
                arrow.transform.position = arrow.transform.position;
                myPanel.GetComponent<Panel>().ForceTimeLeft(-1, false);
                break;
            case 135:
                rotateSpeed = 0;
                arrow.transform.position = arrow.transform.position;
                myPanel.GetComponent<Panel>().SetSuccess(true);
                break;
            case 225:
                rotateSpeed = 0;
                arrow.transform.position = arrow.transform.position;
                myPanel.GetComponent<Panel>().ForceTimeLeft(-1, false);
                break;
            case 315:
                rotateSpeed = 0;
                arrow.transform.position = arrow.transform.position;
                myPanel.GetComponent<Panel>().SetSuccess(true);
                break;
        }
  
    }

    public void OnControlHold() // Runs on every frame the button is held
    {
       
    }

    public void OnControlUp() // Runs on the frame the key is released. Should happen only once per press
    {
        
    }

    public void OnTimeUp() // Runs when the minigame is out of time. This function is typically for animations if any (like, a baby crying when the milk isn't finished or something idk)
    {
    }

    public void MiniUpdate() // Basically the Update() function but for these panels
    {
        arrow.transform.Rotate(0, 0, rotateSpeed);
        if (Mathf.RoundToInt(arrow.transform.eulerAngles.z) % 90 == 0)
        {
            arrow.transform.Rotate(0, 0, 45f);
        }

        finalAngle = Mathf.RoundToInt(arrow.transform.eulerAngles.z);
    }

}
