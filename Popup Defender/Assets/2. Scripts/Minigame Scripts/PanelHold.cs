using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelHold : IPanelStrategy
{
    // MAKE SURE THESE VARIABLES ARE IN
    GameObject myPanel, myDisplay;
    float sizeX, sizeY;

    // Extra variables go under here
    private GameObject downloadBar, fillBar, audio;
    float progress;
    bool played;
    bool timeup;
    float chosen;

    public Vector2 SetPanelSize() => new Vector2(sizeX, sizeY);
    public string SetPanelBG() => "sprBG_download";
    public int ObjectiveKeyTech() => 2;
    public string ObjectiveDesc() => "to finish the download!";

    public void ResetMinigame(GameObject panelParent, GameObject displayParent)
    {
        myPanel = panelParent; myDisplay = displayParent;
        sizeX = 2f; sizeY = 1f;

        if (downloadBar == null)
		{
            downloadBar = GameObject.Instantiate(Resources.Load("DownloadBar"), new Vector3(0, 0.25f, 1) + myDisplay.transform.position, Quaternion.identity, myDisplay.transform) as GameObject;
            fillBar = downloadBar.transform.Find("Bar").gameObject;
        }

        if (audio == null)
        {
            audio = GameObject.Find("AudioManager");
        }

        chosen = Random.Range(1, 4);
    }

    public void OnControlDown()
	{
    }

    public void OnControlHold()
    {
        if (progress <= 15f)
        {
            progress += Time.deltaTime * 3f;
            myPanel.GetComponent<Panel>().ForceTimeLeft(Time.deltaTime / 2, false, true);
        }
        else
        {
            myPanel.GetComponent<Panel>().SetSuccess(true);
            StopSound();
            timeup = true;
        }

        if (played == false && timeup == false)
        {
            PlaySound();
            played = true;
        }
        else if (played == true && timeup == false)
        {
            UnpauseSound();
        }
    }

    public void OnControlUp()
    {
        PauseSound();
    }

    public void OnTimeUp()
    {
        StopSound();
        timeup = true;
    }

    public void MiniUpdate()
    {
        fillBar.transform.localScale = new Vector3(progress / 15f, 1, 1);
    }

    private void PlaySound()
    {
        if (chosen == 1)
        {
            audio.GetComponent<AudioManager>().Play("Download 1");
        }
        else if (chosen == 2)
        {
            audio.GetComponent<AudioManager>().Play("Download 2");
        }
        else if (chosen == 3)
        {
            audio.GetComponent<AudioManager>().Play("Download 3");
        }
    }

    private void PauseSound()
    {
        if (chosen == 1)
        {
            audio.GetComponent<AudioManager>().Pause("Download 1");
        }
        else if (chosen == 2)
        {
            audio.GetComponent<AudioManager>().Pause("Download 2");
        }
        else if (chosen == 3)
        {
            audio.GetComponent<AudioManager>().Pause("Download 3");
        }
    }

    private void UnpauseSound()
    {
        if (chosen == 1)
        {
            audio.GetComponent<AudioManager>().Unpause("Download 1");
        }
        else if (chosen == 2)
        {
            audio.GetComponent<AudioManager>().Unpause("Download 2");
        }
        else if (chosen == 3)
        {
            audio.GetComponent<AudioManager>().Unpause("Download 3");
        }
    }

    private void StopSound()
    {
        if (chosen == 1)
        {
            audio.GetComponent<AudioManager>().Stop("Download 1");
        }
        else if (chosen == 2)
        {
            audio.GetComponent<AudioManager>().Stop("Download 2");
        }
        else if (chosen == 3)
        {
            audio.GetComponent<AudioManager>().Stop("Download 3");
        }
    }
}
