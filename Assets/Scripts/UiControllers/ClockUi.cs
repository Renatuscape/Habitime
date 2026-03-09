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
    public ClockData clock;

    public void Initialise()
    {
        clock = DataTools.playerData.activeClock;

        if (clock != null && DataTools.playerData.watches.Count > 0)
        {
            button.gameObject.SetActive(true);
            nameText.text = clock.name;

            if (clock.hasStarted)
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
        else
        {
            button.gameObject.SetActive(false);
            timeText.text = "Create Clock to Start";
        }

            button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => ButtonClick());
    }

    public void ButtonClick()
    {
        if (clock != null && clock.hasStarted)
        {
            StopAllCoroutines();
            ClockTools.StopTimer(clock);
            buttonText.text = "Start";
            timeText.text = "00:00:00";
        }
        else if (clock != null && !clock.hasStarted)
        {
            ClockTools.StartTimer(clock);
            buttonText.text = "Stop";
            StartCoroutine(UpdateLoop());
        }
    }

    IEnumerator UpdateLoop()
    {
        while (true)
        {
            if (timeText != null && clock != null)
            {
                timeText.text = TimeTools.FormatElapsed(ClockTools.GetTime(clock));
            }

            yield return new WaitForSeconds(0.01f);
        }
    }

    // Gesture -  Swipe down on clock / Click on clock
    // Open clock edit screen
}
