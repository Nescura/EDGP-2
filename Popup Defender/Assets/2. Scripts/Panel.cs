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
    private Color thisKeyBGColour;
    private GameObject thisMask, thisTimeParent, thisTimeBarFill;
    private SpriteRenderer thisKeyBG, thisTimeBarBG, thisTimeBarSpr;
    private TMPro.TextMeshPro thisKeyMesh, thisTimeMesh, thisTimeMeshDec, thisObjectiveKeyTechMesh, thisObjectiveMesh;

    [Header("Panel Dragging Stuff")]
    public Vector3 mouseClickPosOffset;

    [Header("Panel Look Variations")]
    public Sprite[] panelSprites;

    private float speedX, speedY;
    private bool isClearPlayed;

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
        timeInit = timeInSecs;

        GetComponent<SpriteRenderer>().sprite = panelSprites[Random.Range(0, panelSprites.Length)];

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
        GetComponent<RectTransform>().localScale = new Vector3(2f, 2f, 1f);
        GetComponent<RectTransform>().sizeDelta = panelStrat.SetPanelSize(); //new Vector2(panelSizeX, panelSizeY);
        GetComponent<SpriteRenderer>().size = new Vector2(panelStrat.SetPanelSize().x + 0.497f, panelStrat.SetPanelSize().y + 0.49f); //new Vector2(panelSizeX, panelSizeY);
        //thisMask.GetComponent<SpriteRenderer>().size = panelStrat.SetPanelSize(); // new Vector2(panelSizeX, panelSizeY);
        thisMask.transform.localScale = panelStrat.SetPanelSize();
        GetComponent<BoxCollider2D>().size = new Vector2(panelStrat.SetPanelSize().x + 0.1125f, panelStrat.SetPanelSize().y + 0.175f); // new Vector2(panelSizeX, panelSizeY); // collider
        #endregion

        #region Set Background of panel minigame
        if (panelStrat.SetPanelBG() == "") // If no BG was defined, just load the default square
		{
            thisMask.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("UISquare");
            thisMask.GetComponent<SpriteRenderer>().color = new Color(68f / 255f, 62f / 255f, 96f / 255f);
        }
        else
		{
            thisMask.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("BGSprites/" + panelStrat.SetPanelBG());
            thisMask.GetComponent<SpriteRenderer>().color = Color.white;
        }
		#endregion

		#region Other Miscellaneous Stuff
        //Set expiryTime to zero - this timer is purely for animation when an objective is completed
		expiryTime = 0f;

        // Set the boolean to whether the cleared sound is played or not back to false
        isClearPlayed = false;
        #endregion
    }

    public void SetSuccess(bool b) // Checks whether the minigame is cleared or not
	{
        isObjectiveClear = b;
    }

    public void ForceTimeLeft(float f, bool directSet, bool animateColour) // Forcefully change the time minigame has left
    {
        if (animateColour)
        {
            if (f < 0) thisTimeBarBG.color = new Color(1f, 0f, 0f, 1f);
            if (f > 0) thisTimeBarBG.color = new Color(0f, 188f / 255f, 0f, 155f / 255f);
        }

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
        foreach (SpriteRenderer sprRender in spriteRenders)
        {
            sprRender.sortingOrder = index - 5 + sprRender.GetComponent<LayerAppending>().indexToAppend;
            sprRender.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        }

        // Layering all objects with TextMeshPro components in DisplayGameObjs
        TMPro.TextMeshPro[] textMeshes = displayObjs.GetComponentsInChildren<TMPro.TextMeshPro>();
        foreach (TMPro.TextMeshPro tmPro in textMeshes) tmPro.sortingOrder = index - 5;

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

        // Set the masking BG to only mask things within these orders (THE MOST IMPORTANT STEP)
        thisMask.GetComponent<SpriteMask>().frontSortingOrder = index + 25;
        thisMask.GetComponent<SpriteMask>().backSortingOrder = index - 25;
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
                if (!isObjectiveClear)
                {
                    timeLeft -= Time.deltaTime; // Only decrease time if objective is not complete
                    
                    panelStrat.MiniUpdate();
                }
            }
            else
            {
                panelStrat.OnTimeUp();
            }
        }
        
        thisTimeMesh.text = string.Format("{0}", Mathf.FloorToInt(Mathf.Abs((int)timeLeft)));
        thisTimeMeshDec.text = string.Format(".{0}", (int)Mathf.FloorToInt(Mathf.Abs(timeLeft * 10 % 10)));
        //Debug.Log((int)Mathf.Abs(timeLeft * 10 % 10));

        // Expiry Actions - executed ONLY when isObjective is true, or if you failed
        if (isObjectiveClear || timeLeft <= 0)
		{
            expiryTime += Time.deltaTime;
            StartCoroutine(PlayClearSFX());

            if ((expiryTime >= 0.7f && isObjectiveClear) || (expiryTime >= 3f && !isObjectiveClear))
			{
                transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(3f, 0f, 1f), Time.deltaTime * 10);
            }

            if ((expiryTime >= 1.2f && isObjectiveClear) || (expiryTime >= 3.5f && !isObjectiveClear)) // After expiryTime is up, run all the stuff that deactivates it 
			{
                GameControlling.GetInstance().inputManager.ReturnKey(assignedKey);

                if (isObjectiveClear)
				{
                    // virus timer regen code and minigame clear animation stuff goes here
                    GameControlling.GetInstance().GetComponent<GameTimer>().AddVirusTimer(2f);
                }
                else
				{
                    // minigame fail animation goes here
                }

                Destroy(this.gameObject); // gameObject.SetActive(false); replace with object pooling expire down the line
            }
        }

        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(3f, 3f, 1f), Time.deltaTime * 10);
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
        // Note: moved sound stuff into a function at PlayClearSFX, then moved to line 216
        if (isObjectiveClear)
		{
            GetComponent<SpriteRenderer>().color = Color.green;
        }
        else if (timeLeft > 0)
		{
            GetComponent<SpriteRenderer>().color = Color.white;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.red;
        }

        // Preventing Panel from moving too far offscreen
        CameraBoundaryCheck();

        // Moving the panels on their own
        MoveMe(speedX, speedY);
    }

    private IEnumerator PlayClearSFX()
	{
        yield return new WaitForFixedUpdate();
        
        if (!isClearPlayed)
		{
            if (!isObjectiveClear)
            {
                FindObjectOfType<AudioManager>().Play("FailMiniGame");
            }
            else
            {
                FindObjectOfType<AudioManager>().Play("ClearMiniGame");
            }

            isClearPlayed = true;
        }
    }

    private void OnMouseDown()
    {
        if (GameControlling.GetInstance().GetComponent<GameState>().state == GameCurrentState.START)
        {
            LayerToFront(GameControlling.layerAppend += 50);

            mouseClickPosOffset = this.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane + 1));
        }
    }

    private void OnMouseDrag()
    {
        if (GameControlling.GetInstance().GetComponent<GameState>().state == GameCurrentState.START)
        {
            this.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane + 1)) + mouseClickPosOffset;
        }
    }

    public void CameraBoundaryCheck() // Prevents the panel from moving too far away from offscreen
    {
        float moveX = transform.position.x;
        float moveY = transform.position.y;

        Vector2 viewportZero = GameControlling.GetInstance().mainCamera.ViewportToWorldPoint(Vector2.zero);
        Vector2 viewportOne = GameControlling.GetInstance().mainCamera.ViewportToWorldPoint(Vector2.one);

        // If Panel is out of the screen from the left
        if (moveX > viewportOne.x)
            moveX = Mathf.Lerp(moveX, viewportOne.x, 0.02f);
        else if (transform.position.x < viewportZero.x)
            moveX = Mathf.Lerp(moveX, viewportZero.x, 0.02f);

        if (moveY > viewportOne.y)
            moveY = Mathf.Lerp(moveY, viewportOne.y, 0.02f);
        else if (transform.position.y < viewportZero.y)
            moveY = Mathf.Lerp(moveY, viewportZero.y, 0.02f);

        //Debug.Log(playerPosX + ", " + playerPosY);
        transform.position = new Vector2(moveX, moveY);
    }

    public void MoveMe(float x, float y) // Forcefully moves the panel based on inputs
	{
        Vector2 viewportZero = GameControlling.GetInstance().mainCamera.ViewportToWorldPoint(Vector2.zero);
        Vector2 viewportOne = GameControlling.GetInstance().mainCamera.ViewportToWorldPoint(Vector2.one);

        if (transform.position.x > viewportOne.x) speedX = -Mathf.Abs(speedX);
        else if (transform.position.x < viewportZero.x) speedX = Mathf.Abs(speedX);

        if (transform.position.y > viewportOne.y) speedY = -Mathf.Abs(speedY);
        else if (transform.position.y < viewportZero.y) speedY = Mathf.Abs(speedY);

        transform.position += new Vector3(x * Time.deltaTime, y * Time.deltaTime, 0);
	}

    public void SetSpeed(float x, float y)
	{
        speedX = x; speedY = y;
	}
}

public interface IPanelStrategy
{
    Vector2 SetPanelSize(); // Defines the size of the panel. Should be unique for each minigame. ALWAYS return a new Vector2 for this one
    string SetPanelBG(); // Defines the bg for the sprite. The input here is the NAME of the background sprite in Resources/BGSprites
    int ObjectiveKeyTech(); // 0 for Press, 1 for Tap, 2 for Hold, 3 for Spam
    string ObjectiveDesc(); // Defines the objective of the minigame as instructions for the player. You do NOT need to put a space in the beginning of the string
    void ResetMinigame(GameObject panelParent, GameObject displayParent); // Used when reinitialising game from object pool. Also as a just in case if things go wrong
    void OnControlDown(); // On the frame the control is pressed
    void OnControlHold(); // Each frame when the control is held down
    void OnControlUp(); // On the frame the control is released
    void OnTimeUp(); // What to do when the timer is up
    void MiniUpdate(); // Basically the Update() function for the panel strat scripts
}
