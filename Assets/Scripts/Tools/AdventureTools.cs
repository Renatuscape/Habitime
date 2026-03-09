using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class AdventureTools
{
    public static double BASE_XP = 450000.0;
    public static double MAX_LV_XP = 259200000.0;
    public static int MAX_LEVEL = 100;
    public static double GROWTH;
    public static double STEP;

    private static long ApplyBonus(long ms)
    {
        if (DataTools.playerData?.activeAdventurer != null)
            ms += DataTools.playerData.activeAdventurer.bonusXP;
        return ms;
    }

    private static int GetLevelRaw(long ms, AdventurerData adventurer)
    {
        if (ms <= 0) return 0;
        int level = 0;
        while (level < MAX_LEVEL && ms >= XpForLevel(level + 1, adventurer))
            level++;
        return level;
    }

    public static void GetXpRange(AdventurerData adventurer){
        AdventurerTemplate template;
        
        if (Codex.adventurerTemplates == null || Codex.adventurerTemplates.Length < 1){
            template = new(){xpBase = 450000, xpCap = 259200000.0};
        }
        else if (adventurer == null){
            template = Codex.adventurerTemplates[0];
        }
        else{
        template = Codex.adventurerTemplates.FirstOrDefault((t) => t.id == adventurer.templateId);
        }

        MAX_LV_XP = template.xpCap;
        BASE_XP = template.xpBase;
    }

    public static int GetLevel(long ms, AdventurerData adventurer)
    {
        GetXpRange(adventurer);
        return GetLevelRaw(ApplyBonus(ms), adventurer);
    }

    public static double XpForLevel(int level, AdventurerData adventurer)
    {
        GetXpRange(adventurer);
        return BASE_XP * level + STEP * (level * (level - 1) / 2.0);
    }

    public static float GetLevelProgress(long ms, AdventurerData adventurer)
    {
        ms = ApplyBonus(ms);
        int level = GetLevelRaw(ms, adventurer);
        if (level >= MAX_LEVEL) return 1f;
        double currentLevelXp = XpForLevel(level, adventurer);
        double nextLevelXp = XpForLevel(level + 1, adventurer);
        return (float)((ms - currentLevelXp) / (nextLevelXp - currentLevelXp));
    }

    public static double SolveStep(AdventurerData adventurer)
    {
        GetXpRange(adventurer);
        double low = 0, high = MAX_LV_XP;
        for (int i = 0; i < 1000; i++)
        {
            double mid = (low + high) / 2;
            STEP = mid;
            if (XpForLevel(MAX_LEVEL, adventurer) < MAX_LV_XP) low = mid;
            else high = mid;
        }
        return (low + high) / 2;
    }
}