using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StopwatchData
{
    public StopwatchData(int id, string name)
    {
        this.id = id;
        this.name = name;
    }

    public int id;
    public string name;
    public bool hasStarted;
    public long startTimestamp;
    public long endTimestamp;

    public void StartTimer()
    {
        if (!hasStarted)
        {
            startTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            hasStarted = true;
        }
    }

    public void StopTimer()
    {
        hasStarted = false;

        // Move to archive
        endTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        MainController.ArchiveStopwatch(this);
        endTimestamp = 0;
        id = MainController.playerData.watches.Count + MainController.playerData.watchArchive.Count;
    }

    public long GetTime()
    {
        if (!hasStarted) return endTimestamp - startTimestamp;
        return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - startTimestamp;
    }
}
