using System;

public static class ClockTools
{
    public static void StartTimer(ClockData clock)
    {
        if (!clock.hasStarted)
        {
            clock.startTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            clock.hasStarted = true;
        }
    }

    public static void StopTimer(ClockData clock)
    {
        clock.hasStarted = false;

        // Move to archive
        clock.life = 0;
        clock.endTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        DataTools.ArchiveClock(clock);

        if (DataTools.playerData.watches.Count > 0)
        {
            CanvasController.LoadClock(DataTools.playerData.watches[0]);
        }
        else
        {
            DataTools.playerData.activeClock = null;
            CanvasController.OpenCreateClockMenu();
        }
    }

    public static long GetTime(ClockData clock)
    {
        if (!clock.hasStarted) return clock.endTimestamp - clock.startTimestamp;
        return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - clock.startTimestamp;
    }

    public static ClockData CreateClock(string name = "Timer", int life = 1)
    {
        ClockData clock = new();
        clock.name = name;
        clock.life = life;

        return clock;
    }
}