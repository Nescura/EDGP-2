using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesktopIcon : MonoBehaviour
{
    // WARNING: DO NOT CHANGE THE VARIABLE NAMES or we have to assign them all again and that's fuckin tedious :weary:
    [Header("Specific Icon Sprites")]
    public Sprite browserSprite;

    [Header("Other Randomised Icons")]
    [SerializeField]
    public Sprite[] iconSpriteArray;
    public string[] iconNameArray;

    // To chance these please look at the DesktopIcon prefab in the Resources folder
    [Header("Randomised Folder Names (go nuts!)")]
    public string[] folderNameArray;

    [Header("Randomised Text File Names (go nuts!)")]
    public string[] txtNameArray;

    private Scene myScene;
    private Vector3 mouseClickPosOffset;
    public Buttons myButtonScr;
    public GameObject iconHighlight;
    public bool isCorrectBttn, isLitterbin;

    // Start is called before the first frame update
    void Start()
    {
        Initialise();
    }

    // Update is called once per frame
    void Update()
    {
    }

	public void Initialise()
    {
        iconHighlight.SetActive(false);
        if (myScene == null) myScene = GameObject.Find("Scene").GetComponent<Scene>();

        if (isLitterbin) return;

        if (isCorrectBttn)
		{
            myButtonScr.thisSprite.sprite = browserSprite;
            myButtonScr.thisTextMesh.text = "Donut Browser";
            transform.position = new Vector3(Random.Range(-8f, 0f), Random.Range(-3f, 3.8f), 0);
        }
        else
		{
            int randomIndex = Random.Range(0, iconSpriteArray.Length);
            myButtonScr.thisSprite.sprite = iconSpriteArray[randomIndex];

            if (randomIndex == 0)
			{
                myButtonScr.thisTextMesh.text = folderNameArray[Random.Range(0, folderNameArray.Length)];
            }
            else if (randomIndex == 1)
            {
                myButtonScr.thisTextMesh.text = txtNameArray[Random.Range(0, txtNameArray.Length)];
            }
            else
			{
                myButtonScr.thisTextMesh.text = iconNameArray[randomIndex];
            }
            transform.position = new Vector3(Random.Range(-8f, 0f), Random.Range(-3f, 3.8f), -1);
        }

        gameObject.SetActive(true);
    }

	private void OnMouseOver()
    {
        iconHighlight.SetActive(myScene.canInteract);
    }

	private void OnMouseExit()
	{
        iconHighlight.SetActive(false);
	}

	private void OnMouseDown()
    {
        mouseClickPosOffset = this.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane + 1));
    }

    private void OnMouseDrag()
    {
        if (myScene.canInteract == true && name != "Litterbin")
        {
            this.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane + 1)) + mouseClickPosOffset;
        }
    }
}
