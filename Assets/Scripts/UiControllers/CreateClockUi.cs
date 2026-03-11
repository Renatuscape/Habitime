using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateClockUi : MonoBehaviour
{
    public TMP_InputField nameInput;
    public Slider lifeSlider;
    public Toggle tgSimpleClock;
    public GameObject goals;
    public GameObject life;
    public Button btCreate;
    public Button btCancel;
    public List<TMP_InputField> inputGoals;

    private void OnEnable()
    {
        if (DataTools.playerData?.watches?.Count > 0)
        {
            btCancel.gameObject.SetActive(true);
        }
        else
        {
            btCancel.gameObject.SetActive(false);
        }
    }
    private void Awake()
    {
        btCreate.onClick.AddListener(() => OnCreate());
        btCancel.onClick.AddListener(() => OnCancel());
        tgSimpleClock.onValueChanged.AddListener(OnToggleSimple);
        tgSimpleClock.isOn = false;
        OnToggleSimple(false);

        if (DataTools.playerData?.watches?.Count > 0)
        {
            btCancel.gameObject.SetActive(true);
        }
        else
        {
            btCancel.gameObject.SetActive(false);
        }
    }
    public void OnCreate()
    {
        Debug.Log("OnCreate called. Attempting to create clock.");

        if (DataTools.playerData == null)
        {
            DataTools.playerData = new PlayerData();
        }

        var clock = ClockTools.CreateClock(nameInput.text, (int)lifeSlider.value);

        if (clock.name.Length <= 0)
        {
            clock.name = "Timer";
        }

        if (!tgSimpleClock.isOn)
        {
            foreach (var input in inputGoals)
            {
                if (input.text.Length > 0)
                {
                    clock.goals.Add(new() { text = input.text });
                }
            }

            clock.life = 1;
        }

        DataTools.StoreClock(clock);
        CanvasController.LoadClock(clock);
        gameObject.SetActive(false);
    }

    public void OnCancel()
    {
        CanvasController.LoadClock(DataTools.playerData.activeClock);
        gameObject.SetActive(false);
    }

    public void OnToggleSimple(bool isOn)
    {
        if (tgSimpleClock.isOn)
        {
            goals.SetActive(false);
            life.SetActive(false);
        }
        else
        {
            goals.SetActive(true);
            life.SetActive(true);
        }
    }
}
