using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameControlling : MonoBehaviour
{
    #region Variables
    // get NEW inputmanager in this class
    public Camera mainCamera;
    public GameObject panelPrefab;

    public static int layerAppend = 0;

    public int spawnMax = 1; // max amt of pop-ups that can be on screen
    public float spawnPanelTimeAvg = 8f, spawnPanelTime = 0;
    public float minigameTimer = 10f;

    //public int popupCounter = 0;

    public InputManager inputManager;

    // Singleton
    private static GameControlling itsMe;
    public static GameControlling GetInstance() => itsMe;

    //End Screen Stuff
    [Header("End Screen Stuff")]
    public GameObject restartBttn;
    public GameObject mainMenuBttn;
    public GameObject blueScreen;

    public float playGlitch;
    public float BlinkTime;
    public string textToBlink;
    public TMP_Text textComponent;
    public bool blinked = false;
    public bool activateBlinking;
    public float blinkTimer;
    public float timeToDisplay;
    #endregion

    private void Awake()
	{
        // if another GameControlling already exist, kill myself as I am not needed :c
        if (itsMe != null)
        {
            Destroy(gameObject);
        }
        
        // if the GameControlling haven't existed, this instance becomes the OG
        else
        {
            itsMe = this;
            // DontDestroyOnLoad(gameObject); // This line makes it so that the GameControlling persists between scenes - disable if not needed 
        }

        //Set the Text that will be blinked from the inspector of the text component
        textToBlink = textComponent.text;
        blinkTimer = BlinkTime;
    }

	// Start is called before the first frame update
	void Start()
    {
        // Make a new Input Manager for this game controller singleton
        inputManager = new InputManager();

        // Populate the alphabet
        inputManager.PopulateList();
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<GameState>().state != GameCurrentState.PAUSED)
        {
            #region Spawn Panel

            // On game over, spam screen with panels
            if (GetComponent<GameState>().state == GameCurrentState.END)
            {
                spawnMax = 24;

                if(GetActivePanelCount() < spawnMax)
                {
                    if (spawnPanelTime <= 0)
                    {
                        SpawnPanel();
                        spawnPanelTime = 1.5f;
                    }
                    else
                    {
                        spawnPanelTime -= Time.deltaTime * 10;
                    }
                }
                else
                {
                    StartBSOD();
                }
            }

            // Actual game spawning for panels
            else if(GetComponent<GameState>().state == GameCurrentState.START)
            {
                spawnPanelTime -= Time.deltaTime;

                // When it's time to spawn a panel, spawn one
                if (spawnPanelTime <= 0)
                {
                    if (GetActivePanelCount() < spawnMax)
                    {
                        int amtToSpawn = spawnMax - GetActivePanelCount();
                        int randomAmt = Random.Range(1, amtToSpawn + 1);

                        for (int i = 0; i < randomAmt; i++)
                        {
                            SpawnPanel();
                        }
                    }

                    spawnPanelTime = Random.Range(spawnPanelTimeAvg * 0.8f, spawnPanelTimeAvg * 1.2f); // randomize intervals of time between panel spawns based on spawnPanelTimeAvg. Decreases every odd level, increasing frequency of panel spawnws
                }
            }
            #endregion

            #region Debugging Codes
            if (Input.GetKeyDown(KeyCode.Insert) && GetActivePanelCount() < 24) // FOR DEBUGGING PURPOSES - spawn a panel regardless of time or max
			{
                SpawnPanel(); 
                spawnPanelTime = Random.Range(spawnPanelTimeAvg * 0.8f, spawnPanelTimeAvg * 1.2f);
            }

            if (Input.GetKeyDown(KeyCode.Delete)) // FOR DEBUGGING PURPOSES - clear all panels and give 20s of breathing room
            {
                Panel[] panelInstances = FindObjectsOfType<Panel>();
                foreach (Panel thisPan in panelInstances)
                {
                    thisPan.isObjectiveClear = true;
                    thisPan.ForceTimeLeft(0, true, true);
                }
                spawnPanelTime = 20f;
            }

            if (Input.GetKeyDown(KeyCode.End))
            {
                GetComponent<GameState>().state = GameCurrentState.END;
            }
            #endregion
        }
    }

    public IPanelStrategy GetRandomMinigame()
	{
        int index = Random.Range(0, 7); // REMEMBER, INTEGER RANDOM RANGE IS MAX EXCLUSIVE - e.g. if there are 5 cases, make sure the max range is 6!
        switch (index)
        {
            // ADD ALL YOUR PANEL MINIGAMES HERE!!!
            case 0:
                return new PanelSpam();
            case 1:
                return new PanelDonutTouch();
            case 2:
                return new PanelHold();
            case 3:
                return new PanelHotDog();
            case 4:
                return new PanelRoulette();
            case 5:
                return new PanelWhack();
            case 6:
                return new PanelTowerBuilder();
            default:
                return new PanelTemplate();
		}

        // I'M SURE THERE'S A BETTER WAY TO DO THIS BUT...
        // Initially I put them all in a list and randomly get an index, but doing so means that all interactions will react with the SAME minigame
        // will clean up code if a better method is found
    }

    public void SpawnPanel()
	{
        //popupCounter += 1;
        
        //Debug.Log(inputManager.keyCodeList.Count);
        // Pull one random minigame to be spawned
        // Make sure you throw your script into the minigames List
        IPanelStrategy chosenPanelStrat = GetRandomMinigame();

        Vector2 viewportZero = mainCamera.ViewportToWorldPoint(Vector2.zero);
        Vector2 viewportOne  = mainCamera.ViewportToWorldPoint(Vector2.one);
        //Debug.Log(string.Format(("{0}, {1}"), viewportZero, viewportOne));
        FindObjectOfType<AudioManager>().Play("PopUp");
        GameObject newPanel = Instantiate(panelPrefab, 
            new Vector3(Random.Range(viewportZero.x * 0.9f, viewportOne.x * 0.9f), 
            Random.Range(viewportZero.y * 0.9f, viewportOne.y * 0.9f), -5), 
            Quaternion.identity, this.transform);
        newPanel.GetComponent<Panel>().Initialize(chosenPanelStrat, minigameTimer, inputManager.GenerateKey());
        newPanel.GetComponent<Panel>().LayerToFront(layerAppend += 50);
        if (GetComponent<Levels>().currentLevel >= 10)
        {
            newPanel.GetComponent<Panel>().SetSpeed(Random.Range(GetComponent<Levels>().currentLevel / -20f, GetComponent<Levels>().currentLevel / 20f), Random.Range(GetComponent<Levels>().currentLevel / -20f, GetComponent<Levels>().currentLevel / 20f));
        }
    }

    public void SpawnMaxPanels()
    {
        for (int x = 0; x < spawnMax; x++)
        {
            SpawnPanel();
        }
    }

    public int GetActivePanelCount()
    {
        int unclearedPanels = 0;
        Panel[] panelInstances = FindObjectsOfType<Panel>();
        foreach(Panel thisPan in panelInstances)
		{
            if (thisPan.isObjectiveClear == false) unclearedPanels += 1;
		}
        //Debug.Log(activePanelCount.Length);
        //Debug.Log(unclearedPanels);
        return unclearedPanels;
    }

    public void StartBSOD()
    {
        playGlitch -= Time.deltaTime;
        GameObject.Find("Main Camera").GetComponent<GlitchEffect>().glitch = true;

        if (playGlitch <= 0)
        {
            FindObjectOfType<AudioManager>().Stop("Glitch");

            blueScreen.SetActive(true);

            if (activateBlinking == true)
            {
                #region Blinking Text Function
                if (blinked == false)
                {
                    blinkTimer -= Time.deltaTime;

                    if (blinkTimer <= 0)
                    {
                        textComponent.text = string.Empty;
                        blinked = true;
                    }
                }
                else if (blinked == true)
                {
                    blinkTimer += Time.deltaTime;

                    if (blinkTimer >= BlinkTime)
                    {
                        textComponent.text = textToBlink;
                        blinked = false;
                    }
                }
                #endregion
            }
        }
        else
        {
            FindObjectOfType<AudioManager>().StopAll();
            FindObjectOfType<AudioManager>().Play("Glitch");
        }

        #region Display Text
        if (timeToDisplay >= 0)
        {
            timeToDisplay -= Time.deltaTime;
        }
        else
        {
            activateBlinking = false;
            textComponent.text = textToBlink;
            textToBlink = "*** STOP; 0x00000050 (0x8872A990, 0x00x00000001, 0x804F35D7, 0x00000000) \n" +
                          "\n" +
                          "A problem has been detected and Doors has been shut down to prevent damage to your computer. \n" +
                          "\n" +
                          "I_GOT_YOUR_COMPUTER_NOW" +
                          "\n" +
                          "If this is your first time you've seen this Stop error screen. \n" +
                          "Follow these steps:\n" +
                          " - To reboot system, press any key. \n" +
                          " - If the system does not reboot, proceed to bang your head on the keyboard and scream like a little girl. \n" +
                          " - To contact the peeps who stole your computer... nope. Sucks to suck bye.\n" +
                          "\n" +
                          "Technical information: \n" +
                          "\n" +
                          "*** STOP; 0x00000050 (0x8872A990, 0x00x00000001, 0x804F35D7, 0x00000000) \n" +
                          "\n" +
                          "Beginning extraction of physical memory... \n" +
                          "Extraction of physical memory complete. \n" +
                          "Thanks for giving us your computer :D \n" + 
                          "\n" + "Press Any Key To Continue";

            if (Input.anyKeyDown)
            {
                SceneManager.LoadScene(0);
            }
        }
        #endregion
    }
}
