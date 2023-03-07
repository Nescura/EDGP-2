using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AutoText : MonoBehaviour
{
    public static void TypeText(TMP_Text textElement, string text, float time)
    {
        float characterDelay = time / text.Length;
        textElement.StartCoroutine(SetText(textElement, text, characterDelay));
    }

    static IEnumerator SetText(TMP_Text textElement, string text, float characterDelay)
    {
        for (int i = 0; i < text.Length; i++)
        {
            textElement.text += text[i];
            yield return new WaitForSeconds(characterDelay);
        }
    }
}
