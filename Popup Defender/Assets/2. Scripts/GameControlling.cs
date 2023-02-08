using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControlling : MonoBehaviour
{
    // get NEW inputmanager in this class

    public Camera mainCamera;
    public GameObject panelPrefab;

    public static int layerAppend = 0;

    float windowSpawner = 0;

    public int popupCounter = 0;

    // Singleton
    public static GameControlling itsMe;
    public static GameControlling GetInstance()
    {
        if (itsMe == null)
        {
            itsMe = new GameControlling();
        }
        return itsMe;
    }


    // Start is called before the first frame update
    void Start()
    {
        // Populate the alphabet
        InputManager.GetInstance().PopulateList();
    }

    // Update is called once per frame
    void Update()
    {
        windowSpawner -= Time.deltaTime;
        //spawn Panel (debug)
        if (windowSpawner <= 0)
        {
            SpawnPanel();
            windowSpawner = 5;
        }
    }

    public IPanelStrategy GetRandomMinigame()
	{
        int index = Random.Range(0, 1);
        switch (index)
        {
            // ADD ALL YOUR PANEL MINIGAMES HERE!!!
            case 0:
                return new PanelTest();
            default:
                return new PanelTest();
		}

        // I'M SURE THERE'S A BETTER WAY TO DO THIS BUT...
        // Initially I put them all in a list and randomly get an index, but doing so means that all interactions will react with the SAME minigame
        // will clean up code if a better method is found
    }

    public void SpawnPanel()
	{
        popupCounter += 1;
        
        Debug.Log(InputManager.GetInstance().keyCodeList.Count);
        // Pull one random minigame to be spawned
        // Make sure you throw your script into the minigames List
        IPanelStrategy chosenPanelStrat = GetRandomMinigame();

        Vector2 viewportZero = mainCamera.ViewportToWorldPoint(Vector2.zero);
        Vector2 viewportOne  = mainCamera.ViewportToWorldPoint(Vector2.one);
        //Debug.Log(string.Format(("{0}, {1}"), viewportZero, viewportOne));

        GameObject newPanel = Instantiate(panelPrefab, new Vector2(Random.Range(viewportZero.x * 0.9f, viewportOne.x * 0.9f), Random.Range(viewportZero.y * 0.9f, viewportOne.y * 0.9f)), Quaternion.identity, this.transform);
        newPanel.GetComponent<Panel>().Initialize(chosenPanelStrat, Random.Range(0.8f, 2f), Random.Range(0.8f, 2f), 10f, InputManager.GetInstance().GenerateKey());
        newPanel.GetComponent<Panel>().LayerToFront(layerAppend += 50);
    }
}
