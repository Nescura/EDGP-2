using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore;

public class Panel : MonoBehaviour
{
    [Header("Initialization Variables")]
    public bool isObjectiveClear = false;
    public float /*panelSizeX = 1.125f, panelSizeY = 1.2f,*/ timeInit, timeLeft, expiryTime = 0f; // NOTE: Timers here are Mini Timers
    private KeyCode assignedKey;
    public IPanelStrategy panelStrat;
    public GameObject thisDisplayParent;
    public string[] keyTechs = {"Press", "Tap", "Hold", "Spam"};

    [Header("Display Stuff (please leave them empty)")]
    public Color thisKeyBGColour;
    public GameObject thisMask, thisTimeParent, thisTimeBarFill;
    public SpriteRenderer thisKeyBG, thisTimeBarBG, thisTimeBarSpr;
    public TMPro.TextMeshPro thisKeyMesh, thisTimeMesh, thisTimeMeshDec, thisObjectiveKeyTechMesh, thisObjectiveMesh;

    [Header("Panel Dragging Stuff")]
    public Vector3 mouseClickPosOffset;

    // Initialization of variables - there's a lot here, it's because these variables would change very often. Please bear with it lol
    public void Initialize(IPanelStrategy panelStrategy, float timeInSecs, KeyCode key)
    {
        #region Define parent for interface to spawn gameobjects in
        thisDisplayParent = transform.Find("DisplayGameObjs").gameObject;
        #endregion

        #region Get a random input from the input list
        assignedKey = key;
        #endregion

        #region Setting strategies
        this.panelStrat = panelStrategy;
        //panelSizeX = sizeX; panelSizeY = sizeY;
        timeInit = timeInSecs;

        /* This doesn't need to be here. If we wanted to make time tick down faster for this minigame, we can do it in that script instead. Refer to line 97 in PanelDonutTouch.cs
        if (this.panelStrat.ToString() == "PanelDonutTouch")
        {
            timeLeft = timeInit - 5f;
        }
        else
        {
            timeLeft = timeInit - 1f;
        }*/
        #endregion

        #region Assign the mask in child
        thisMask = transform.Find("PanelMask").gameObject;
        #endregion

        #region Assign the key display's components
        thisKeyBG = transform.Find("KeyBG").GetComponent<SpriteRenderer>();
        thisKeyMesh = thisKeyBG.transform.Find("KeyTxt").GetComponent<TMPro.TextMeshPro>();
        if (assignedKey == KeyCode.Space)
		{
            thisKeyMesh.text = "Spc"; // may be too long? 
        }
        else
		{
            thisKeyMesh.text = assignedKey.ToString();
		}
        thisKeyBGColour = new Color (0f, 0f, 0f, 155f / 255f);
        #endregion

        #region Assign the time display's components
        thisTimeParent = transform.Find("TimeParent").gameObject;
        thisTimeMesh = thisTimeParent.transform.Find("TimeTxt").GetComponent<TMPro.TextMeshPro>();
        thisTimeMeshDec = thisTimeParent.transform.Find("TimeTxtDec").GetComponent<TMPro.TextMeshPro>();
        thisTimeBarFill = thisTimeParent.transform.Find("TimeBarFill").gameObject;
        thisTimeBarSpr = thisTimeBarFill.transform.Find("TimeBarSpr").GetComponent<SpriteRenderer>();
        thisTimeBarBG = thisTimeParent.transform.Find("TimeBarBG").GetComponent<SpriteRenderer>();
        #endregion

        #region Assign the objective display's text component and show the minigame's objective
        thisObjectiveKeyTechMesh = transform.Find("ObjectiveTxtLeft").GetComponent<TMPro.TextMeshPro>();
        thisObjectiveMesh = transform.Find("ObjectiveTxtRight").GetComponent<TMPro.TextMeshPro>();
        #endregion

        #region Reset the minigame and set their parents up (for success)
        panelStrat.ResetMinigame(this.gameObject, thisDisplayParent);
        #endregion

        #region Set size of panel
        GetComponent<RectTransform>().sizeDelta = panelStrat.SetPanelSize(); //new Vector2(panelSizeX, panelSizeY);
        GetComponent<SpriteRenderer>().size = new Vector2(panelStrat.SetPanelSize().x + 0.497f, panelStrat.SetPanelSize().y + 0.49f); //new Vector2(panelSizeX, panelSizeY);
        thisMask.GetComponent<SpriteRenderer>().size = panelStrat.SetPanelSize(); // new Vector2(panelSizeX, panelSizeY);
        GetComponent<BoxCollider2D>().size = new Vector2(panelStrat.SetPanelSize().x + 0.1125f, panelStrat.SetPanelSize().y + 0.175f); // new Vector2(panelSizeX, panelSizeY); // collider
        #endregion

        #region Set expiryTime to zero - this timer is purely for animation when an objective is completed
        expiryTime = 0f;
        #endregion
    }

    public void SetSuccess(bool b) // Checks whether the minigame is cleared or not
	{
        isObjectiveClear = b;
	}

    public void ForceTimeLeft(float f, bool directSet) // Forcefully change the time minigame has left
    {
        if (f < 0) thisTimeBarBG.color = new Color(1f, 0f, 0f, 1f);
        if (f > 0) thisTimeBarBG.color = new Color(0f, 188f / 255f, 0f, 155f / 255f);

        if (directSet) // if directSet is true, it SETS the time to f
        {
            timeLeft = f;
        }
        else // if directSet is false, it adds f to the time. If you want to reduce time, f would need to be negative
        {
            timeLeft += f;
        }
    }

    public void LayerToFront(int index)
	{
        // Border
        GetComponent<SpriteRenderer>().sortingOrder = index + 10; // extra numbers are their "deafult" values

        // Objects in Panel Minigame
        GameObject displayObjs = transform.Find("DisplayGameObjs").gameObject;

        // Layering all objects with SpriteRenderer components in DisplayGameObjs
        SpriteRenderer[] spriteRenders = displayObjs.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sprRender in spriteRenders) sprRender.sortingOrder = index - 5 + sprRender.GetComponent<LayerAppending>().indexToAppend;

        // Layering all objects with TextMeshPro components in DisplayGameObjs
        TMPro.TextMeshPro[] textMeshes = displayObjs.GetComponentsInChildren<TMPro.TextMeshPro>();
        foreach (TMPro.TextMeshPro tmPro in textMeshes) tmPro.sortingOrder = index - 5;

        #region To be Removed
        /*
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
        }*/
        #endregion

        // Key Mesh
        thisKeyMesh.transform.parent.GetComponent<SpriteRenderer>().sortingOrder = index + 14;
        thisKeyMesh.sortingOrder = index + 15;

        // Time Bars and Mesh
        //thisTimeMesh.transform.parent.GetComponent<SpriteRenderer>().sortingOrder = index;
        thisTimeBarBG.sortingOrder = index + 12;
        thisTimeBarSpr.sortingOrder = index + 13;
        thisTimeMesh.sortingOrder = index + 15;
        thisTimeMeshDec.sortingOrder = index + 15;

        // Objective Mesh
        thisObjectiveKeyTechMesh.sortingOrder = index + 15;
        thisObjectiveMesh.sortingOrder = index + 15;

        // Masking BG
        thisMask.GetComponent<SpriteRenderer>().sortingOrder = index - 20;
    }

    // Update is called once per frame
    void Update()
    {
        // Objective Text Handling
        thisObjectiveKeyTechMesh.text = keyTechs[Mathf.Clamp(panelStrat.ObjectiveKeyTech(), 0, 3)];
        thisObjectiveMesh.text = panelStrat.ObjectiveDesc();

        // Timer Handling
        thisTimeBarFill.transform.localScale = new Vector3(Mathf.Clamp(timeLeft / timeInit, 0f, 1f), 1, 0);

        if (GameControlling.GetInstance().GetComponent<GameState>().state == GameCurrentState.START)
        {
            if (timeLeft > 0)
            {
                if (!isObjectiveClear) timeLeft -= Time.deltaTime; // Only decrease time if objective is not complete

                panelStrat.MiniUpdate();
            }
            else
            {
                panelStrat.OnTimeUp();
                //Destroy(this.gameObject); This shouldn't be here. Lines 199 to 217 already does this

                //gameControlling.popupCounter -= 1;
            }
        }
        
        thisTimeMesh.text = string.Format("{0}", Mathf.FloorToInt(Mathf.Abs((int)timeLeft)));
        thisTimeMeshDec.text = string.Format(".{0}", (int)Mathf.FloorToInt(Mathf.Abs(timeLeft * 10 % 10)));
        //Debug.Log((int)Mathf.Abs(timeLeft * 10 % 10));

        // Expiry Actions - executed ONLY when isObjective is true, or if you failed
        if (isObjectiveClear || timeLeft <= 0)
		{
            expiryTime += Time.deltaTime;

            if ((expiryTime >= 1.2f && isObjectiveClear) || (expiryTime >= 3.5f && !isObjectiveClear)) // After expiryTime is up, run all the stuff that deactivates it 
			{
                GameControlling.GetInstance().inputManager.ReturnKey(assignedKey);

                if (isObjectiveClear)
				{
                    // virus timer regen code and minigame clear animation stuff goes here
                    GameControlling.GetInstance().GetComponent<GameTimer>().AddVirusTimer(+5f);
                }
                else
				{
                    // minigame fail animation goes here
                }

                Destroy(this.gameObject); // gameObject.SetActive(false); replace with object pooling expire down the line
            }
		}

        // Input Handling
        if (timeLeft > 0)
        {
            if (Input.GetKeyDown(assignedKey))
            {
                panelStrat.OnControlDown();
                thisKeyBGColour = new Color(1f, 0f, 105f / 255f, 155f / 255f);
            }
            if (Input.GetKey(assignedKey))
            {
                panelStrat.OnControlHold();
            }
            if (Input.GetKeyUp(assignedKey))
            {
                panelStrat.OnControlUp();
                thisKeyBGColour = new Color(0f, 0f, 0f, 155f / 255f);
            }
        }

        // Change colour of the time & key's BG for *flair*
        thisKeyBG.color = Color.Lerp(thisKeyBG.color, thisKeyBGColour, 0.05f);
        thisTimeBarBG.color = Color.Lerp(thisTimeBarBG.color, new Color(0f, 0f, 0f, 155f / 255f), 0.01f);

        // Objective Clearing
        if (timeLeft <= 0 && !isObjectiveClear)
		{
            GetComponent<SpriteRenderer>().color = Color.red;
        }
        else if (isObjectiveClear)
		{
            GetComponent<SpriteRenderer>().color = Color.green;
        }
        else
		{
            GetComponent<SpriteRenderer>().color = Color.white;
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
    Vector2 SetPanelSize(); // Defines the size of the panel. Should be unique for each minigame. ALWAYS return a new Vector2 for this one
    int ObjectiveKeyTech(); // 0 for Press, 1 for Tap, 2 for Hold, 3 for Spam
    string ObjectiveDesc(); // Defines the objective of the minigame as instructions for the player. You do NOT need to put a space in the beginning of the string
    void ResetMinigame(GameObject panelParent, GameObject displayParent); // Used when reinitialising game from object pool. Also as a just in case if things go wrong
    void OnControlDown(); // On the frame the control is pressed
    void OnControlHold(); // Each frame when the control is held down
    void OnControlUp(); // On the frame the control is released
    void OnTimeUp(); // What to do when the timer is up
    void MiniUpdate(); // Basically the Update() function for the panel strat scripts
}
