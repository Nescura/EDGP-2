using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControlling : MonoBehaviour
{
    // get NEW inputmanager in this class

    public Camera mainCamera;
    public GameObject panelPrefab;

    public List<IPanelStrategy> minigames = new List<IPanelStrategy>();

    public int layerAppend = 0;

    // Start is called before the first frame update
    void Start()
    {
        InputManager.GetInstance().PopulateList();

        // ADD ALL YOUR PANEL MINIGAMES HERE!!!
        minigames.Add(new PanelTest());
    }

    // Update is called once per frame
    void Update()
    {
        //spawn Panel (debug)
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            SpawnPanel();
        }
    }

    public void SpawnPanel()
	{
        Debug.Log(InputManager.GetInstance().keyCodeList.Count);
        // Pull one random minigame to be spawned
        // Make sure you throw your script into the minigames List
        IPanelStrategy chosenPanelStrat = minigames[0/*Random.Range(0, minigames.Count + 1)*/];

        Vector2 viewportZero = mainCamera.ViewportToWorldPoint(Vector2.zero);
        Vector2 viewportOne  = mainCamera.ViewportToWorldPoint(Vector2.one);

        GameObject newPanel = Instantiate(panelPrefab, new Vector2(Random.Range(viewportZero.x, viewportOne.x), Random.Range(viewportZero.y, viewportOne.y)), Quaternion.identity, this.transform);
        newPanel.GetComponent<Panel>().Initialize(chosenPanelStrat, Random.Range(0.8f, 2f), Random.Range(0.8f, 2f), 10f, InputManager.GetInstance().GenerateKey());
        newPanel.GetComponent<Panel>().LayerToFront(layerAppend);
        layerAppend += 50;
    }
}
