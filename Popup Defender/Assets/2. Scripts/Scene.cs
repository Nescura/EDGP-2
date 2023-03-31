using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scene : MonoBehaviour
{
    public GameObject desktopBttnPrefab, browserBttnPrefab, submitBttnPrefab;
    public bool canClick;
    public GameObject desktopPage, browserPage, websitePage, yayFlash;
    private GameObject browserBttn, websiteBttn, submitBttn;
    //public List<GameObject> desktopButtons = new List<GameObject> { }, browserButtons = new List<GameObject> { }, submitButtons = new List<GameObject> { };

    public GameObject dayPage;
    public TMPro.TextMeshProUGUI dayTextMesh;
    public bool canInteract;

    private GameControlling gameCtrl;

    [Header("Randomised Submit Button Text")]
    private string[] submitTextArray = new string[] { "Susmit", "Submiss", "Subsist", "Superst", "Sauce", "5ubm1t", "Subway", "sUbMiT", "timbuS", "Simbat", "Sumbit", "Simbutt" };

    #region Dynamic Object Pooling
    public static Dictionary<string, List<GameObject>> objectPools = new Dictionary<string, List<GameObject>>();
    public List<GameObject> activeDesktopIcons = new List<GameObject>();
    public List<GameObject> activeBrowserIcons = new List<GameObject>();
    public List<GameObject> activeSubmitBttns = new List<GameObject>();

    public static List<GameObject> GetPool(string poolName)
    // Only called in ObjectEnd() - put an object into a specified object pool. If it doesn't exist, create it then put it in
    {
        if (!objectPools.ContainsKey(poolName))
        {
            objectPools.Add(poolName, new List<GameObject>());
        }

        return objectPools[poolName];
    }

    private GameObject ObjectUse(string objName, System.Action<GameObject> objLoaded, GameObject objPrefab, Transform parentToSpawnIn)
    // The Instantiate() of object pooling - specifies the name of the object, what its initial variables can be (optional), and the object to spawn if the object does not exist in any pool before
    {
        GameObject objChosen;
        if (objectPools.TryGetValue(objName, out List<GameObject> z) && objectPools[objName].Find(obj => obj.name.Contains(objName)))
        {
            objChosen = objectPools[objName].Find(obj => obj.name.Contains(objName));
            objLoaded?.Invoke(objChosen);
            objectPools[objName].Remove(objChosen);
        }
        else
        {
            objChosen = Instantiate(objPrefab, parentToSpawnIn);
            objLoaded?.Invoke(objChosen);
        }

        return objChosen;
    }

    public void ObjectEnd(string objName, GameObject obj)
    // The Destroy() of object pooling - puts the object into a pool using its name as a reference
    {
        GetPool(objName).Add(obj);
        obj.SetActive(false);
    }
    #endregion

    private void Start()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            gameCtrl = GameControlling.GetInstance();
            gameCtrl.GetComponent<GameDataManager>().Load();
            SubmitButton();
        }
        else if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            GetComponent<GameDataManager>().Load();
            if (GetComponent<GameDataManager>().ReadFromFile("data.txt") == "" || GameObject.Find("Scene").GetComponent<GameDataManager>().data.dayCleared < 1)
			{
                GameControlling.tutorialPlease = true;
			}

            canClick = true;
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            ButtonCanInteract();
        }
        

        if (Input.GetMouseButtonDown(0))
        {
            FindObjectOfType<AudioManager>().SetPitch("MouseDown", Random.Range(1.9f, 2.1f));
            FindObjectOfType<AudioManager>().Play("MouseDown");
        }

        if (Input.GetMouseButtonUp(0))
        {
            FindObjectOfType<AudioManager>().SetPitch("MouseUp", Random.Range(1.9f, 2.1f));
            FindObjectOfType<AudioManager>().Play("MouseUp");
        }

        if (Input.anyKeyDown)
        {
            FindObjectOfType<AudioManager>().SetPitch("Keypress", Random.Range(0.9f, 1.1f));
            FindObjectOfType<AudioManager>().Play("Keypress");
        }

        if (Input.GetKeyDown(KeyCode.PageDown))
        {
            GameControlling.GetInstance().spawnMax = 20;
            GameControlling.GetInstance().spawnPanelTimeAvg = 10f;
            GameControlling.GetInstance().GetComponent<GameTimer>().virusTimerInit = 10f;

            gameCtrl.GetComponent<Levels>().currentLevel = 20;

            SubmitButton();
        }
    }

    #region Randomisation of Button Locations
    public void SetupLevel(int level)
	{
        // First, flush all Buttons
        if (browserBttn != null) ObjectEnd(browserBttn.name, browserBttn); 
        foreach (GameObject d in activeDesktopIcons)
        {
            ObjectEnd(d.name, d);
        }
        activeDesktopIcons.Clear();
        /*foreach (GameObject b in activeBrowserIcons)
        {
            ObjectEnd("BrowserIcon", b);
        }*/
        if (submitBttn != null) ObjectEnd(submitBttn.name, submitBttn);
        foreach (GameObject s in activeSubmitBttns)
        {
            ObjectEnd(s.name, s);
        }
        activeSubmitBttns.Clear();
        // Creation of Desktop Buttons
        if (browserBttn == null) // if the real browser button hasn't been made, make it
		{
            browserBttn = ObjectUse("BrowserBttn", (browserIcon) =>
            {
                browserIcon.name = "BrowserBttn";
                browserIcon.GetComponent<DesktopIcon>().isCorrectBttn = true;
                browserIcon.GetComponent<DesktopIcon>().Initialise();
            }, desktopBttnPrefab, desktopPage.transform);
        }
        else // same as above, but requires less setup
		{
            ObjectUse(browserBttn.name, (browserIcon) =>
            {
                browserIcon.name = "BrowserBttn";
                browserIcon.GetComponent<DesktopIcon>().Initialise();
            }, desktopBttnPrefab, desktopPage.transform);
        }
        for (int i = (level - 1) * 2; i > 0;  i -= 1)
		{
            GameObject d = ObjectUse("dsktpIcon", (desktopIcon) =>
            {
                desktopIcon.name = "dsktpIcon";
                desktopIcon.GetComponent<DesktopIcon>().Initialise();
            }, desktopBttnPrefab, desktopPage.transform);
            activeDesktopIcons.Add(d);
        }

        // Creation of Browser Buttons (will have more than one bookmark)
        // nothing here for now

        // Creation of Submit Buttons (will have many buttons of similar looking names, e.g. "Susmit" and "Summit")
        if (submitBttn == null) // if the real browser button hasn't been made, make it
        {
            submitBttn = ObjectUse("SubmitBttn", (SubmitBttn) =>
            {
                SubmitBttn.name = "SubmitBttn";
                SubmitBttn.GetComponent<Buttons>().notVirus = true;
                SubmitBttn.GetComponent<Buttons>().thisTextMesh.text = "Submit";
                SubmitBttn.transform.localPosition = new Vector2(0.3f, -4.5f);
                SubmitBttn.SetActive(true);
            }, submitBttnPrefab, websitePage.transform);
        }
        else // same as above, but requires less setup
        {
            ObjectUse(submitBttn.name, (SubmitBttn) =>
            {
                SubmitBttn.name = "SubmitBttn";
                SubmitBttn.GetComponent<Buttons>().notVirus = true;
                SubmitBttn.GetComponent<Buttons>().thisTextMesh.text = "Submit";
                SubmitBttn.transform.localPosition = new Vector2(Random.Range(-5f, 5f), Random.Range(-4.65f, 1f));
                SubmitBttn.SetActive(true);
                if (level >= 6)
                {
                    SubmitBttn.GetComponent<SubmitMove>().SetSpeed(Random.Range(level / -9f, level / 9f), Random.Range(level / -9f, level / 9f));
                }
            }, submitBttnPrefab, websitePage.transform);
        }
        for (int i = (level - 1) * 2; i > 0; i -= 1)
        {
            GameObject s = ObjectUse("SusBttn", (susBttn) =>
            {
                susBttn.name = "SusBttn";
                susBttn.GetComponent<Buttons>().notVirus = false;
                susBttn.GetComponent<Buttons>().thisTextMesh.text = submitTextArray[Random.Range(0, submitTextArray.Length)];
                susBttn.transform.localPosition = new Vector2(Random.Range(-5f, 5f), Random.Range(-4.65f, 1f));
                if (submitBttn.GetComponent<Collider2D>().IsTouching(susBttn.GetComponent<Collider2D>()))
				{
                    ObjectEnd(susBttn.name, susBttn);
                    activeSubmitBttns.Remove(susBttn);
                    i += 1;
                }
                if (level >= 6)
				{
                    susBttn.GetComponent<SubmitMove>().SetSpeed(Random.Range(level / -9f, level / 9f), Random.Range(level / -9f, level / 9f));
				}
                susBttn.SetActive(true);
            }, submitBttnPrefab, websitePage.transform);
            activeSubmitBttns.Add(s);
        }
    }
    #endregion

    #region Bttn Functions
    public void Play()
    {
        SceneManager.LoadScene(1);
        FindObjectOfType<AudioManager>().Stop("MenuBGM");
        FindObjectOfType<AudioManager>().Play("GameBGM");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        FindObjectOfType<AudioManager>().Stop("GameBGM");
        FindObjectOfType<AudioManager>().Play("MenuBGM");
        
        SceneManager.LoadScene(0);
    }

    public void BrowserButton()
    {
        desktopPage.SetActive(false);
        browserPage.SetActive(true);

        gameCtrl.spawnPanelTime = 0f;
    }

    public void WebsiteButton()
    {
        browserPage.SetActive(false);
        websitePage.SetActive(true);

        gameCtrl.spawnPanelTime = 0f;
    }

    public void SubmitButton()
    {
        // tams here, moved some stuff into the coroutine. If this messes stuff up... I'm sorry
        gameCtrl.GetComponent<GameTimer>().myDeadLineTxt.enabled = false;
        gameCtrl.spawnPanelTime = 99f;
        gameCtrl.GetComponent<Levels>().CheckCurrentLevel();

        dayTextMesh.text = string.Format("Day {0}", gameCtrl.GetComponent<Levels>().currentLevel);
        dayPage.SetActive(true);
        StartCoroutine(DesktopPage());
    }

    public void WrongButton()
	{
        gameCtrl.spawnPanelTime = 0f;
    }
    #endregion

    IEnumerator DesktopPage()
    {
        yayFlash.GetComponent<Image>().color = new Color(60f / 255f, 1f, 120f / 255f, 1f);

        desktopPage.SetActive(false);
        browserPage.SetActive(false);
        websitePage.SetActive(false);

        // Clear every panel (just in case)
        Panel[] panelInstances = FindObjectsOfType<Panel>();
        if (panelInstances.Length > 0)
        {
            foreach (Panel thisPan in panelInstances)
            {
                thisPan.isObjectiveClear = true;
                thisPan.ForceTimeLeft(0, true, true);
            }
        }

        for (float s = 2; s > 0; s -= Time.deltaTime)
		{
            yayFlash.GetComponent<Image>().color = Color.Lerp(yayFlash.GetComponent<Image>().color, new Color(60f / 255f, 1f, 120f / 255f, 0f), Time.deltaTime * 4f);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        yield return new WaitForSeconds(1.5f);

        gameCtrl.GetComponent<Levels>().clearedLevel += 1;
        SetupLevel(gameCtrl.GetComponent<Levels>().currentLevel);

        yield return new WaitForSeconds(0.1f);
        dayPage.SetActive(false);
        gameCtrl.GetComponent<GameTimer>().ResetSystemTimer();
        gameCtrl.GetComponent<GameTimer>().myDeadLineTxt.enabled = true;
        desktopPage.SetActive(true);
        if (GameControlling.tutorialPlease && gameCtrl.GetComponent<Levels>().clearedLevel == 0)
		{
            gameCtrl.SpawnTutorialPanel();
            gameCtrl.spawnPanelTime = 1f;
        }
        else
        {
            GameControlling.tutorialPlease = false;
            gameCtrl.SpawnMaxPanels();
            gameCtrl.spawnPanelTime = 0f;
		}

    }

    // Decoupled so that all buttons can be interacted with less hardcoding
    public void ButtonCanInteract()
    {
        if (GameControlling.GetInstance().GetActivePanelCount() > 0)
        {
            canInteract = false;
        }
        else
        {
            canInteract = true;
        }
    }
}
