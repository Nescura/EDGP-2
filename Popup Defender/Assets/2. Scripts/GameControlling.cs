using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControlling : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        InputManager.GetInstance().PopulateList();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
