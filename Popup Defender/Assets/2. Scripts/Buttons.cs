using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour
{
    private Scene myScene;
    public float downTime, upTime, upTime2; // Timers to check if a button is pushed down and up at the same time
    public bool mustDoubleClick;

    private void Start()
    {
        myScene = GameObject.Find("Scene").GetComponent<Scene>();
    }

    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

        // Colour Handling
        if (myScene.canInteract == true)
        {
            GetComponent<SpriteRenderer>().color = Color.Lerp(GetComponent<SpriteRenderer>().color, Color.white, Time.deltaTime * 5);
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.Lerp(GetComponent<SpriteRenderer>().color, new Color(0.5f, 0.5f, 0.5f), Time.deltaTime * 5);
        }

        // Input Handling: will activate timers on click, and on release
        if (hit.collider == GetComponent<BoxCollider>() && myScene.canInteract == true)
        {
            if (Input.GetMouseButtonDown(0))
			{
                GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f);
                downTime = 0.5f;
			}

            if (Input.GetMouseButtonUp(0))
            {
                if (mustDoubleClick && upTime > 0)
                {
                    upTime2 = 0.5f;
                }
                else
				{
                    upTime = 0.5f;
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
            if (downTime > 0 && upTime2 > 0)
            {
                ActivateFunction(hit);
            }
		}
        else // Check if button is clicked correctly: It has been clicked AND released
		{
            if (downTime > 0 && upTime > 0)
            {
                ActivateFunction(hit);
            }
		}
    }

    public void ActivateFunction(RaycastHit2D hit)
	{
        if (hit.collider.gameObject.name == "Browser Bttn" && myScene.canInteract == true)
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
        else
		{
            myScene.WrongButton();
            GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f);
		}
    }

    public void ResetTimers()
	{
        downTime = 0f;
        upTime = 0f;
        upTime2 = 0f;
	}
}
