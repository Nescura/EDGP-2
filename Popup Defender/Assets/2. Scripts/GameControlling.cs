using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControlling : MonoBehaviour
{
    // get NEW inputmanager in this class

    public Camera mainCamera;
    public GameObject panelPrefab;

    public static int layerAppend = 0;

    float spawnPanelTime = 0;

    //public int popupCounter = 0;

    public InputManager inputManager;

    // Singleton
    private static GameControlling itsMe;
    public static GameControlling GetInstance() => itsMe;
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
        spawnPanelTime -= Time.deltaTime;
        //spawn Panel (debug)
        if (spawnPanelTime <= 0 || Input.GetKeyDown(KeyCode.Backspace))
        {
            if (GetActivePanelCount() < 1) // spawn two more if there's not even a single one on screen
            {
                SpawnPanel();
                SpawnPanel();
            }
            SpawnPanel();

            spawnPanelTime = Random.Range(6f, 10f); // for playtest (09/02/2023), randomize intervals of time between panel spawns. Note that this will be a constant for each different level
        }
    }

    public IPanelStrategy GetRandomMinigame()
	{
        int index = Random.Range(0, 4);
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

        GameObject newPanel = Instantiate(panelPrefab, 
            new Vector2(Random.Range(viewportZero.x * 0.9f, viewportOne.x * 0.9f), 
            Random.Range(viewportZero.y * 0.9f, viewportOne.y * 0.9f)), 
            Quaternion.identity, this.transform);
        newPanel.GetComponent<Panel>().Initialize(chosenPanelStrat, 10f, inputManager.GenerateKey());
        newPanel.GetComponent<Panel>().LayerToFront(layerAppend += 50);
    }

    public int GetActivePanelCount()
    {
        Panel[] activePanelCount = FindObjectsOfType<Panel>();
        //Debug.Log(activePanelCount.Length);
        return activePanelCount.Length;
    }
}
