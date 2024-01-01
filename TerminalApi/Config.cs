using BepInEx.Configuration;

namespace TerminalApi
{
    public partial class Plugin
    {
        internal static ConfigEntry<bool> configEnableLogs;

        internal void SetupConfig()
        {
            configEnableLogs = Config.Bind("General", "enableLogs", true, "Whether or not to display logs pertaining to TerminalAPI. Will still log that the mod has loaded.");
        }
    }
}
