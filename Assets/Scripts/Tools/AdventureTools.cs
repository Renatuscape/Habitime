using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AdventureTools
{
    public const double BASE_XP = 450000.0; // XP to reach level one
    public const double MAX_LV_XP = 259200000.0; // Three days worth of XP
    public const int MAX_LEVEL = 100;
    public static double GROWTH; //= 1.1248;

    public static int GetLevel(long ms)
    {
        if (ms <= 0) return 0;
        // inverse of the exponential formula
        int level = (int)(Math.Log((ms / BASE_XP) + 1) / Math.Log(GROWTH));
        return Math.Min(level, MAX_LEVEL);
    }

    public static double XpForLevel(int level)
    {
        return BASE_XP * (Math.Pow(GROWTH, level) - 1);
    }

    public static float GetLevelProgress(long ms)
    {
        int level = GetLevel(ms);
        if (level >= MAX_LEVEL) return 1f;

        double currentLevelXp = XpForLevel(level);
        double nextLevelXp = XpForLevel(level + 1);

        return (float)((ms - currentLevelXp) / (nextLevelXp - currentLevelXp));
    }

    public static double SolveGrowthRate()
    {
        double low = 1.0, high = 2.0;
        for (int i = 0; i < 1000; i++)
        {
            double mid = (low + high) / 2;
            double total = BASE_XP * (Math.Pow(mid, MAX_LEVEL) - 1);
            if (total < MAX_LV_XP) low = mid;
            else high = mid;
        }
        return (low + high) / 2;
    }
}
