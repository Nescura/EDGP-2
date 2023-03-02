using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    // Tams here, sorry for changing the names of the variables, it's mostly to avoid confusion ^^;7
    public int systemTimer; // Was deadlineTimer before, and was a float
    public float virusTimer = 15f; // Was deadlinetimerSec before
    public TMPro.TextMeshProUGUI myDeadLineTxt;
    public Image myVirusTimePie;

    private GameState myState;

    // UI & Animation Stuff
    private float virusTimerLerp;

    // Start is called before the first frame update
    void Start()
    {
        myState = this.GetComponent<GameState>();
        systemTimer = 50;
        virusTimer = 15f; // at some point we'd probably need to make this scalable for stages
        virusTimerLerp = virusTimer;
    }

    // Update is called once per frame
    void Update()
    {
        DeadlineTimerFunctions();

        //systemTimer.
    }

    void DeadlineTimerFunctions()
    {
        //as long as game's state is not END or Paused, the deadline timer will continue to tick down
        if (myState.state == GameCurrentState.START)
        {
            // changed a lot: now only adds system time when the DISPLAY virus time hits
            // NOTE: InverseLerp gives a value between 0 to 1, allowing us to basically check if the display virus time is at least 0.05% close to the actual virus time value
            // and if so, add system timer by one and repeat the virus timer process again - this is the "lose a life" scenario
            if (Mathf.InverseLerp(0, 15f, virusTimerLerp) < 0.0005f)
            { 
                systemTimer += 1;
                virusTimer = 15;
                virusTimerLerp = virusTimer;
            }

            // virus time ticks to 0 ALWAYS if there are minigames on screen
            if (virusTimer > 0 && GameControlling.GetInstance().GetActivePanelCount() > 0)
            {
                virusTimer -= Time.deltaTime * 2; //previous virus timer goes down to slow
            }

            // virus timer display stuff
            virusTimerLerp = Mathf.Lerp(virusTimerLerp, virusTimer, Time.deltaTime * 2);
            myVirusTimePie.fillAmount = Mathf.InverseLerp(0, 15f, virusTimerLerp);

            myDeadLineTxt.text = "23:" + string.Format("{0:D2}", (int)systemTimer);
        }

        //once deadline timer reaches 0, game state would be set to END
        if (systemTimer > 59)
        {
            myState.state = GameCurrentState.END;
            myDeadLineTxt.text = "00:00";
        }
    }

    public void ResetSystemTimer() // Was ResetTimer() before
    {
        virusTimer = 15f; virusTimerLerp = virusTimer;
        systemTimer = 50;
        myDeadLineTxt.text = "23:" + string.Format("{0}:D2", (int)systemTimer);
    }

    public void AddVirusTimer(float timeToAdd)
	{
        virusTimer = Mathf.Clamp(virusTimer + timeToAdd, 0f, 15.1f); // Value can also be negative to reduce virus time, if needed
	}
}
