using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour
{
    private Scene myScene;
    public float downTime, upTime, upTime2; // Timers to check if a button is pushed down and up at the same time
    public bool mustDoubleClick, notVirus, clickable;
    public SpriteRenderer thisSprite;
    public TMPro.TextMeshPro thisTextMesh;
    public Sprite virusSpr;
    private RaycastHit2D hit;
    private float playGlitch;

    private void Start()
    {
        myScene = GameObject.Find("Scene").GetComponent<Scene>();
        clickable = false;
    }

    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
        hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

    # region Colour Handling
        if (myScene.canInteract == true)
        {
            thisSprite.color = Color.Lerp(thisSprite.color, Color.white, Time.deltaTime * 5);
        }
        else
        {
            thisSprite.color = Color.Lerp(thisSprite.color, new Color(0.5f, 0.5f, 0.5f), Time.deltaTime * 5);
        }
        #endregion

        #region Input Handling: will activate timers on click, and on release
        if (hit.collider != null && hit.collider.name == gameObject.name && myScene.canInteract == true && clickable)
        {
            if (Input.GetMouseButtonDown(0))
			{
                thisSprite.color = new Color(0.5f, 0.5f, 0.5f);
                downTime = 0.1f;
			}

            if (Input.GetMouseButtonUp(0))
            {
                if (mustDoubleClick && upTime > 0)
                {
                    upTime2 = 0.2f;
                }
                else
				{
                    upTime = 0.25f;
				}
            }
        }
        #endregion

        //Timers
        if (downTime > 0) downTime -= Time.deltaTime;
        if (upTime > 0) upTime -= Time.deltaTime;
        if (upTime2 > 0) upTime2 -= Time.deltaTime;

        // Button activating when conditions are met
        if (mustDoubleClick) // Check if button has been double clicked correctly
		{
            if (downTime > 0 && upTime2 > 0 && hit.collider != null)
            {
                ActivateFunction(hit);
            }
		}
        else // Check if button is clicked correctly: It has been clicked AND released
		{
            if (downTime > 0 && upTime > 0 && hit.collider != null)
            {
                ActivateFunction(hit);
            }
		}

        if (GameObject.Find("Main Camera").GetComponent<GlitchEffect>().glitch == true && GameControlling.GetInstance().GetComponent<GameState>().state != GameCurrentState.END)
        {
            FindObjectOfType<AudioManager>().Play("Glitch");
            playGlitch -= Time.deltaTime;
        }

        if (playGlitch <= 0)
        {
            GameObject.Find("Main Camera").GetComponent<GlitchEffect>().glitch = false;
            playGlitch = 1f;
        }
    }

	public void OnCollisionStay2D(Collision2D collision)
	{
        if (this.name == "Litterbin" && collision.gameObject.name != "BrowserBttn" && collision.gameObject.TryGetComponent(out Buttons btnScr) && (btnScr.upTime > 0 || btnScr.upTime2 > 0))
		{
            myScene.ObjectEnd("dsktpIcon", collision.gameObject);
            FindObjectOfType<AudioManager>().Play("Trashed");
        }
	}

	public void ActivateFunction(RaycastHit2D hit)
	{
        if (!clickable) return;

        if (hit.collider.gameObject.name == "BrowserBttn" && myScene.canInteract == true)
        {
            myScene.BrowserButton();
            GameControlling.GetInstance().SpawnMaxPanels();
            ResetTimers();
        }
        else if (hit.collider.gameObject.name == "Website Bttn" && myScene.canInteract == true)
        {
            myScene.WebsiteButton();
            GameControlling.GetInstance().SpawnMaxPanels();
            ResetTimers();
        }
        else if (hit.collider.gameObject.name == "SubmitBttn" && myScene.canInteract == true)
        {
            myScene.SubmitButton();
            FindObjectOfType<AudioManager>().Play("Submitted");
            ResetTimers();
        }
        else if (notVirus == false)
		{
            myScene.WrongButton();

            if (virusSpr != null)
            {
                thisSprite.sprite = virusSpr;
            }
            else
			{
                thisSprite.color = new Color(1f, 0f, 0f);
            }

            GameObject.Find("Main Camera").GetComponent<GlitchEffect>().glitch = true;
            if (thisTextMesh != null) thisTextMesh.text = "HAHAHA\nHAHAHA";
		}
    }

    public void ResetTimers()
	{
        downTime = 0f;
        upTime = 0f;
        upTime2 = 0f;
	}

	private void OnMouseOver()
	{
        clickable = true;
	}

	private void OnMouseExit()
	{
        clickable = false;
    }
}
