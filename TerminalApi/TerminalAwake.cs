using HarmonyLib;

namespace TerminalApi
{
	/// <summary>
	/// A harmony patch that runs code after the Awake method on <see cref="Terminal"/>
	/// </summary>
	[HarmonyPatch(typeof(Terminal), "Awake")]
	public static class TerminalAwake
	{
		public static void Postfix(ref Terminal __instance)
		{
			TerminalApi.Terminal = __instance;
			if (TerminalApi.QueuedActions.Count > 0)
			{
				TerminalApi.plugin.Log.LogMessage($"In game, now adding words.");
				foreach (DelayedAction delayedAction in TerminalApi.QueuedActions)
				{
					delayedAction.Run();
				}
				TerminalApi.QueuedActions.Clear();
			}
		}
	}
}
