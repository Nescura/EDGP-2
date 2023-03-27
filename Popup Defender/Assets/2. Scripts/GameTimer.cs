using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    // Tams here, sorry for changing the names of the variables, it's mostly to avoid confusion ^^;7
    public int systemTimer; // Was deadlineTimer before, and was a float
    public float virusTimerInit = 10f, virusTimer; // Was deadlinetimerSec before
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
        virusTimerInit = 10f;
        virusTimer = virusTimerInit;
        virusTimerLerp = virusTimer;
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
            // changed a lot: now only adds system time when the DISPLAY virus time hits
            // NOTE: InverseLerp gives a value between 0 to 1, allowing us to basically check if the display virus time is at least 0.05% close to the actual virus time value
            // and if so, add system timer by one and repeat the virus timer process again - this is the "lose a life" scenario
            if (Mathf.InverseLerp(0, 15f, virusTimerLerp) < 0.0005f)
            {
                FindObjectOfType<AudioManager>().Play("Gong");
                FindObjectOfType<AudioManager>().SetPitch("Gong", 1 - ((systemTimer - 50) * 0.05f));
                systemTimer += 1;
                virusTimer = virusTimerInit;
                virusTimerLerp = virusTimer;
                myDeadLineTxt.rectTransform.localScale = new Vector3(1.5f, 1.5f, 1f);
                myDeadLineTxt.color = Color.red;
            }

            // virus time ticks to 0 ALWAYS if there are minigames on screen
            if (virusTimer > 0 && GameControlling.GetInstance().GetActivePanelCount() > 0)
            {
                virusTimer -= Time.deltaTime * 2; //previous virus timer goes down too slow // Counterpoint: Now it goes down *too fast*. Besides, aren't we making the virus timer decrease faster as the game goes on, and not already blindingly fast at the very beginning? I've added a thing for it in Levels.cs -tams
            }

            // virus timer display stuff
            virusTimerLerp = Mathf.Lerp(virusTimerLerp, virusTimer, Time.deltaTime);
            myVirusTimePie.fillAmount = Mathf.InverseLerp(0, virusTimerInit, virusTimerLerp);

            myDeadLineTxt.text = "23:" + string.Format("{0:D2}", (int)systemTimer);
            myDeadLineTxt.rectTransform.localScale = Vector3.Lerp(myDeadLineTxt.rectTransform.localScale, Vector3.one, Time.deltaTime);
            myDeadLineTxt.color = Color.Lerp(myDeadLineTxt.color, Color.white, Time.deltaTime);
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
        virusTimer = virusTimerInit; virusTimerLerp = virusTimer;
        systemTimer = 50;
        myDeadLineTxt.text = "23:" + string.Format("{0}:D2", (int)systemTimer);
    }

    public void AddVirusTimer(float timeToAdd)
	{
        virusTimer = Mathf.Clamp(virusTimer + timeToAdd, 0f, virusTimerInit + 0.1f); // Value can also be negative to reduce virus time, if needed
	}
}
