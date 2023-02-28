using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    private GameState myState;

    public GameObject myPauseBttn;
    public GameObject myResumeBttn;

    private void Start()
    {
        myState = GetComponent<GameState>();
    }

    public void PauseClicked()
    {
        myState.state = GameCurrentState.PAUSED;
        myResumeBttn.SetActive(true);
        myPauseBttn.SetActive(false);
    }

    public void ResumeClicked()
    {
        myState.state = GameCurrentState.START;
        myResumeBttn.SetActive(false);
        myPauseBttn.SetActive(true);
    }
}
