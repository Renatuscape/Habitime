using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class MainController : MonoBehaviour
{
    // Test data
    public static MainController main;
    public static PlayerData playerData;
    public static StopwatchData activeWatch;
    public Notification notification;
    public PlayerData dummyData;
    public StopwatchData stopwatch;
    public StopwatchUi stopwatchUi;
    string json;

    private void Awake()
    {
        main = GetComponent<MainController>();
        AdventureTools.GROWTH = AdventureTools.SolveGrowthRate();
    }

    void Start()
    {
        json = PlayerPrefs.GetString("PlayerData", "");

        if (json is null || json == "")
        {
            playerData = new PlayerData();
            stopwatch = CreateTimer("Silence");
        }
        else
        {
            playerData = JsonUtility.FromJson<PlayerData>(json);
            stopwatch = playerData.watches[0];
            Debug.Log(json);
        }

        stopwatchUi.Initialise(stopwatch);

        activeWatch = stopwatch;

        dummyData = playerData;
    }
    void OnApplicationPause(bool paused)
    {
        if (paused) // save your data here
        {
            SaveData();
            notification.SendStopwatchNotification(TimeHelper.FormatNotification(stopwatch.GetTime(), stopwatch.name));
        }
}

    void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus) // save your data here
        {
            SaveData();
        }
}

    public StopwatchData CreateTimer(string name)
    {
        StopwatchData stopwatch = new(playerData.watches.Count + playerData.watchArchive.Count, name);
        playerData.watches.Add(stopwatch);

        SaveData();
        return stopwatch;
    }

    void SaveData()
    {
        json = JsonUtility.ToJson(playerData);
        PlayerPrefs.SetString("PlayerData", json);
        PlayerPrefs.Save();
        Debug.Log(json);
    }

    public static void ArchiveStopwatch(StopwatchData watchData)
    {
        playerData.watchArchive.Add(new(watchData));
        main.SaveData();
    }
}