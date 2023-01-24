using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowContext : MonoBehaviour
{
    // Initialization Variables
    private GameObject thisMask;
    private KeyCode assignedControl;
    public float windowSizeX = 1.125f, windowSizeY = 1.2f, timeLeft = 10f, difficulty = 1f;
    public IWindowStrategy windowStrat;

    public void Initialize(float sizeX, float sizeY, IWindowStrategy windowStrategy, float timeInSecs, float currDifficulty)
    {
        // Setting variables based on inputs
        this.windowSizeX = sizeX; this.windowSizeY = sizeY;
        this.windowStrat = windowStrategy; this.timeLeft = timeInSecs; this.difficulty = currDifficulty;

        // Get the mask in child
        thisMask = transform.Find("WindowMask").gameObject;

        // Set size of window based on inputs
        GetComponent<SpriteRenderer>().size = new Vector2(windowSizeX, windowSizeY);

        // Get a random input from the input list
        assignedControl = InputManager.GetInstance().GenerateKey();
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
            windowStrat.MiniUpdate();
		}
        else
		{
            windowStrat.OnTimeUp();
		}

        // Input Handling
        if (Input.GetKeyDown(assignedControl))
		{
            windowStrat.OnControlDown();
		}
        if (Input.GetKey(assignedControl))
        {
            windowStrat.OnControlHold();
        }
        if (Input.GetKeyUp(assignedControl))
        {
            windowStrat.OnControlUp();
        }
    }
}

public interface IWindowStrategy
{
    void SetTime(float initTime); // Amount of time minigame remains active - note that you CAN make running the time up a success condition
    void SetControl(KeyCode assignedKey); // The key pulled from InputManager
    void MiniUpdate(); // Basically the update function to use for minigames
    void OnControlDown(); // On the frame the control is pressed
    void OnControlHold(); // Each frame when the control is held down
    void OnControlUp(); // On the frame the control is released
    void OnTimeUp(); // What to do when the timer is up
    void OnSucceed(); // Specific task for succeeding the minigame
    void OnFail(); // Special cases for failing the minigame
}
