using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AdventureTools
{
    public const double BASE_XP = 450000.0;
    public const double MAX_LV_XP = 259200000.0;
    public const int MAX_LEVEL = 100;
    public static double GROWTH;
    public static double STEP;

    private static long ApplyBonus(long ms)
    {
        if (DataTools.playerData?.activeAdventurer != null)
            ms += DataTools.playerData.activeAdventurer.bonusXP;
        return ms;
    }

    private static int GetLevelRaw(long ms)
    {
        if (ms <= 0) return 0;
        int level = 0;
        while (level < MAX_LEVEL && ms >= XpForLevel(level + 1))
            level++;
        return level;
    }

    public static int GetLevel(long ms)
    {
        return GetLevelRaw(ApplyBonus(ms));
    }

    public static double XpForLevel(int level)
    {
        return BASE_XP * level + STEP * (level * (level - 1) / 2.0);
    }

    public static float GetLevelProgress(long ms)
    {
        ms = ApplyBonus(ms);
        int level = GetLevelRaw(ms);
        if (level >= MAX_LEVEL) return 1f;
        double currentLevelXp = XpForLevel(level);
        double nextLevelXp = XpForLevel(level + 1);
        return (float)((ms - currentLevelXp) / (nextLevelXp - currentLevelXp));
    }

    public static double SolveStep()
    {
        double low = 0, high = MAX_LV_XP;
        for (int i = 0; i < 1000; i++)
        {
            double mid = (low + high) / 2;
            STEP = mid;
            if (XpForLevel(MAX_LEVEL) < MAX_LV_XP) low = mid;
            else high = mid;
        }
        return (low + high) / 2;
    }
}