using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    public static CanvasController instance;
    public BackgroundUi backgroundUi;   // Bottom level
    public AdventurerUi adventurerUi;   // Lower mid level.
    public ClockUi clockUi;             // Upper mid level. Click-through to adventurer
    public MenuUi menuUi;               // Upper level. Click-through to menu button
    public CreateClockUi createClockUi; // Top level. No click-through
    private void Awake()
    {
        instance = this;
    }
    public void Initialise()
    {
        if (DataTools.playerData.activeClock != null)
        {
            createClockUi.gameObject.SetActive(false);
            menuUi.gameObject.SetActive(false);
            LoadClockUi();
            adventurerUi.CheckCurrentAdventurer();
        }
        else
        {
            OpenCreate();
        }
    }

    public void BtnToggleMenu()
    {
        menuUi.ToggleMenu(!menuUi.gameObject.activeInHierarchy);
    }

    void LoadClockUi()
    {
        clockUi.gameObject.SetActive(true);
        clockUi.Initialise();
        adventurerUi.gameObject.SetActive(!DataTools.playerData.disableAdventureMode);
        adventurerUi.CheckCurrentAdventurer();
    }

    void OpenCreate()
    {
        createClockUi.gameObject.SetActive(true);
        menuUi.gameObject.SetActive(false);
        clockUi.gameObject.SetActive(false);
        adventurerUi.gameObject.SetActive(false);
    }

    public static void LoadClock(ClockData clock)
    {
        DataTools.playerData.activeClock = clock;
        instance.LoadClockUi();
    }

    public static void OpenCreateMenu()
    {
        instance.OpenCreate();
    }
}
