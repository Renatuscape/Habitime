using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateAdventurerUi : MonoBehaviour
{
    public TextMeshProUGUI templateTitle;
    public TextMeshProUGUI templateText;
    public TextMeshProUGUI templateDescription;
    public TextMeshProUGUI templateNumbers;
    public Button btPrevTemplate;
    public Button btNextTemplate;
    public Button btCreate;
    public Button btCancel;
    public TMP_InputField nameInput;
    public AdventurerTemplate selectedTemplate;

    private void Awake()
    {
        btCancel.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            CanvasController.LoadClock(DataTools.playerData.activeClock);
        });
        btPrevTemplate.onClick.AddListener(() => OnSwapTemplate(false));
        btNextTemplate.onClick.AddListener(() => OnSwapTemplate(true));
        btCreate.onClick.AddListener(() => CreateAdventurer());
    }
    public void Initialise()
    {
        gameObject.SetActive(true);
        selectedTemplate = Codex.adventurerTemplates[0];
    }

    public void CreateAdventurer()
    {
        ClockData clock = DataTools.playerData?.activeClock;

        if (clock != null)
        {
            AdventurerData adventurer = new() { name = nameInput.text, template = selectedTemplate };
            clock.activeAdventurer = DataTools.FinishAndSaveAdventurer(clock, adventurer);

            CanvasController.LoadClock(DataTools.playerData.activeClock);
            gameObject.SetActive(false);
        }
    }

    void PrintTemplate(AdventurerTemplate template)
    {
        templateText.text = template.name;
    }

    public void OnSwapTemplate(bool isRight)
    {
        int indexMod = isRight ? 1 : -1;
        int currentIndex = Array.IndexOf(Codex.adventurerTemplates, selectedTemplate);

        int count = DataTools.playerData.watches.Count;
        int newIndex = (currentIndex + indexMod + count) % count;

        selectedTemplate = Codex.adventurerTemplates[newIndex];
        PrintTemplate(selectedTemplate);
    }
}
