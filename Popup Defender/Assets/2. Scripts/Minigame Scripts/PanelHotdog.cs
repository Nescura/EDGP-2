using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelHotDog : IPanelStrategy
{
    // MAKE SURE THESE VARIABLES ARE IN
    GameObject myPanel, myDisplay;
    float sizeX, sizeY;

    // Extra variables go under here
    private GameObject hotdog, hotdogBun, audio;
    private float direction = 5f;

    public Vector2 SetPanelSize() => new Vector2(sizeX, sizeY);
    public string SetPanelBG() => "sprBG_HIYA";
    public int ObjectiveKeyTech() => 1;
    public string ObjectiveDesc() => "  to catch the dog!";

    public void ResetMinigame(GameObject panelParent, GameObject displayParent)
    {
        myPanel = panelParent; myDisplay = displayParent;
        sizeX = 2f; sizeY = 1.5f; // You can do a lot with the size of panels and even vary it up like so

        if (hotdogBun == null)
        {
            hotdogBun = GameObject.Instantiate(Resources.Load("HotDogBun"), new Vector3(0, 0, 1) + myDisplay.transform.position, Quaternion.identity, myDisplay.transform) as GameObject;
        }
        if (hotdog == null)
		{
            hotdog = GameObject.Instantiate(Resources.Load("HotDog"), new Vector3(-1, 0, 1) + myDisplay.transform.position, Quaternion.identity, myDisplay.transform) as GameObject;
        }

        if (audio == null)
        {
            audio = GameObject.Find("AudioManager");
        }

        PlaySound();

        direction = Random.Range(3f, 6f);
    }

    public void OnControlDown()
	{
        if (hotdog.GetComponent<Collider2D>().IsTouching(hotdogBun.GetComponent<Collider2D>()))
        {
            direction = 0;
            hotdog.transform.position = hotdogBun.transform.position;
            myPanel.GetComponent<Panel>().SetSuccess(true);
            StopSound();
        }
        else
		{
            myPanel.GetComponent<Panel>().ForceTimeLeft(-1, false, true);
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
        StopSound();   
    }

    void PlaySound()
    {
        int chosen = Random.Range(1, 4);

        if (chosen == 1)
        {
            audio.GetComponent<AudioManager>().Play("Hot Dog Theme 1");
        }
        else if (chosen == 2)
        {
            audio.GetComponent<AudioManager>().Play("Hot Dog Theme 2");
        }
        else if (chosen == 3)
        {
            audio.GetComponent<AudioManager>().Play("Hot Dog Theme 3");
        }
    }

    void StopSound()
    {
        audio.GetComponent<AudioManager>().Stop("Hot Dog Theme 1");
        audio.GetComponent<AudioManager>().Stop("Hot Dog Theme 2");
        audio.GetComponent<AudioManager>().Stop("Hot Dog Theme 3");
    }

    public void MiniUpdate()
    {
        if (hotdog.transform.localPosition.x >= 0.95 && direction > 0)   direction *= -1;
        if (hotdog.transform.localPosition.x <= -0.95 && direction < 0)   direction *= -1;

        hotdog.transform.position += new Vector3(Time.deltaTime * direction, 0, 0);
    }
}
