using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore;

public class Panel : MonoBehaviour
{
    // Initialization Variables
    public bool isObjectiveClear = false;
    public float panelSizeX = 1.125f, panelSizeY = 1.2f, timeLeft = 10f, difficulty = 1f;
    private KeyCode assignedKey;
    public IPanelStrategy panelStrat;
    public GameObject thisParent;

    // Display Stuff
    public GameObject thisMask;
    public TMPro.TextMeshPro thisKeyMesh, thisTimeMesh;

    // Panel Dragging Stuff
    public Vector3 mouseClickPosOffset;

    public void Initialize(IPanelStrategy panelStrategy, float sizeX, float sizeY, float timeInSecs, KeyCode key)
    {
        // Define parent for interface to spawn gameobjects in
        thisParent = transform.Find("DisplayGameObjs").gameObject;

        // Get a random input from the input list
        assignedKey = key;

        // Setting strategies
        this.panelStrat = panelStrategy;
        panelSizeX = sizeX; panelSizeY = sizeY; timeLeft = timeInSecs;

        // Get the mask in child
        thisMask = transform.Find("PanelMask").gameObject;

        // Set size of panel
        GetComponent<RectTransform>().sizeDelta = new Vector2(panelSizeX, panelSizeY);
        GetComponent<SpriteRenderer>().size = new Vector2(panelSizeX, panelSizeY);
        thisMask.GetComponent<SpriteRenderer>().size = new Vector2(panelSizeX, panelSizeY);
        GetComponent<BoxCollider2D>().size = new Vector2(panelSizeX, panelSizeY); // collider

        // Get the key display thing and display it
        thisKeyMesh = transform.Find("KeyBG").transform.Find("KeyTxt").GetComponent<TMPro.TextMeshPro>();
        thisKeyMesh.text = assignedKey.ToString();

        // Get the time display thing
        thisTimeMesh = transform.Find("TimeBG").transform.Find("TimeTxt").GetComponent<TMPro.TextMeshPro>();

        // LASTLY. Reset the minigame
        panelStrat.ResetMinigame(thisParent);
    }

    public void SetSuccess(bool b)
	{
        isObjectiveClear = b;
	}

    public void LayerToFront(int index)
	{
        // Border
        GetComponent<SpriteRenderer>().sortingOrder = index + 10; // extra numbers are their "deafult" values

        // Objects in Panel Minigame
        foreach (Transform child in transform.Find("DisplayGameObjs"))
		{
            child.TryGetComponent(out SpriteRenderer childSprRen);
            child.TryGetComponent(out TMPro.TextMeshPro childTMPro);
            if (childSprRen != null) childSprRen.sortingOrder = index - 5;
            if (childTMPro != null) childTMPro.sortingOrder = index - 5;

            foreach(Transform grandchild in child)
            {
                grandchild.TryGetComponent(out SpriteRenderer grandchildSprRen);
                grandchild.TryGetComponent(out TMPro.TextMeshPro grandchildTMPro);
                if (grandchildSprRen != null) grandchildSprRen.sortingOrder = index - 5;
                if (grandchildTMPro != null) grandchildTMPro.sortingOrder = index - 5;
            }
        }

        // Key Mesh
        thisKeyMesh.transform.parent.GetComponent<SpriteRenderer>().sortingOrder = index;
        thisKeyMesh.sortingOrder = index;

        // Time Mesh
        thisTimeMesh.transform.parent.GetComponent<SpriteRenderer>().sortingOrder = index;
        thisTimeMesh.sortingOrder = index;

        // Masking BG
        thisMask.GetComponent<SpriteRenderer>().sortingOrder = index - 20;
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

            panelStrat.MiniUpdate();
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

	private void OnMouseDown()
	{
        LayerToFront(GameControlling.layerAppend += 50);
        //Debug.Log(GameControlling.layerAppend);
        mouseClickPosOffset = this.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane + 1));

    }

	private void OnMouseDrag()
	{
        this.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane + 1)) + mouseClickPosOffset;
	}
}

public interface IPanelStrategy
{
    void ResetMinigame(GameObject parent); // Used when reinitialising game from object pool. Also as a just in case if things go wrong
    void OnControlDown(); // On the frame the control is pressed
    void OnControlHold(); // Each frame when the control is held down
    void OnControlUp(); // On the frame the control is released
    void OnTimeUp(); // What to do when the timer is up
    void MiniUpdate(); // Basically the Update() function for the panel strat scripts
}
