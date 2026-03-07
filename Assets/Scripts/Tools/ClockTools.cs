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
        clock.endTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        DataTools.ArchiveStopwatch(clock);
        clock.endTimestamp = 0;
        clock.id = DataTools.playerData.watches.Count + DataTools.playerData.watchArchive.Count;
    }

    public static long GetTime(ClockData clock)
    {
        if (!clock.hasStarted) return clock.endTimestamp - clock.startTimestamp;
        return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - clock.startTimestamp;
    }
}