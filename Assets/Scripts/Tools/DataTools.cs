using UnityEngine;

public static class DataTools
{
    public static PlayerData playerData;
    public static ClockData activeClock;
    public static AdventurerData activeAdventurer;
    public static void ArchiveStopwatch(ClockData watchData)
    {
        playerData.watchArchive.Add(new(watchData));
        SaveData();
    }

    public static void SaveData()
    {
        string json = PlayerPrefs.GetString("PlayerData", "");
        json = JsonUtility.ToJson(playerData);
        PlayerPrefs.SetString("PlayerData", json);
        PlayerPrefs.Save();
        Debug.Log(json);
    }

    public static ClockData CreateData()
    {
        playerData = new PlayerData();
        ClockData stopwatch = CreateTimer("Silence");
        return stopwatch;
    }

    public static ClockData CreateTimer(string name)
    {
        ClockData stopwatch = new(playerData.watches.Count + playerData.watchArchive.Count, name);
        playerData.watches.Add(stopwatch);

        SaveData();
        return stopwatch;
    }

    public static AdventurerData CreateAdventurer(ClockData stopwatch)
    {
        AdventurerData adventurer = new();
        adventurer.clockId = stopwatch.id;
        playerData.adventurers.Add(adventurer);

        return adventurer;
    }

    public static void LoadData()
    {
        string json = PlayerPrefs.GetString("PlayerData", "");

        if (json is null || json == "")
        {
            CreateData();
        }
        else
        {
            playerData = JsonUtility.FromJson<PlayerData>(json);
            Debug.Log(json);
        }

        activeClock = playerData.watches[0];
    }
}