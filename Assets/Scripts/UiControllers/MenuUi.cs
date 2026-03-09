using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuUi : MonoBehaviour
{
    public TMP_InputField clockNameInput;
    public Button btLeftClock;
    public Button btRightClock;
    public Button btCreate;

    private void Awake()
    {
        btCreate.onClick.RemoveAllListeners();
        btCreate.onClick.AddListener(() => CreateNewClock());

        btRightClock.onClick.RemoveAllListeners();
        btRightClock.onClick.AddListener(() => OnSwapClock(true));

        btLeftClock.onClick.RemoveAllListeners();
        btLeftClock.onClick.AddListener(() => OnSwapClock(false));
    }

    public void ToggleMenu(bool isOpening)
    {
        if (DataTools.playerData?.activeClock != null)
        {
            if (isOpening)
            {
                gameObject.SetActive(true);
                Debug.Log("Setting field to clock name " + DataTools.playerData.activeClock.name);
                clockNameInput.text = DataTools.playerData.activeClock.name;

                if (DataTools.playerData?.watches?.Count > 1)
                {
                    btLeftClock.gameObject.SetActive(true);
                    btRightClock.gameObject.SetActive(true);
                }
                else
                {
                    btLeftClock.gameObject.SetActive(false);
                    btRightClock.gameObject.SetActive(false);
                }
            }
            else
            {
                Debug.Log("Setting clock name to " + clockNameInput.text);
                DataTools.playerData.activeClock.name = clockNameInput.text;
                DataTools.SaveData();
                gameObject.SetActive(false);
            }
        }
    }

    public void OnSwapClock(bool isRight)
    {
        int indexMod = isRight ? 1 : -1;
        int currentIndex = DataTools.playerData.watches.IndexOf(DataTools.playerData.activeClock);

        int count = DataTools.playerData.watches.Count;
        int newIndex = (currentIndex + indexMod + count) % count;

        CanvasController.LoadClock(DataTools.playerData.watches[newIndex]);
        ToggleMenu(true);
    }

    public void CreateNewClock()
    {
        CanvasController.OpenCreateMenu();
    }

    // Gesture - swipe down on menu
    // Close menu on swipe down

    // Gesture - swipe left or right
    // Change to next clock
}
