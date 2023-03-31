using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Credits : MonoBehaviour
{
    public Animator credits;
    float timer;

    // Update is called once per frame
    void Update()
    {
        if (timer >= 17.8f)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            timer += Time.deltaTime;
        }

        if (Input.anyKey)
        {
            timer += Time.deltaTime * 2;
            credits.speed = 2.25f;
        }
        else
		{
            credits.speed = 0.75f;
		}
    }
}
