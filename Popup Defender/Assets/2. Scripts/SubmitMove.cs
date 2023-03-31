using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmitMove : MonoBehaviour
{
    public float speedX, speedY;

    // Update is called once per frame
    void Update()
    {
        // Preventing Panel from moving too far offscreen
        BoundaryCheck();

        // Moving the panels on their own
        MoveMe(speedX, speedY);
    }

    public void BoundaryCheck() // Prevents the button from moving too far away from offscreen
    {
        float moveX = transform.position.x;
        float moveY = transform.position.y;

        Vector2 bdZero = new Vector2(-7.5f, -3.7f);
        Vector2 bdOne = new Vector2(7.5f, 2.3f);

        // If Panel is out of the screen from the left
        if (moveX > bdOne.x)
            moveX = Mathf.Lerp(moveX, bdOne.x, 0.02f);
        else if (transform.position.x < bdZero.x)
            moveX = Mathf.Lerp(moveX, bdZero.x, 0.02f);

        if (moveY > bdOne.y)
            moveY = Mathf.Lerp(moveY, bdOne.y, 0.02f);
        else if (transform.position.y < bdZero.y)
            moveY = Mathf.Lerp(moveY, bdZero.y, 0.02f);

        //Debug.Log(playerPosX + ", " + playerPosY);
        transform.position = new Vector2(moveX, moveY);
    }

    public void MoveMe(float x, float y) // Forcefully button the panel based on inputs
    {
        Vector2 bdZero = new Vector2(-7.5f, -3.7f);
        Vector2 bdOne = new Vector2(7.5f, 2.3f);

        if (transform.position.x > bdOne.x) speedX = -Mathf.Abs(speedX);
        else if (transform.position.x < bdZero.x) speedX = Mathf.Abs(speedX);

        if (transform.position.y > bdOne.y) speedY = -Mathf.Abs(speedY);
        else if (transform.position.y < bdZero.y) speedY = Mathf.Abs(speedY);

        transform.position += new Vector3(x * Time.deltaTime, y * Time.deltaTime, 0);
    }

    public void SetSpeed(float x, float y)
    {
        speedX = x; speedY = y;
    }
}
