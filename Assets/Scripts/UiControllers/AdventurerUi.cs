using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdventurerUi : MonoBehaviour
{
    public TextMeshProUGUI levelText;
    public Button btAdventurer;
    public Slider xpProgress;
    bool isActive;

    private void OnEnable()
    {
        StartCoroutine(AdventureChecker());
    }

    public void ClickAdventurer()
    {
        if (DataTools.playerData.activeClock.hasStarted)
        {
            AnimatorTool.JumpObject(btAdventurer.gameObject, 5, 0.2f);
            if (DataTools.playerData?.activeAdventurer != null)
            {
                int xp = 10000 - (100 * AdventureTools.GetLevel(ClockTools.GetTime(DataTools.playerData.activeClock)));
                DataTools.playerData.activeAdventurer.bonusXP += xp;
                Debug.Log("XP added: " + xp + ". Total is " + AdventureTools.GetLevelProgress(ClockTools.GetTime(DataTools.playerData.activeClock)));
            }
        }
    }

    IEnumerator AdventureChecker()
    {
        while (true)
        {
            if (DataTools.playerData != null && DataTools.playerData.disableAdventureMode)
            {
                gameObject.SetActive(false);
            }
            else if (!isActive && DataTools.playerData?.activeClock != null && levelText != null)
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
        string prevText = levelText.text;

        while (true)
        {
            CheckCurrentAdventurer();

            levelText.text = "Lv. " + AdventureTools.GetLevel(ClockTools.GetTime(DataTools.playerData.activeClock));

            if (levelText.text != prevText)
            {
                AnimatorTool.JumpObject(levelText.gameObject, 30, 0.1f);
                AnimatorTool.JumpObject(xpProgress.gameObject, 20, 0.05f);
                prevText = levelText.text;
            }

            if (DataTools.playerData.activeClock.hasStarted && !DataTools.playerData.activeAdventurer.isDead)
            {
                xpProgress.value = AdventureTools.GetLevelProgress(ClockTools.GetTime(DataTools.playerData.activeClock));
                xpProgress.gameObject.SetActive(true);
            }
            else
            {
                xpProgress.gameObject.SetActive(false);
            }

            yield return new WaitForSeconds(1.00f);
        }
    }


    public void CheckCurrentAdventurer()
    {
        if (DataTools.playerData.activeAdventurer == null || DataTools.playerData.activeClock.id != DataTools.playerData.activeAdventurer?.clockId)
        {
            AdventurerData matchingAdventurer = DataTools.playerData.adventurers.FirstOrDefault(a => a.clockId == DataTools.playerData.activeClock.id);

            if (matchingAdventurer != null)
            {
                DataTools.playerData.activeAdventurer = matchingAdventurer;
            }
            else
            {
                DataTools.playerData.activeAdventurer = DataTools.CreateAdventurer(DataTools.playerData.activeClock);
            }
        }
        else if (DataTools.playerData.activeClock.id == DataTools.playerData.activeAdventurer.clockId
            && DataTools.playerData.activeClock.life <= 0)
        {
            DataTools.playerData.activeAdventurer.isDead = true;
        }
    }
}
