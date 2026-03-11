
using System;
using System.Linq;
using UnityEngine;

public static class DataTools
{
    public static PlayerData playerData;
    public static void ArchiveClock(ClockData watchData)
    {
        playerData.watchArchive.Add(new(watchData));
        playerData.watches.Remove(watchData);

        SaveData();
    }

    public static void SaveData()
    {
        if (playerData.activeClock != null)
        {
            string json = PlayerPrefs.GetString("PlayerData", "");
            json = JsonUtility.ToJson(playerData);
            PlayerPrefs.SetString("PlayerData", json);
            PlayerPrefs.Save();
            Debug.Log(json);
        }
        else
        {
            Debug.Log("No data created, no data saved.");
        }
    }

    public static void StoreClock(ClockData clock)
    {
        clock.id = playerData.watches.Count + playerData.watchArchive.Count;
        foreach (var adventurer in clock.adventurers)
        {
            adventurer.clockId = clock.id;
        }

        playerData.watches.Add(clock);
        SaveData();
    }

    public static AdventurerData FinishAndSaveAdventurer(ClockData clock, AdventurerData adventurer)
    {
        if (adventurer.name.Length < 1)
        {
            adventurer.name = "Hero of " + clock.name;
        }
        if (adventurer.template == null || adventurer.template.name.Length < 1)
        {
            adventurer.template = Codex.adventurerTemplates[0];
        }

        adventurer.clockId = clock.id;

        string random = new string(Enumerable.Range(0, 3)
            .Select(_ => "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"[UnityEngine.Random.Range(0, 36)])
            .ToArray());

        adventurer.id = clock.id + "-" + clock.adventurers.Count + "-" + random;
        adventurer.startTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        clock.adventurers.Add(adventurer);

        return adventurer;
    }

    public static void LoadData()
    {
        string json = PlayerPrefs.GetString("PlayerData", "");

        if (json is null || json == "")
        {
            playerData = new PlayerData();
        }
        else
        {
            playerData = JsonUtility.FromJson<PlayerData>(json);
            Debug.Log(json);

            CheckDataIntegrity(playerData);
        }
    }

    static void CheckDataIntegrity(PlayerData playerData)
    {
        foreach (var clock in playerData.watches)
        {
            int i = 0;
            foreach (var adventurer in clock.adventurers)
            {
                if (adventurer.template == null || adventurer.template.name.Length < 1)
                {
                    Debug.Log("Adventurer missing template. Assigning default.");
                    adventurer.template = Codex.adventurerTemplates[0];
                }

                if (adventurer.template.maxLevel == 0
                    || adventurer.template.xpCap == 0
                    || adventurer.template.xpBase == 0)
                {
                    Debug.Log("Adventurer template data missing. Refreshing data.");
                    adventurer.template = Codex.adventurerTemplates.FirstOrDefault((t) => t.name == adventurer.template.name);
                }

                if (adventurer.startTimestamp == 0)
                {
                    Debug.Log("Adventurer timestamp missing. Setting starttime to current time.");
                    adventurer.startTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                }

                if ((adventurer.isDead || adventurer.isMaxed) && adventurer.endTimestamp == 0)
                {
                    Debug.Log("Adventurer timestamp missing. Setting endtime to current time.");
                    adventurer.endTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                }

                if (adventurer.id == null || adventurer.id.Length < 1)
                {
                    string random = new string(Enumerable.Range(0, 3)
                        .Select(_ => "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"[UnityEngine.Random.Range(0, 36)])
                        .ToArray());

                    adventurer.id = clock.id + "-" + i + "-" + random;
                }

                i++;
            }
        }
    }
}