using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Credits : MonoBehaviour
{
    float timer;

    // Update is called once per frame
    void Update()
    {
        if (timer >= 16)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            timer += Time.deltaTime;
        }
    }
}
