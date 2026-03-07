using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClockUi : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI buttonText;
    public Button button;
    ClockData clockData;

    public void Initialise(ClockData clockData)
    {
        this.clockData = clockData;

        if (clockData != null)
        {
            nameText.text = clockData.name;

            if (clockData.hasStarted)
            {
                buttonText.text = "Stop";
                StartCoroutine(UpdateLoop());
            }
            else
            {
                buttonText.text = "Start";
                timeText.text = "00:00:00";
            }
        }

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => ButtonClick());
    }

    public void ButtonClick()
    {
        if (clockData != null && clockData.hasStarted)
        {
            StopAllCoroutines();
            ClockTools.StopTimer(clockData);
            buttonText.text = "Start";
            timeText.text = "00:00:00";
        }
        else if (clockData != null && !clockData.hasStarted)
        {
            ClockTools.StartTimer(clockData);
            buttonText.text = "Stop";
            StartCoroutine(UpdateLoop());
        }
    }

    IEnumerator UpdateLoop()
    {
        while (true)
        {
            if (timeText != null && clockData != null)
            {
                timeText.text = TimeTools.FormatElapsed(ClockTools.GetTime(clockData));
            }

            yield return new WaitForSeconds(0.01f);
        }
    }
}
