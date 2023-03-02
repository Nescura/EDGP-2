using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levels : MonoBehaviour
{
    public int currentLevel;

    public void CheckCurrentLevel()
    {
        currentLevel += 1;

        if (currentLevel % 2 == 0)
        {
            //max number of pop-ups +1
            if (GameControlling.GetInstance().spawnMax < 20)
            {
                GameControlling.GetInstance().spawnMax += 2;
            }
        }
        else
        {
            //mini game timer -1 seconds
            if (GameControlling.GetInstance().minigameTimer > 5f)
            {
                GameControlling.GetInstance().minigameTimer -= 1;
            }
        }
    }
}
