using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameCurrentState { START, END, PAUSED }

public class GameState : MonoBehaviour
{
    public GameCurrentState state;

    private bool savedAlrd;

    // Start is called before the first frame update
    void Start()
    {
        state = GameCurrentState.START;
    }

    private void Update()
    {
        if (state == GameCurrentState.END)
        {
            if (savedAlrd != true)
            {
                GetComponent<GameDataManager>().data.dayCleared = GetComponent<Levels>().clearedLevel;
                GetComponent<GameDataManager>().Save();
                savedAlrd = true;
            }
        }
    }
}