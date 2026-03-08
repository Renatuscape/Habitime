
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

    public static void CreateNewGameData()
    {
        playerData = new PlayerData();
    }

    public static void StoreClock(ClockData clock)
    {
        clock.id = playerData.watches.Count + playerData.watchArchive.Count;
        playerData.watches.Add(clock);
        SaveData();
    }

    public static AdventurerData CreateAdventurer(ClockData clock)
    {
        AdventurerData adventurer = new();
        adventurer.name = "Hero of " + clock.name;
        adventurer.clockId = clock.id;
        adventurer.templateId = Codex.adventurerTemplates[0].id;
        playerData.adventurers.Add(adventurer);

        if (playerData.activeClock.id == clock.id)
        {
            playerData.activeAdventurer = adventurer;
        }

        return adventurer;
    }

    public static void LoadData()
    {
        string json = PlayerPrefs.GetString("PlayerData", "");

        if (json is null || json == "")
        {
            CreateNewGameData();
        }
        else
        {
            playerData = JsonUtility.FromJson<PlayerData>(json);
            Debug.Log(json);
        }
    }
}