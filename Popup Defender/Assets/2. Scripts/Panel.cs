using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore;

public class Panel : MonoBehaviour
{
    // Initialization Variables
    private bool isObjectiveClear = false;
    public float panelSizeX = 1.125f, panelSizeY = 1.2f, timeLeft = 10f, difficulty = 1f;
    private KeyCode assignedKey;
    public IPanelStrategy panelStrat;

    // Display Stuff
    public GameObject thisMask;
    public TextMesh thisKeyMesh, thisTimeMesh;

    public void Initialize(IPanelStrategy panelStrategy, float sizeX, float sizeY, float timeInSecs, KeyCode key)
    {
        // Get a random input from the input list
        assignedKey = key;

        // Setting strategies
        this.panelStrat = panelStrategy;
        panelSizeX = sizeX; panelSizeY = sizeY; timeLeft = timeInSecs;

        // Get the mask in child
        thisMask = transform.Find("PanelMask").gameObject;

        // Set size of panel based on inputs
        GetComponent<RectTransform>().sizeDelta = new Vector2(panelSizeX, panelSizeY);
        GetComponent<SpriteRenderer>().size = new Vector2(panelSizeX, panelSizeY);
        thisMask.GetComponent<SpriteRenderer>().size = new Vector2(panelSizeX, panelSizeY);

        // Get the key display thing and display it
        thisKeyMesh = transform.Find("KeyBG").transform.Find("KeyTxt").GetComponent<TextMesh>();
        thisKeyMesh.text = assignedKey.ToString();

        // Get the time display thing
        thisTimeMesh = transform.Find("TimeBG").transform.Find("TimeTxt").GetComponent<TextMesh>();
    }

    public void SetSuccess(bool b)
	{
        isObjectiveClear = b;
	}

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Timer Handling
        if (timeLeft > 0)
		{
            timeLeft -= Time.deltaTime;
            thisTimeMesh.text = string.Format("{00}", Mathf.FloorToInt(timeLeft));
        }
        else
		{
            panelStrat.OnTimeUp();
            InputManager.GetInstance().ReturnKey(assignedKey);
            gameObject.SetActive(false);
        }

        // Input Handling
        if (Input.GetKeyDown(assignedKey))
		{
            panelStrat.OnControlDown();
		}
        if (Input.GetKey(assignedKey))
        {
            panelStrat.OnControlHold();
        }
        if (Input.GetKeyUp(assignedKey))
        {
            panelStrat.OnControlUp();
        }
    }
}

public interface IPanelStrategy
{
    void OnControlDown(); // On the frame the control is pressed
    void OnControlHold(); // Each frame when the control is held down
    void OnControlUp(); // On the frame the control is released
    void OnTimeUp(); // What to do when the timer is up
}
