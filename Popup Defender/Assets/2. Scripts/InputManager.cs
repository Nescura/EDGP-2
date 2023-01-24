using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{
    public List<KeyCode> keyCodeList = new List<KeyCode>();

    private InputManager() { }
    private static InputManager instance;

    public static InputManager GetInstance()
    {
        if (instance == null)
        {
            instance = new InputManager();
        }
        return instance;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public KeyCode GenerateKey()
    {
        int tempNumber;
        KeyCode tempKeyCode;

        tempNumber = Random.Range(0, keyCodeList.Count);
        tempKeyCode = keyCodeList[tempNumber];
        keyCodeList.RemoveAt(tempNumber);
        return tempKeyCode;
    }

    public void ReturnKey(KeyCode x)
    {
        keyCodeList.Add(x);
    }

    // Get the entire list made (or reset)
    public void PopulateList()
    {
        keyCodeList.Clear();
        keyCodeList.Add(KeyCode.A);
        keyCodeList.Add(KeyCode.B);
        keyCodeList.Add(KeyCode.C);
        keyCodeList.Add(KeyCode.D);
        keyCodeList.Add(KeyCode.E);
        keyCodeList.Add(KeyCode.F);
        keyCodeList.Add(KeyCode.G);
        keyCodeList.Add(KeyCode.H);
        keyCodeList.Add(KeyCode.I);
        keyCodeList.Add(KeyCode.J);
        keyCodeList.Add(KeyCode.K);
        keyCodeList.Add(KeyCode.L);
        keyCodeList.Add(KeyCode.M);
        keyCodeList.Add(KeyCode.N);
        keyCodeList.Add(KeyCode.O);
        keyCodeList.Add(KeyCode.P);
        keyCodeList.Add(KeyCode.Q);
        keyCodeList.Add(KeyCode.R);
        keyCodeList.Add(KeyCode.S);
        keyCodeList.Add(KeyCode.T);
        keyCodeList.Add(KeyCode.U);
        keyCodeList.Add(KeyCode.V);
        keyCodeList.Add(KeyCode.W);
        keyCodeList.Add(KeyCode.X);
        keyCodeList.Add(KeyCode.Y);
        keyCodeList.Add(KeyCode.Z);
        keyCodeList.Add(KeyCode.Space);
    }
}
