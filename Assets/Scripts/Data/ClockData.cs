using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ClockData
{
    public int id;
    public int life = 1;
    public string name;
    public bool hasStarted;
    public bool isSimple = true;
    public long startTimestamp;
    public long endTimestamp;
    public List<Goal> goals = new();
    public List<AdventurerData> adventurers = new();
    public AdventurerData activeAdventurer;
}

[Serializable]
public class Goal
{
    public string text;
}