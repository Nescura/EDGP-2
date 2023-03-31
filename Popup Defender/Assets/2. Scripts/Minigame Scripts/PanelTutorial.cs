using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelTutorial : IPanelStrategy // DO NOT EDIT THIS TEMPLATE - Copy and paste it when making new minigames, and remember to make the class name and file name the same
{
    // MAKE SURE THESE VARIABLES ARE IN
    GameObject myPanel, myDisplay;
    float sizeX, sizeY;

    // Extra variables go under here
    private GameObject notepad;
    TMPro.TextMeshPro words;
    int clickCount;

    public Vector2 SetPanelSize() => new Vector2(sizeX, sizeY); // You shouldn't need to change anything in this line, refer to line 22 on changing the panel's size
    public string SetPanelBG() => "sprBG_white";
    public int ObjectiveKeyTech() => 1;  // 0 for Press, 1 for Tap, 2 for Hold, 3 for Spam
    public string ObjectiveDesc() => "to advance"; // tell the player what pressing the key does

    public void ResetMinigame(GameObject panelParent, GameObject displayParent)
    {
        myPanel = panelParent; myDisplay = displayParent;
        sizeX = 1.9f; sizeY = 2.3f; // You can do a lot with the size of panels and even vary it up like so

        // From here, you can do whatever you need to do here
        // When spawning objects, because this script does not inherit MonoBehaviour, you MUST use GameObject.Instantiate() as GameObject - it just is, sorry about that
        // Example here:
        if (notepad == null) // this line is mostly to account for object pooling later, please make you do this
		{
            notepad = GameObject.Instantiate(Resources.Load("Notepad"), new Vector3(0f, 0f, 1) + myDisplay.transform.position, Quaternion.identity, myDisplay.transform) as GameObject;
            words = notepad.GetComponent<TMPro.TextMeshPro>();
        }

        clickCount = 3;
    }

    public void OnControlDown() // Runs on the frame the key is pressed. Should happen only once per press
    {
        if (clickCount > 0) clickCount--;
        if (clickCount <= 0) myPanel.GetComponent<Panel>().SetSuccess(true);
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
        myPanel.GetComponent<Panel>().ForceTimeLeft(10f, true, false);
        GameControlling.GetInstance().GetComponent<GameTimer>().virusTimer += Time.deltaTime * 0.9f;

        switch (clickCount)
        {
            case 3:
                words.text = "Thank you for installing the OMEGA MINIGAME FRAGMENTED GENERATION VIRUS v1.10.33\n"
                           + "\n"
                           + "Looks like your lil bro has installed one of our favourite viruses!\n"
                           + "\n"
                           + "Your so screwed now. As soon as your pc crashes, we get ALL your data.\n"
                           + "\n"
                           + "resistance is futile. But we let you have a bit of fun :)\n"
                           + "\n"
                           + "Follow the instructions:\n";
                break;
            case 2:
                words.text = "Good! You can read!\n"
                           + "\n"
                           + "hope u like weird ads because thats what this virus will do\n"
                           + "\n"
                           + "There will be A LOT of them so you better prepare\n"
                           + "\n"
                           + "You have multiple fingers right? Youll need them fyi.\n"
                           + "\n"
                           + "\n"
                           + "\n"
                           + "Go on, follow the instructions:\n";
                break;
            case 1:
                words.text = "Oh yeah looking at your schedule btw\n"
                           + "\n"
                           + "Wow you have assignment submissions every day??? damn thats rough buddy\n"
                           + "\n"
                           + "you should really get to doing that huh :)))\n"
                           + "\n"
                           + "because u need it, every minigame you clear we give u extra time to submit ok?\n"
                           + "\n"
                           + "good luck, toodaloo loser!!!";
                break;
        }
    }
}
