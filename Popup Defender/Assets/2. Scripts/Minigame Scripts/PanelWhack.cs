using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelWhack : IPanelStrategy
{
    // MAKE SURE THESE VARIABLES ARE IN
    GameObject myPanel, myDisplay;
    float sizeX, sizeY;

    // Extra variables go under here
    private GameObject hammer, mole, moleHide;
    int smackCount;
    float speed = 2f;
    bool isHeld = false, isSmacking = false;


    public Vector2 SetPanelSize() => new Vector2(sizeX, sizeY); // You shouldn't need to change anything in this line, refer to line 22 on changing the panel's size
    public string SetPanelBG() => "Ground";
    public int ObjectiveKeyTech() => 2;  // 0 for Press, 1 for Tap, 2 for Hold, 3 for Spam
    public string ObjectiveDesc() => "to lift hammer & release to whack"; // tell the player what pressing the key does

    public void ResetMinigame(GameObject panelParent, GameObject displayParent)
    {
        myPanel = panelParent; myDisplay = displayParent;
        sizeX = 1.5f; sizeY = 1.8f;

        // From here, you can do whatever you need to do here
        // When spawning objects, because this script does not inherit MonoBehaviour, you MUST use GameObject.Instantiate() as GameObject - it just is, sorry about that
        // Example here:
        if (hammer == null) // this line is mostly to account for object pooling later, please make you do this
        {
            hammer = GameObject.Instantiate(Resources.Load("Hammer"), new Vector3(-1.9f, 0.7f, 1f) + myDisplay.transform.position, Quaternion.identity, myDisplay.transform) as GameObject;
        }

        if (mole == null) // this line is mostly to account for object pooling later, please make you do this
        {
            mole = GameObject.Instantiate(Resources.Load("Mole"), new Vector3(0f, -0.4f, 1f) + myDisplay.transform.position, Quaternion.identity, myDisplay.transform) as GameObject;
        }

        if (moleHide == null) // this line is mostly to account for object pooling later, please make you do this
        {
            moleHide = GameObject.Instantiate(Resources.Load("MoleHide"), new Vector3(0f, -2.55f, 1f) + myDisplay.transform.position, Quaternion.identity, myDisplay.transform) as GameObject;
        }
    }

    public void OnControlDown() // Runs on the frame the key is pressed. Should happen only once per press
    {
        
    }

    public void OnControlHold() // Runs on every frame the button is held
    {
        isHeld = true;
        
        if(hammer.transform.eulerAngles.z < 60f)
        {
            hammer.transform.Rotate(0f, 0f, Time.deltaTime * 100f);
        }
    }

    public void OnControlUp() // Runs on the frame the key is released. Should happen only once per press
    {
        isHeld = false;

        // If fully held, it will smack
        if (hammer.transform.eulerAngles.z >= 60f)
        {
            hammer.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            isSmacking = true;
        }
        // Otherwise, it will reset
        else
        {
            hammer.transform.rotation = Quaternion.Euler(0f, 0f, 20f);
        }
    }

    public void OnTimeUp() // Runs when the minigame is out of time. This function is typically for animations if any (like, a baby crying when the milk isn't finished or something idk)
    {

    }

    public void MiniUpdate() // Basically the Update() function but for these panels
    {
        // Change sprites based on smackCount
        mole.GetComponent<SpriteRenderer>().sprite = mole.GetComponent<MoleSprites>().sprites[Mathf.Clamp(smackCount, 0, 3)];

        // Peeka
        if(mole.transform.localPosition.y >= -0.2f && speed > 0)
        {
            speed *= -1;
        }

        // Boo
        if (mole.transform.localPosition.y <= -0.8f & speed < 0)
        {
            speed *= -1;
        }

        // PeekaBoo!
        mole.transform.position += new Vector3(0, Time.deltaTime * speed, 0);

        // isSmacking check helps with collider stuff not detecting
        if (isSmacking)
        {
            // 
            if (hammer.GetComponent<Collider2D>().IsTouching(mole.GetComponent<Collider2D>()))
            {
                smackCount++;
                isSmacking = false;
                myPanel.GetComponent<Panel>().ForceTimeLeft(2, false, true);
            }
        }

        // Win Condition
        if (smackCount >= 3)
        {
            myPanel.GetComponent<Panel>().SetSuccess(true);
        }

        // Resets hammer angle upon releasing once held/tapped button
        if (isHeld == false && hammer.transform.eulerAngles.z < 20f)
        {
            hammer.transform.Rotate(0, 0, Time.deltaTime * 50f);
        }
    }
}
