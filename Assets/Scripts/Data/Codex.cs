public static class Codex
{
    public static AdventurerTemplate[] adventurerTemplates = new[] {new AdventurerTemplate()
    {
        id = 0,
        name = "Hero",
        xpBase = 450000.0,      // 30 seconds until lv 1
        xpCap = 259200000.0,    // Three days until lv 100
    }, new AdventurerTemplate(){
        id = 1,
        name = "Commoner",
        xpBase = 150000.0,      // ?? seconds until lv 1
        xpCap = 86400000.0,    // One day until lv 100
    } };
}