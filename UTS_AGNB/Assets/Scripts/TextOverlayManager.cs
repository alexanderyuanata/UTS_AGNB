using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TextOverlayManager : MonoBehaviour
{
    public TextMeshProUGUI textMeshProUGUI;

    private void Start()
    {
    }

    private IEnumerator temporaryText(string text, float duration)
    {
        //set text
        textMeshProUGUI.text = text;

        //start countdown
        yield return new WaitForSeconds(duration);

        //return back to invisible
        textMeshProUGUI.text = "";
    }

    public void startTextOverlay(string text)
    {
        StartCoroutine(temporaryText(text, 2f));
    }

}
