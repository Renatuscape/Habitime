using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class AdventureTools
{
    public static double GROWTH;
    public static double STEP;

    private static long ApplyAdjustment(long ms, AdventurerData adventurer)
    {
        if (adventurer != null)
        {
            if (adventurer.prestige > 0)
            {
                ms -= adventurer.prestige * (long)adventurer.template.maxLevel;
            }

            ms += adventurer.bonusXP;
        }
        return ms;
    }

    private static int GetLevelRaw(long ms, AdventurerData adventurer)
    {
        if (ms <= 0) return 0;
        int level = 0;
        while (level < adventurer.template.maxLevel && ms >= XpForLevel(level + 1, adventurer))
            level++;
        return level;
    }

    public static void GetXpRange(AdventurerData adventurer)
    {
        AdventurerTemplate template;

        if (Codex.adventurerTemplates == null || Codex.adventurerTemplates.Length < 1)
        {
            template = new() { xpBase = 450000, xpCap = 259200000.0 };
        }
        else if (adventurer == null || adventurer.template == null)
        {
            template = Codex.adventurerTemplates[0];
        }
        else
        {
            template = adventurer.template;
        }
    }

    public static int GetLevel(AdventurerData adventurer)
    {
        if (adventurer.isMaxed) { return (int)adventurer.template.maxLevel; }
        var ms = adventurer.isDead ? adventurer.endTimestamp - adventurer.startTimestamp : DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - adventurer.startTimestamp;
        GetXpRange(adventurer);
        return GetLevelRaw(ApplyAdjustment(ms, adventurer), adventurer);
    }

    public static double XpForLevel(int level, AdventurerData adventurer)
    {
        GetXpRange(adventurer);
        return adventurer.template.xpBase * level + STEP * (level * (level - 1) / 2.0);
    }

    public static float GetLevelProgress(AdventurerData adventurer)
    {
        var ms = ApplyAdjustment(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - adventurer.startTimestamp, adventurer);
        int level = GetLevelRaw(ms, adventurer);
        if (level >= adventurer.template.maxLevel) return 1f;
        double currentLevelXp = XpForLevel(level, adventurer);
        double nextLevelXp = XpForLevel(level + 1, adventurer);
        return (float)((ms - currentLevelXp) / (nextLevelXp - currentLevelXp));
    }

    public static double SolveStep(AdventurerData adventurer)
    {
        GetXpRange(adventurer);
        double low = 0, high = adventurer.template.xpCap;
        for (int i = 0; i < 1000; i++)
        {
            double mid = (low + high) / 2;
            STEP = mid;
            if (XpForLevel((int)adventurer.template.maxLevel, adventurer) < adventurer.template.xpCap) low = mid;
            else high = mid;
        }
        return (low + high) / 2;
    }
}