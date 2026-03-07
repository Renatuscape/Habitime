using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AdventurerUi : MonoBehaviour
{
    public TextMeshProUGUI levelText;
    bool isActive;

    private void OnEnable()
    {
        StartCoroutine(AdventureChecker());
    }

    IEnumerator AdventureChecker()
    {
        while (true)
        {

            if (!isActive && MainController.activeWatch != null && levelText != null)
            {
                isActive = true;
                StartCoroutine(UpdateLoop());
                break;
            }
            else if (!isActive)
            {
                isActive = false;
            }

            yield return new WaitForSeconds(1.00f);
        }
    }

    IEnumerator UpdateLoop()
    {
        while (true)
        {
            levelText.text = "Lv. " + AdventureTools.GetLevel(MainController.activeWatch.GetTime());
            yield return new WaitForSeconds(1.00f);
        }
    }
}
