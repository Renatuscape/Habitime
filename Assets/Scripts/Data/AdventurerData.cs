using System;

[Serializable]
public class AdventurerData
{
    public string id;      // Unique ID
    public int clockId; // Relevant clock
    public AdventurerTemplate template;
    public string name;
    public bool isDead;
    public bool isMaxed;
    public int bonusXP;
    public int prestige; // Times advanced
    public long startTimestamp;
    public long endTimestamp;
}
