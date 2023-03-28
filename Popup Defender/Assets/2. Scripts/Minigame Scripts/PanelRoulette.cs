using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelRoulette: IPanelStrategy // DO NOT EDIT THIS TEMPLATE - Copy and paste it when making new minigames, and remember to make the class name and file name the same
{
    // MAKE SURE THESE VARIABLES ARE IN
    GameObject myPanel, myDisplay;
    float sizeX, sizeY;

    // Extra variables go under here
    private GameObject wheel, arrow, audio;
    private float rotateSpeedInit = 5f, rotateSpeed;
    private int finalAngle, isFlipped;

    public Vector2 SetPanelSize() => new Vector2(sizeX, sizeY); // You shouldn't need to change anything in this line, refer to line 22 on changing the panel's size
    public string SetPanelBG() => "sprBG_roulette";
    public int ObjectiveKeyTech() => 1;  // 0 for Press, 1 for Tap, 2 for Hold, 3 for Spam
    public string ObjectiveDesc() => "to STOP SPINNING"; // tell the player what pressing the key does

    public void ResetMinigame(GameObject panelParent, GameObject displayParent)
    {
        myPanel = panelParent; myDisplay = displayParent;
        sizeX = 1.5f; sizeY = 1.5f; // You can do a lot with the size of panels and even vary it up like so

        // From here, you can do whatever you need to do here
        // When spawning objects, because this script does not inherit MonoBehaviour, you MUST use GameObject.Instantiate() as GameObject - it just is, sorry about that
        // Example here:
        if (wheel == null) // load the wheel
        {
            wheel = GameObject.Instantiate(Resources.Load("Wheel"), Vector3.zero + myDisplay.transform.position, Quaternion.identity, myDisplay.transform) as GameObject;
        }
        if (arrow == null) // load the arrow
        {
            arrow = GameObject.Instantiate(Resources.Load("Arrow"), Vector3.zero + myDisplay.transform.position, Quaternion.identity, myDisplay.transform) as GameObject;
        }

        if (audio == null)
        {
            audio = GameObject.Find("AudioManager");
        }

        isFlipped = Random.Range(0, 2);
        if (isFlipped == 1)
		{
            wheel.transform.rotation = Quaternion.Euler(0, 0, 90);
		}
        rotateSpeedInit = Random.Range(3f, 4f);
        RouletteSpinSound();
    }

    public void OnControlDown() // Runs on the frame the key is pressed. Should happen only once per press
    {
        //stop arrow
        arrow.transform.rotation = arrow.transform.rotation;
        rotateSpeed = 0;

        //angle stuff here
        //  2  |  1 
        // - - + - -
        //  3  |  4 
        if ((0f < finalAngle && finalAngle < 90f) || (180f < finalAngle && finalAngle < 270f)) // if in quadrants 2 or 4
        {
            if (isFlipped == 1) Shit();
            else Gottem();
        }
        else if  ((90f <= finalAngle && finalAngle <= 180f) || (270f <= finalAngle && finalAngle <= 360f)) // if in quadrants 1 or 3
        {
            if (isFlipped == 1) Gottem();
            else Shit();
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
        StopRouletteSpinSound();
    }

    public void MiniUpdate() // Basically the Update() function but for these panels
    {
        arrow.transform.Rotate(0, 0, rotateSpeed);

        finalAngle = Mathf.RoundToInt(arrow.transform.eulerAngles.z);

        if (myPanel.GetComponent<Panel>().isObjectiveClear != true)
        {
            rotateSpeed = Mathf.Clamp(Mathf.Lerp(rotateSpeed, rotateSpeedInit, rotateSpeedInit * Time.deltaTime), 1f, 5f);
        }
    }

    private void Shit()
    {
        myPanel.GetComponent<Panel>().ForceTimeLeft(-1f, false, true);
        rotateSpeedInit = Mathf.Clamp(rotateSpeedInit - 1f, 1f, 14f); // leniency - slow down arrow per fail
        audio.GetComponent<AudioManager>().Play("Roulette Lose");
    }

    private void Gottem()
	{
        myPanel.GetComponent<Panel>().SetSuccess(true);
        StopRouletteSpinSound();
        audio.GetComponent<AudioManager>().Play("Roulette Win");
    }

    private void RouletteSpinSound()
    {
        int chosen = Random.Range(1, 4);

        if (chosen == 1)
        {
            audio.GetComponent<AudioManager>().Play("Roulette Spin");
        }
        else if (chosen == 2)
        {
            audio.GetComponent<AudioManager>().Play("Roulette Spin 2");
        }
        else if (chosen == 3)
        {
            audio.GetComponent<AudioManager>().Play("Roulette Spin 3");
        }
    }

    private void StopRouletteSpinSound()
    {
        audio.GetComponent<AudioManager>().Stop("Roulette Spin");
        audio.GetComponent<AudioManager>().Stop("Roulette Spin 2");
        audio.GetComponent<AudioManager>().Stop("Roulette Spin 3");
    }
}
