using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelSpam : IPanelStrategy
{
    // MAKE SURE THESE VARIABLES ARE IN
    GameObject myPanel, myDisplay;
    float sizeX, sizeY;

    // Extra variables go under here
    private GameObject mob, downloadBar, fillBar, slash;
    private GameObject audio;
    float enemyHP;

    public Vector2 SetPanelSize() => new Vector2(sizeX, sizeY);
    public string SetPanelBG() => "sprBG_rpgForest";
    public int ObjectiveKeyTech() => 3;
    public string ObjectiveDesc() => "  to defeat the monster!";

    public void ResetMinigame(GameObject panelParent, GameObject displayParent)
    {
        myPanel = panelParent; myDisplay = displayParent;
        sizeX = 1.6f; sizeY = 1.6f;

        if (mob == null)
		{
            mob = GameObject.Instantiate(Resources.Load("Monster"), new Vector3(0, 0f, 1) + myDisplay.transform.position, Quaternion.identity, myDisplay.transform) as GameObject;
        }

        if (downloadBar == null)
        {
            downloadBar = GameObject.Instantiate(Resources.Load("DownloadBar"), new Vector3(0, 1.8f, 1) + myDisplay.transform.position, Quaternion.identity, myDisplay.transform) as GameObject;
            fillBar = downloadBar.transform.Find("Bar").gameObject;

            downloadBar.transform.localScale = new Vector3(0.25f, 0.15f, 1f);
            fillBar.transform.Find("BarSpr").GetComponent<SpriteRenderer>().color = Color.red;
        }

        if (slash == null)
        {
            slash = GameObject.Instantiate(Resources.Load("Slash"), new Vector3(0, 0f, 1) + myDisplay.transform.position, Quaternion.identity, myDisplay.transform) as GameObject;
            slash.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
        }

        if (audio == null)
        {
            audio = GameObject.Find("AudioManager");
        }

        enemyHP = 10 + GameControlling.GetInstance().GetComponent<Levels>().currentLevel;
    }

    public void OnControlDown()
    {
        audio.GetComponent<AudioManager>().SetPitch("Slash", Random.Range(0.9f, 1.1f));
        audio.GetComponent<AudioManager>().Play("Slash");
        mob.GetComponent<SpriteRenderer>().color = Color.red;

        slash.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
        slash.transform.Rotate(0, 0, Random.Range(0f, 180f));
        slash.GetComponent<SpriteRenderer>().color = Color.white;

        if (enemyHP > 0) enemyHP -= 0.9f;
        if (enemyHP <= 0) myPanel.GetComponent<Panel>().SetSuccess(true);
    }

    public void OnControlHold()
    {
    }

    public void OnControlUp()
    {
    }

    public void OnTimeUp()
    {
    }

    public void MiniUpdate()
    {
        if (slash.transform.localScale.x < 1f) slash.transform.localScale += new Vector3(Time.deltaTime * 10f, 0f, 0f);
        slash.GetComponent<SpriteRenderer>().color = Color.Lerp(slash.GetComponent<SpriteRenderer>().color, new Color(1f, 1f, 1f, 0f), Time.deltaTime * 10);

        mob.GetComponent<SpriteRenderer>().color = Color.Lerp(mob.GetComponent<SpriteRenderer>().color, Color.white, Time.deltaTime * 10);
        fillBar.transform.localScale = new Vector3(enemyHP / 10f, 1, 1);
    }
}
