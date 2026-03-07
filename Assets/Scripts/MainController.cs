using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class MainController : MonoBehaviour
{
    // Test data
    public static MainController main;
    public Notification notification;
    public PlayerData dummyData;
    public ClockData stopwatch;
    public ClockUi stopwatchUi;

    private void Awake()
    {
        main = GetComponent<MainController>();
        AdventureTools.GROWTH = AdventureTools.SolveGrowthRate();
    }

    void Start()
    {
        DataTools.LoadData();
        dummyData = DataTools.playerData;
        stopwatch = DataTools.playerData.watches[0];

        stopwatchUi.Initialise(stopwatch);
    }
    void OnApplicationPause(bool paused)
    {
        if (paused) // save your data here
        {
            DataTools.SaveData();
            notification.SendStopwatchNotification(TimeTools.FormatNotification(ClockTools.GetTime(stopwatch), stopwatch.name));
        }
}

    void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus) // save your data here
        {
            DataTools.SaveData();
        }
}
}