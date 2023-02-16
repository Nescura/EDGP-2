using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameCurrentState { START, END, PAUSED }

public class GameState : MonoBehaviour
{
    public GameCurrentState state;

    // Start is called before the first frame update
    void Start()
    {
        state = GameCurrentState.START;
    }
}