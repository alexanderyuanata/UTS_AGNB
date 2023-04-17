using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private Image timersprite;

    private void Start()
    {
        timersprite = GetComponent<Image>();
    }

    public void updateTimer(float current, float max)
    {
        timersprite.fillAmount = current / max;
    }
}
