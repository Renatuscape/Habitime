using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class PlayerData
{
    public List<ClockData> watches = new();
    public List<ClockArchiveData> watchArchive = new();
    public List<AdventurerData> adventurers = new();
    public ClockData activeClock;
    public AdventurerData activeAdventurer;
    public bool disableAdventureMode;
}
