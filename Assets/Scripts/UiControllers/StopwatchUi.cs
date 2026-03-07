using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StopwatchUi : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI buttonText;
    public Button button;
    StopwatchData stopwatch;

    public void Initialise(StopwatchData stopwatch)
    {
        this.stopwatch = stopwatch;

        if (stopwatch != null)
        {
            nameText.text = stopwatch.name;

            if (stopwatch.hasStarted)
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
        Debug.Log("Button clicked");

        if (stopwatch != null && stopwatch.hasStarted)
        {
            StopAllCoroutines();
            stopwatch.StopTimer();
            buttonText.text = "Start";
            timeText.text = "00:00:00";
        }
        else if (stopwatch != null && !stopwatch.hasStarted)
        {
            stopwatch.StartTimer();
            buttonText.text = "Stop";
            StartCoroutine(UpdateLoop());
        }
    }

    IEnumerator UpdateLoop()
    {
        while (true)
        {
            if (timeText != null && stopwatch != null)
            {
                Debug.Log("Coroutine called");
                timeText.text = TimeHelper.FormatElapsed(stopwatch.GetTime());
            }

            yield return new WaitForSeconds(0.01f);
        }
    }
}
