using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdventurerUi : MonoBehaviour
{
    public TextMeshProUGUI levelText;
    public Button btAdventurer;
    bool isActive;

    private void OnEnable()
    {
            StartCoroutine(AdventureChecker());
    }

    public void ClickAdventurer()
    {
        AnimatorTool.JumpObject(btAdventurer.gameObject, 5, 0.2f);
    }

    IEnumerator AdventureChecker()
    {
        while (true)
        {
            if (DataTools.playerData != null && DataTools.playerData.disableAdventureMode)
            {
                gameObject.SetActive(false);
            }
            else if (!isActive && DataTools.activeClock != null && levelText != null)
            {
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
            levelText.text = "Lv. " + AdventureTools.GetLevel(ClockTools.GetTime(DataTools.activeClock));
            if (levelText.text != prevText)
            {
                AnimatorTool.JumpObject(levelText.gameObject, 30, 0.1f);
                prevText = levelText.text;
            }
            yield return new WaitForSeconds(1.00f);
        }
    }
}
