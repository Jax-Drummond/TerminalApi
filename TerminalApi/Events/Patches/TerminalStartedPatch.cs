using HarmonyLib;

namespace TerminalApi.Events
{

    /// <summary>
    /// A harmony patch that runs code after the Start method on <see cref="Terminal"/>
    /// </summary>
    [HarmonyPatch(typeof(Terminal))]
    public static partial class Events
	{
        [HarmonyPatch("Start")]
        [HarmonyPostfix]
		public static void Started(ref Terminal __instance)
        { 
            TerminalStarted?.Invoke((object)__instance, new() { Terminal = __instance} );
        }
    }
}
