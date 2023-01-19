using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManagerTest : MonoBehaviour
{
    public KeyCode chosenKey;
    // Start is called before the first frame update
    void Start()
    {
        InputManager.GetInstance().PopulateList();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            chosenKey = InputManager.GetInstance().GenerateKey();
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            InputManager.GetInstance().ReturnKey(chosenKey);
        }
    }
}
