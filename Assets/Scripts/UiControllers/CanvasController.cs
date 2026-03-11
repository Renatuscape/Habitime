using System;
using System.Linq;
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
    public AdventurerArchiveUi adventurerArchiveUi;
    public Button btCreateAdventurer;
    public Button btPromoteAdventurer;
    public Button btAdventurerArchive;
    public Button btSettings;
    public Button btMenu;
    private void Awake()
    {
        instance = this;
        btCreateAdventurer.onClick.AddListener(() => OpenCreateAdventurer());
        btMenu.onClick.AddListener(() => BtnToggleMenu());
        btAdventurerArchive.onClick.AddListener(() => ToggleAdventureArchiveAndLoad());
    }
    public void Initialise()
    {
        HideAllUiElements();

        if (DataTools.playerData.activeClock != null)
        {
            LoadClockUi();
        }
        else
        {
            OpenCreateClock();
        }
    }

    public void BtnToggleMenu()
    {
        menuUi.ToggleMenu(!menuUi.gameObject.activeInHierarchy);

        if (menuUi.gameObject.activeInHierarchy)
        {
            btCreateAdventurer.gameObject.SetActive(false);
        }
        else
        {
            LoadClockUi();
        }
    }

    public void OpenCreateAdventurer()
    {
        HideAllUiElements();
        createAdventurerUi.Initialise();
    }

    void LoadClockUi()
    {
        HideAllUiElements();
        clockUi.gameObject.SetActive(true);
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
        HideAllUiElements();
        createClockUi.gameObject.SetActive(true);
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
                TryEnableAdventurerButtons();
            }
            else
            {
                if (clock.activeAdventurer == null)
                {
                    clock.activeAdventurer = clock.adventurers.FirstOrDefault((a) => !a.isDead && !a.isMaxed) ?? clock.adventurers.FirstOrDefault((a) => !a.isDead);

                    if (clock.activeAdventurer == null)
                    {
                        adventurerUi.gameObject.SetActive(false);
                        TryEnableAdventurerButtons();
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
                    TryEnableAdventurerButtons();
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

    void TryEnableAdventurerButtons()
    {
        if (menuUi.gameObject.activeInHierarchy)
        {
            btCreateAdventurer.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Toggle method turned on create adventurer button. Menu was active: " + menuUi.gameObject.activeInHierarchy);
            btCreateAdventurer.gameObject.SetActive(true);

            // Check if promotion button should be available here
        }
    }

    void ToggleAdventureArchiveAndLoad()
    {
        if (adventurerArchiveUi.gameObject.activeInHierarchy)
        {
            LoadClock(DataTools.playerData.activeClock);
        }
        else
        {
            HideAllUiElements();
            adventurerArchiveUi.Initialise(DataTools.playerData.activeClock);
        }
    }

    void HideAllUiElements()
    {
        btPromoteAdventurer.gameObject.SetActive(false);
        btCreateAdventurer.gameObject.SetActive(false);
        adventurerArchiveUi.gameObject.SetActive(false);
        adventurerUi.gameObject.SetActive(false);
        createClockUi.gameObject.SetActive(false);
        clockUi.gameObject.SetActive(false);
        menuUi.gameObject.SetActive(false);
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

    public static void ToggleAdventureArchive()
    {
        instance.ToggleAdventureArchiveAndLoad();
    }
}
