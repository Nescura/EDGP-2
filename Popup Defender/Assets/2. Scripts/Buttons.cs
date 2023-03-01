using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour
{
    private Scene myScene;
    public float downTime, upTime, upTime2; // Timers to check if a button is pushed down and up at the same time
    public bool mustDoubleClick, notVirus;
    public SpriteRenderer thisSprite;
    public TMPro.TextMeshPro thisTextMesh;
    public Sprite virusSpr;
    private RaycastHit2D hit;

    private void Start()
    {
        myScene = GameObject.Find("Scene").GetComponent<Scene>();
    }

    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
        hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

        // Colour Handling
        if (myScene.canInteract == true)
        {
            thisSprite.color = Color.Lerp(thisSprite.color, Color.white, Time.deltaTime * 5);
        }
        else
        {
            thisSprite.color = Color.Lerp(thisSprite.color, new Color(0.5f, 0.5f, 0.5f), Time.deltaTime * 5);
        }

        // Input Handling: will activate timers on click, and on release
        if (hit.collider != null && hit.collider.name == gameObject.name && myScene.canInteract == true)
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
                    upTime2 = 0.1f;
                }
                else
				{
                    upTime = 0.2f;
				}
            }
        }

        // Timers
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
    }

	public void OnCollisionStay2D(Collision2D collision)
	{
        if (this.name == "Litterbin" && collision.gameObject.name != "BrowserBttn" && collision.gameObject.TryGetComponent(out Buttons btnScr) && (btnScr.upTime > 0 || btnScr.upTime2 > 0))
		{
            myScene.ObjectEnd("Dcon", collision.gameObject);
		}
	}

	public void ActivateFunction(RaycastHit2D hit)
	{
        if (hit.collider.gameObject.name == "BrowserBttn" && myScene.canInteract == true)
        {
            myScene.BrowserButton();
            ResetTimers();
        }
        else if (hit.collider.gameObject.name == "Website Bttn" && myScene.canInteract == true)
        {
            myScene.WebsiteButton();
            ResetTimers();
        }
        else if (hit.collider.gameObject.name == "Submit Bttn" && myScene.canInteract == true)
        {
            myScene.SubmitButton();
            ResetTimers();
        }
        else if (notVirus == false)
		{
            myScene.WrongButton();
            thisSprite.color = new Color(1f, 0f, 0f);
            thisSprite.sprite = virusSpr;
            if (thisTextMesh != null) thisTextMesh.text = "HAHAHAHA\nHAHAHAHA";
		}
    }

    public void ResetTimers()
	{
        downTime = 0f;
        upTime = 0f;
        upTime2 = 0f;
	}
}
