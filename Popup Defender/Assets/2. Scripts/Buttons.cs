using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour
{
    private Scene myScene;

    private void Start()
    {
        myScene = GameObject.Find("Scene").GetComponent<Scene>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.name == "Browser Bttn" && myScene.canInteract == true)
                {
                    myScene.BrowserButton();
                }
                else if (hit.collider.gameObject.name == "Website Bttn" && myScene.canInteract == true)
                {
                    myScene.WebsiteButton();
                }
                else if (hit.collider.gameObject.name == "Submit Bttn" && myScene.canInteract == true)
                {
                    myScene.SubmitButton();
                }
            }
        }
    }
}
