using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scene : MonoBehaviour
{
    public GameObject desktopBttnPrefab, browserBttnPrefab, websiteBttnPrefab;

    public GameObject desktopPage, browserPage, websitePage;
    private GameObject browserBttn, websiteBttn, submitBttn;
    //public List<GameObject> desktopButtons = new List<GameObject> { }, browserButtons = new List<GameObject> { }, submitButtons = new List<GameObject> { };

    public GameObject dayPage;
    public TMPro.TextMeshProUGUI dayTextMesh;
    public bool canInteract;

    private GameControlling gameCtrl;

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
        gameCtrl = GameControlling.GetInstance();
        SubmitButton();
    }

    private void Update()
    {
        ButtonCanInteract();
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
        /*foreach (GameObject b in activeBrowserIcons)
        {
            ObjectEnd("BrowserIcon", b);
        }
        foreach (GameObject s in activeSubmitBttns)
        {
            ObjectEnd("SubmitBttn", s);
        }*/

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
        // nothing here for now
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
        FindObjectOfType<AudioManager>().Play("Button");

        Application.Quit();
    }

    public void Retry()
    {
        FindObjectOfType<AudioManager>().Play("Button");

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        FindObjectOfType<AudioManager>().Play("Button");
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
        desktopPage.SetActive(false);
        browserPage.SetActive(false);
        websitePage.SetActive(false);

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
        yield return new WaitForSeconds(1.5f);

        SetupLevel(gameCtrl.GetComponent<Levels>().currentLevel);

        yield return new WaitForSeconds(0.1f);
        dayPage.SetActive(false);
        gameCtrl.GetComponent<GameTimer>().ResetSystemTimer();
        gameCtrl.GetComponent<GameTimer>().myDeadLineTxt.enabled = true;
        desktopPage.SetActive(true);
        gameCtrl.spawnPanelTime = 0f;
    }

    // Decoupled so that all buttons can be interacted with less hardcoding
    public void ButtonCanInteract()
    {
        if (GameControlling.GetInstance().GetActivePanelCount() > 0)
        {
            canInteract = false;

            /*browserBttn.GetComponent<SpriteRenderer>().color = new Color(125f / 225f, 125f / 225f, 125f / 225f);
            websiteBttn.GetComponent<SpriteRenderer>().color = new Color(125f / 225f, 125f / 225f, 125f / 225f);
            submitBttn.GetComponent<SpriteRenderer>().color = new Color(125f / 225f, 125f / 225f, 125f / 225f);*/
        }
        else
        {
            canInteract = true;

            /*browserBttn.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
            websiteBttn.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
            submitBttn.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);*/
        }
    }
}
