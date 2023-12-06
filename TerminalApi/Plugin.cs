using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;


namespace TerminalApi
{
	[BepInPlugin("atomic.terminalapi", "Terminal Api", "1.3.0")]
	public class Plugin : BaseUnityPlugin
	{
		public ManualLogSource Log = new ManualLogSource("Terminal Api");
		private void Awake()
		{
			Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
			BepInEx.Logging.Logger.Sources.Add( Log );
			TerminalApi.plugin = this;

			Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
			
		}
	}
}