using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum MenuState { INTRO, MENU, GAME }

public class MainMenuTransistion : MonoBehaviour
{
    public GameObject myCamera;
    private MenuState state;

    [Header("Game Title Variables")]
    [SerializeField] private float randomX;
    [SerializeField] private float randomY;

    [SerializeField] private float newX;
    [SerializeField] private float newY;

    public float speed;
    public Vector2 newPos;

    [Header ("Blinking Text Variables")]
    public bool blinked;
    public TMP_Text textComponent;

    [Header("MainMenu Variables")]
    public Image dpLogo;
    public float showDP;
    public GameObject myProfile;
    public GameObject myBG;
    public bool playGame;
    public float playGlitch;
    public TMP_Text highscoreTxt;
    private GameData myData;

    // Start is called before the first frame update
    void Start()
    {
        RandomizeX();
        RandomizeY();

        newPos = new Vector2(newX, newY);
        
        state = MenuState.INTRO;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == MenuState.INTRO)
        {
            if (showDP > 0)
            {
                showDP -= Time.deltaTime;
                BlinkingTxt();
            }
            else
            {
                var color = dpLogo.color;
                color.a -= Time.deltaTime;
                dpLogo.color = color;
                textComponent.color = color;

                if (color.a <= 0)
                {
                    GetComponent<SpriteRenderer>().enabled = true;
                    state = MenuState.MENU;
                }
            }
        }
        else if (state == MenuState.MENU)
        {
            if (GetComponent<SpriteRenderer>().enabled == true)
            {
                BlinkingTxt();
                textComponent.text = "Press any key to continue";
                gameObject.transform.position = Vector2.MoveTowards(transform.position, newPos, speed * Time.deltaTime);

                if (Input.anyKeyDown)
                {
                    GetComponent<BoxCollider2D>().enabled = false;
                    GetComponent<SpriteRenderer>().enabled = false;
                    textComponent.text = "";
                    myProfile.SetActive(true);
                    myBG.GetComponent<SpriteRenderer>().color = new Color(0, 0.151f, 0.202f);
                    myData = GameObject.Find("Scene").GetComponent<GameDataManager>().data;
                    highscoreTxt.enabled = true;
                    highscoreTxt.text = "System Message: Days Gone Without A Crash - " + myData.dayCleared + " Days";
                }
            }

            if (playGame == true)
            {
                FindObjectOfType<AudioManager>().Play("Glitch");
                playGlitch -= Time.deltaTime;
                GameObject.Find("Main Camera").GetComponent<GlitchEffect>().glitch = true;

                if (playGlitch <= 0)
                {
                    GameObject.Find("Scene").GetComponent<Scene>().Play();
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.name == "Collider 1")
        {
            RandomizeY();
            newX = 3.64f;
        }
        else if (collision.gameObject.name == "Collider 2")
        {
            RandomizeY();
            newX = -3.64f;
        }
        else if(collision.gameObject.name == "Collider 3")
        {
            newY = -3.42f;
            RandomizeX();
        }
        else if (collision.gameObject.name == "Collider 4")
        {
            newY = 3.44f;
            RandomizeX();
        }

        float r = Random.Range(0.3f, 1f);
        float g = Random.Range(0.3f, 1f);
        float b = Random.Range(0.3f, 1f);

        gameObject.GetComponent<SpriteRenderer>().color = new Color(r, g, b);

        newPos = new Vector2(newX, newY);
    }

    void RandomizeX()
    {
        randomX = (int)Random.Range(1, 4);

        if (randomX == 1 || randomX == 3)
        {
            newX = 3.64f;
        }
        else if (randomX == 2 || randomX == 4)
        {
            newX = -3.64f;
        }
    }

    void RandomizeY()
    {
        randomY = (int)Random.Range(1, 5);

        if (randomY == 1 || randomY == 3)
        {
            newY = 3.44f;
        }
        else if (randomY == 2 || randomY == 4)
        {
            newY = -3.42f;
        }
    }

    void BlinkingTxt()
    {
        var color = textComponent.color;

        if (blinked == false)
        {
            color.a -= Time.deltaTime;
            textComponent.color = color;

            if (color.a <= 0)
            {
                blinked = true;
            }
        }
        else if (blinked == true)
        {
            color.a += Time.deltaTime;
            textComponent.color = color;

            if (color.a >= 1)
            {
                blinked = false;
            }
        }
    }
}
