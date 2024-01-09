using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;


namespace TerminalApi
{
	[BepInPlugin("atomic.terminalapi", "Terminal Api", "1.5.1")]
	public partial class Plugin : BaseUnityPlugin
	{
		public ManualLogSource Log;
		private void Awake()
		{
			SetupConfig();
			Log = configEnableLogs.Value ? new ManualLogSource("Terminal Api") : null;
            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
			
			if (Log != null)
			{
				BepInEx.Logging.Logger.Sources.Add(Log);
			}

			TerminalApi.plugin = this;

			Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
			
		}
	}
}