using System;

[Serializable]
public class ClockArchiveData
{
    public ClockArchiveData(ClockData clockData)
    {
        id = clockData.id;
        name = clockData.name;
        startTimestamp = clockData.startTimestamp;
        endTimestamp = clockData.endTimestamp;
    }

    public int id;
    public string name;
    public long startTimestamp;
    public long endTimestamp;
}

