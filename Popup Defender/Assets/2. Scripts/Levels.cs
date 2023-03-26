using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levels : MonoBehaviour
{
    public int currentLevel;
    public int clearedLevel;

    private void Start()
    {
        // Fresh start
        clearedLevel = -1;

        GameControlling.GetInstance().spawnMax = 1;
        GameControlling.GetInstance().spawnPanelTimeAvg = 10f;
        GameControlling.GetInstance().GetComponent<GameTimer>().virusTimerInit = 10f;
    }

    public void CheckCurrentLevel()
    {
        currentLevel += 1;

        // Every even level, increase max amount of panels on screen at a time by 1, caps at 20
        if (currentLevel % 2 == 0)
        {
            Mathf.Clamp(GameControlling.GetInstance().spawnMax += 1, 1, 20);

            /*if (GameControlling.GetInstance().spawnMax < 20)
            {
                GameControlling.GetInstance().spawnMax += 1;
            }*/
        }

        // Every odd level, decrease time interval between panel spawns
        // Was minigameTimer before - was established that minigame timers decreasing was a shaky idea, trying out sticking to spawn freq
        else
        {
            Mathf.Clamp(GameControlling.GetInstance().spawnPanelTimeAvg -= 1, 1f, 10f);

            //Mathf.Clamp(GameControlling.GetInstance().minigameTimer -= 1f, 5f, 10f); it's here if we decide to change it back

            /*if (GameControlling.GetInstance().minigameTimer > 5f)
            {
                GameControlling.GetInstance().minigameTimer -= 1;
            }*/
        }

        // Decrease virusTimer by 5% every day (which would decrease exponentially), caps at 1s
        Mathf.Clamp(GameControlling.GetInstance().GetComponent<GameTimer>().virusTimerInit *= 0.95f, 1f, 10f);
    }
}
