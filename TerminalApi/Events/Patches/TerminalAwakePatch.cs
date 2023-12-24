using HarmonyLib;
using TerminalApi.Interfaces;

namespace TerminalApi.Events
{

    /// <summary>
    /// A harmony patch that runs code after the Awake method on <see cref="Terminal"/>
    /// </summary>
    [HarmonyPatch(typeof(Terminal))]
    public static partial class Events
	{
        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
		public static void Awake(ref Terminal __instance)
        {
            TerminalApi.Terminal = __instance;
            if (TerminalApi.QueuedDelayedActions.Count > 0)
            {
				TerminalApi.plugin.Log?.LogMessage($"In game, applying any changes now.");
                foreach (IDelayedAction delayedAction in TerminalApi.QueuedDelayedActions)
                {
                    delayedAction.Run();
                }
				TerminalApi.QueuedDelayedActions.Clear();
            }
            TerminalAwake?.Invoke((object)__instance, new() { Terminal = __instance} );
        }
    }
}
