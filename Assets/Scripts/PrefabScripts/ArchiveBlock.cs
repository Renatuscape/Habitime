using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ArchiveBlock : MonoBehaviour
{
    public List<SpriteContainer> spriteContainers;

    public List<AdventurerData> Initialise(List<AdventurerData> adventurers)
    {
        List<AdventurerData> leftovers = new();

        int i;

        for (i = 0; i < adventurers.Count; i++)
        {
            if (i >= spriteContainers.Count)
            {
                leftovers.Add(adventurers[i]);
            }
            else
            {
                SetUpSprite(adventurers[i], spriteContainers[i]);
            }
        }

        // Disable any unpopulated sprite containers
        int currentContainer = i++;

        if (currentContainer < spriteContainers.Count)
        {
            for (i = currentContainer; i < spriteContainers.Count; i++)
            {
                SetUpSprite(null, spriteContainers[i]);
            }
        }
        return leftovers;
    }

    void SetUpSprite(AdventurerData adventurer, SpriteContainer sprite)
    {
        if (adventurer != null)
        {
            // Set sprite appearance here
            sprite.sprite.SetActive(true);

            sprite.name.text = adventurer.name;
            sprite.name.gameObject.SetActive(true);
        }
        else
        {
            sprite.sprite.SetActive(false);
            sprite.name.gameObject.SetActive(false);
        }
    }
}

[Serializable]
public class SpriteContainer
{
    public GameObject sprite;
    public TextMeshProUGUI name;
}