using HarmonyLib;

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
            if (TerminalApi.QueuedActions.Count > 0)
            {
				TerminalApi.plugin.Log?.LogMessage($"In game, now adding words.");
                foreach (DelayedAction delayedAction in TerminalApi.QueuedActions)
                {
                    delayedAction.Run();
                }
				TerminalApi.QueuedActions.Clear();
            }
            TerminalAwake?.Invoke((object)__instance, new() { Terminal = __instance} );
        }
    }
}
