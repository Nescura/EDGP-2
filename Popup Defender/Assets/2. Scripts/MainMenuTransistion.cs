using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuTransistion : MonoBehaviour
{

    [SerializeField] private float randomX;
    [SerializeField] private float randomY;

    [SerializeField] private float newX;
    [SerializeField] private float newY;

    public float speed;
    public Vector2 newPos;

    // Start is called before the first frame update
    void Start()
    {
        RandomizeX();
        RandomizeY();

        newPos = new Vector2(newX, newY);
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.position = Vector2.MoveTowards(transform.position, newPos, speed * Time.deltaTime);
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
}
