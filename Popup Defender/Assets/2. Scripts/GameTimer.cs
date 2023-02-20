using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    public float deadlineTimer;
    public float deadlinetimerSec;
    public Text myDeadLineTxt;

    private GameState myState;

    // Start is called before the first frame update
    void Start()
    {
        myState = this.GetComponent<GameState>();       
    }

    // Update is called once per frame
    void Update()
    {
        DeadlineTimerFunctions();
    }

    void DeadlineTimerFunctions()
    {
        //as long as game's state is not END or Paused, the deadline timer will continue to tick down
        if (myState.state == GameCurrentState.START)
        {
            if (deadlinetimerSec > 15)
            {
                deadlineTimer += 1;
                deadlinetimerSec = 0;
            }
            else if (deadlinetimerSec < 15)
            {
                deadlinetimerSec += Time.deltaTime;
            }

            myDeadLineTxt.text = "23:" + (int)deadlineTimer;
        }

        //once deadline timer reaches 0, game state would be set to END
        if (deadlineTimer > 59)
        {
            myState.state = GameCurrentState.END;
            myDeadLineTxt.text = "00:00";
        }
    }

    public void ResetTimer()
    {
        deadlinetimerSec = 0;
        deadlineTimer = 50;
        myDeadLineTxt.text = "23:" + (int)deadlineTimer;
    }
}
