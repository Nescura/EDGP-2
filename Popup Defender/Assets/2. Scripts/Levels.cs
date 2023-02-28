using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levels : MonoBehaviour
{
    public int currentLevel;

    private GameObject myController;

    private void Start()
    {
        myController = GameObject.Find("GameCuntroller");
    }

    public void CheckCurrentLevel()
    {
        currentLevel += 1;

        if (currentLevel % 2 == 0)
        {
            //max number of pop-ups +1
            if (myController.GetComponent<GameControlling>().spawnMax < 20)
            {
                myController.GetComponent<GameControlling>().spawnMax += 1;
            }
        }
        else
        {
            //mini game timer -1 seconds
            if (myController.GetComponent<GameControlling>().minigameTimer > 5f)
            {
                myController.GetComponent<GameControlling>().minigameTimer -= 1;
            }
        }
    }
}
