using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{
    // Test data
    public static MainController main;
    public Notification notification;
    public PlayerData dummyData;
    public CanvasController canvasController;

    private void Awake()
    {
        main = GetComponent<MainController>();
        //AdventureTools.GROWTH = AdventureTools.SolveGrowthRate();
    }

    void Start()
    {
        DataTools.LoadData();
        dummyData = DataTools.playerData;

        canvasController.Initialise();
    }
    void OnApplicationPause(bool paused)
    {
        if (paused) // save your data here
        {
            DataTools.SaveData();
            notification.SendStopwatchNotification(TimeTools.FormatNotification(ClockTools.GetTime(DataTools.playerData.activeClock), DataTools.playerData.activeClock.name));
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