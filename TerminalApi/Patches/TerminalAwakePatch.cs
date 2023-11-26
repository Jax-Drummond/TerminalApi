using HarmonyLib;
using TerminalApi.Events;

namespace TerminalApi.Patches
{

    /// <summary>
    /// A harmony patch that runs code after the Awake method on <see cref="Terminal"/>
    /// </summary>
    [HarmonyPatch(typeof(Terminal), "Awake")]
    public static class TerminalAwakePatch
    {
		public static event TerminalAwakeEventHandler TerminalAwake;
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
            TerminalAwake?.Invoke((object)__instance, new() { Terminal = __instance} );
        }
    }
}
