using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class PlayerData
{
    public List<ClockData> watches = new();
    public List<ClockArchiveData> watchArchive = new();
    public List<AdventurerData> adventurers = new();
    public bool disableAdventureMode;
}
