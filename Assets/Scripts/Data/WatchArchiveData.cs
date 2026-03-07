using System;

[Serializable]
public class WatchArchiveData
{
    public WatchArchiveData(StopwatchData watchData)
    {
        id = watchData.id;
        name = watchData.name;
        startTimestamp = watchData.startTimestamp;
        endTimestamp = watchData.endTimestamp;
    }

    public int id;
    public string name;
    public long startTimestamp;
    public long endTimestamp;
}

