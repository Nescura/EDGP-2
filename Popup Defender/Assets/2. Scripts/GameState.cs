using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameCurrentState { START, END, PAUSED }

public class GameState : MonoBehaviour
{
    public GameCurrentState state;

    public bool savedAlrd;

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
                if (GetComponent<GameDataManager>().data.dayCleared < GetComponent<Levels>().clearedLevel)
                {
                    GetComponent<GameDataManager>().data.dayCleared = GetComponent<Levels>().clearedLevel;
                    GetComponent<GameDataManager>().Save();
                    savedAlrd = true;
                }
            }
        }
    }
}