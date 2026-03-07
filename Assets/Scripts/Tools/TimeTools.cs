public static class TimeTools
{
    public static string FormatElapsed(long ms)
    {
        long totalSeconds = ms / 1000;
        //long milliseconds = ms % 1000;
        long centiseconds = (ms % 1000) / 10;
        // then use centiseconds and :D2 everywhere instead of milliseconds/:D3
        // e.g. $"{minutes:D2}:{seconds:D2}.{centiseconds:D2}"
        long seconds = totalSeconds % 60;
        long minutes = (totalSeconds / 60) % 60;
        long hours = (totalSeconds / 3600) % 24;
        long days = (totalSeconds / 86400) % 365;
        long years = totalSeconds / 31536000;

        if (years > 0)
            return $"{years}y {days}d {hours:D2}:{minutes:D2}:{seconds:D2}";
        if (days > 0)
            return $"{days}d {hours:D2}:{minutes:D2}:{seconds:D2}";
        if (hours > 0)
            return $"{hours}:{minutes:D2}:{seconds:D2}";

        return $"{minutes:D2}:{seconds:D2}:{centiseconds:D2}";
    }

    public static string FormatNotification(long ms, string timerName)
    {
        long totalSeconds = ms / 1000;

        long minutes = (totalSeconds / 60) % 60;
        long hours = (totalSeconds / 3600) % 24;
        long days = (totalSeconds / 86400) % 365;
        long years = totalSeconds / 31536000;

        if (years > 0)
            return $"{years}y {days}d {hours:D2}h {minutes:D2}m {timerName}";
        if (days > 0)
            return $"{days}d {hours:D2}h {minutes:D2}m {timerName}";
        if (hours > 0)
            return $"{hours} hours {minutes:D2} minutes {timerName}";

        return $"{minutes:D2} minutes {timerName}";
    }
}