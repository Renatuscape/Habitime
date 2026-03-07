using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public List<StopwatchData> watches = new();
    public List<WatchArchiveData> watchArchive = new();
}
