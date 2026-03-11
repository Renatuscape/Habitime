using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    public static CanvasController instance;
    public BackgroundUi backgroundUi;   // Bottom level
    public AdventurerUi adventurerUi;   // Lower mid level.
    public ClockUi clockUi;             // Upper mid level. Click-through to adventurer
    public MenuUi menuUi;               // Upper level. Click-through to menu button
    public CreateClockUi createClockUi; // Top level. No click-through
    public CreateAdventurerUi createAdventurerUi;
    public Button btCreateAdventurer;
    private void Awake()
    {
        instance = this;
        btCreateAdventurer.onClick.AddListener(() => OpenCreateAdventurer());
    }
    public void Initialise()
    {
        if (DataTools.playerData.activeClock != null)
        {
            adventurerUi.gameObject.SetActive(false);
            createClockUi.gameObject.SetActive(false);
            menuUi.gameObject.SetActive(false);
            LoadClockUi();
            //adventurerUi.CheckCurrentAdventurer();
        }
        else
        {
            OpenCreateClock();
        }
    }

    public void BtnToggleMenu()
    {
        menuUi.ToggleMenu(!menuUi.gameObject.activeInHierarchy);
    }

    public void OpenCreateAdventurer()
    {
        btCreateAdventurer.gameObject.SetActive(false);
        createClockUi.gameObject.SetActive(false);
        menuUi.gameObject.SetActive(false);
        clockUi.gameObject.SetActive(false);
        adventurerUi.gameObject.SetActive(false);
        createAdventurerUi.Initialise();
    }

    void LoadClockUi()
    {
        clockUi.gameObject.SetActive(true);
        createAdventurerUi.gameObject.SetActive(false);
        clockUi.Initialise();

        if (DataTools.playerData.disableAdventureMode)
        {
            adventurerUi.gameObject.SetActive(false);
        }
        else
        {
            CheckAdventurer();
        }
    }

    void OpenCreateClock()
    {
        createClockUi.gameObject.SetActive(true);
        menuUi.gameObject.SetActive(false);
        clockUi.gameObject.SetActive(false);
        adventurerUi.gameObject.SetActive(false);
    }

    void CheckAdventurer()
    {
        var clock = DataTools.playerData.activeClock;

        if (clock != null)
        {
            if (clock.adventurers.Count < 1)
            {
                clock.activeAdventurer = null;
                adventurerUi.gameObject.SetActive(false);
                btCreateAdventurer.gameObject.SetActive(true);
            }
            else
            {
                if (clock.activeAdventurer == null)
                {
                    clock.activeAdventurer = clock.adventurers.FirstOrDefault((a) => !a.isDead && !a.isMaxed) ?? clock.adventurers.FirstOrDefault((a) => !a.isDead);

                    if (clock.activeAdventurer == null)
                    {
                        adventurerUi.gameObject.SetActive(false);
                        btCreateAdventurer.gameObject.SetActive(true);
                        return;
                    }
                }

                adventurerUi.gameObject.SetActive(true);
                adventurerUi.LoadAdventurer(DataTools.playerData.activeClock.activeAdventurer);

                if (AdventureTools.GetLevel(clock.activeAdventurer) >= clock.activeAdventurer.template.maxLevel)
                {
                    if (!clock.activeAdventurer.isMaxed)
                    {
                        clock.activeAdventurer.isMaxed = true;
                        clock.activeAdventurer.endTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                            }
                    btCreateAdventurer.gameObject.SetActive(true);
                }
                else
                {
                    btCreateAdventurer.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            btCreateAdventurer.gameObject.SetActive(false);
        }
    }

    public static void LoadClock(ClockData clock)
    {
        DataTools.playerData.activeClock = clock;
        instance.LoadClockUi();
    }

    public static void OpenCreateClockMenu()
    {
        instance.OpenCreateClock();
    }
}
