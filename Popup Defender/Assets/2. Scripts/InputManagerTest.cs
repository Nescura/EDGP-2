using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManagerTest : MonoBehaviour
{
    public KeyCode chosenKey;
    InputManager inputManager;

    // Start is called before the first frame update
    void Start()
    {
        inputManager = new InputManager();
        //InputManager.GetInstance().GenerateKey();
        inputManager.PopulateList();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            chosenKey = inputManager.GenerateKey(); //InputManager.GetInstance().GenerateKey();
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            inputManager.ReturnKey(chosenKey);
            //InputManager.GetInstance().ReturnKey(chosenKey);
        }
    }
}
