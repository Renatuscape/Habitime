using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ClockData
{
    public ClockData(int id, string name)
    {
        this.id = id;
        this.name = name;
    }

    public int id;
    public string name;
    public bool hasStarted;
    public long startTimestamp;
    public long endTimestamp;
}
