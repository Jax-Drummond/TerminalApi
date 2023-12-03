using HarmonyLib;

namespace TerminalApi.Events
{

    /// <summary>
    /// A harmony patch that runs code after the <see cref="Terminal.QuitTerminal"/> method on <see cref="Terminal"/>
    /// </summary>
    [HarmonyPatch(typeof(Terminal))]
    public static partial class Events
	{
        [HarmonyPatch("QuitTerminal")]
        [HarmonyPostfix]
		public static void OnQuitTerminal(ref Terminal __instance)
        {
            TerminalExited?.Invoke((object)__instance, new() { Terminal = __instance} );
        }
    }
}
