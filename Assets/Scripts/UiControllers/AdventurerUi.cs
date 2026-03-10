using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdventurerUi : MonoBehaviour
{
    public TextMeshProUGUI adventurerLevel;
    public TextMeshProUGUI adventurerName;
    public Button btAdventurer;
    public Slider xpProgress;
    public AdventurerData adventurer;
    bool isActive;
    bool isClickCooldown;

    private void OnEnable()
    {
        StartCoroutine(AdventureChecker());
    }

    public void LoadAdventurer(AdventurerData adventurer)
    {
        this.adventurer = adventurer;
        adventurerName.text = adventurer.name;
        StopAllCoroutines();
        //StartCoroutine(AdventureChecker());

        if (DataTools.playerData != null && DataTools.playerData.disableAdventureMode)
        {
            gameObject.SetActive(false);
        }
        else if (DataTools.playerData?.activeClock != null && adventurerLevel != null)
        {
            CheckCurrentAdventurer();
            StartCoroutine(UpdateLoop());

            if (btAdventurer != null)
            {
                btAdventurer.onClick.RemoveAllListeners();
                btAdventurer.onClick.AddListener(() => ClickAdventurer());
            }

        }
    }

    public void ClickAdventurer()
    {
        if (!isClickCooldown)
        {
            isClickCooldown = true;
            if (DataTools.playerData.activeClock.hasStarted
                && !DataTools.playerData.activeClock.activeAdventurer.isMaxed
                && !DataTools.playerData.activeClock.activeAdventurer.isDead)
            {
                AnimatorTool.JumpObject(btAdventurer.gameObject, 5, 0.2f);
                if (adventurer != null)
                {
                    int xp = 10000;// - (195 * AdventureTools.GetLevel(adventurer));
                    adventurer.bonusXP += xp;
                    Debug.Log("XP added: " + xp + ". Total is " + AdventureTools.GetLevelProgress(adventurer));
                }
            }
        }

        StartCoroutine(ClickCooldown());
    }
    IEnumerator ClickCooldown()
    {
        yield return new WaitForSeconds(0.1f);
        isClickCooldown = false;
    }

    IEnumerator AdventureChecker()
    {
        Debug.Log("Starting Adventure Checker coroutine");
        while (true)
        {
            if (DataTools.playerData != null && DataTools.playerData.disableAdventureMode)
            {
                gameObject.SetActive(false);
            }
            else if (!isActive && DataTools.playerData?.activeClock != null && adventurerLevel != null)
            {
                CheckCurrentAdventurer();
                isActive = true;
                StartCoroutine(UpdateLoop());

                if (btAdventurer != null)
                {
                    btAdventurer.onClick.RemoveAllListeners();
                    btAdventurer.onClick.AddListener(() => ClickAdventurer());
                }

                break;
            }
            else if (!isActive)
            {
                isActive = false;
            }

            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator UpdateLoop()
    {
        Debug.Log("Starting adventurer Update Loop coroutine");
        string prevText = adventurerLevel.text;

        while (true)
        {
            CheckCurrentAdventurer();

            int level = AdventureTools.GetLevel(adventurer);
            adventurerLevel.text = "Lv. " + level;

            if (level >= adventurer.template.maxLevel)
            {
                xpProgress.gameObject.SetActive(false);
            }

            if (adventurerLevel.text != prevText)
            {
                AnimatorTool.JumpObject(adventurerLevel.gameObject, 30, 0.1f);
                AnimatorTool.JumpObject(xpProgress.gameObject, 20, 0.05f);
                prevText = adventurerLevel.text;
            }

            if (DataTools.playerData.activeClock.hasStarted
            && !adventurer.isDead
            && level < adventurer.template.maxLevel)
            {
                xpProgress.value = AdventureTools.GetLevelProgress(adventurer);
                xpProgress.gameObject.SetActive(true);
            }
            else
            {
                xpProgress.gameObject.SetActive(false);
            }

            yield return new WaitForSeconds(0.20f);
        }
    }


    public void CheckCurrentAdventurer()
    {
        if (adventurer == null || DataTools.playerData.activeClock.id != adventurer.clockId)
        {
            AdventurerData matchingAdventurer = DataTools.playerData.activeClock.adventurers.FirstOrDefault(a => a.clockId == DataTools.playerData.activeClock.id);

            if (matchingAdventurer != null)
            {
                adventurer = matchingAdventurer;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        else if (DataTools.playerData.activeClock.id == adventurer.clockId
            && DataTools.playerData.activeClock.life <= 0)
        {
            adventurer.isDead = true;
        }
    }

    // Gesture - swipe left or right
    // Scroll adventurers
}
