using HarmonyLib;

namespace TerminalApi.Events
{

    /// <summary>
    /// A harmony patch that runs code before the <see cref="Terminal.BeginUsingTerminal"/> method on <see cref="Terminal"/>
    /// </summary>
    [HarmonyPatch(typeof(Terminal))]
    public static partial class Events
	{
        [HarmonyPatch("BeginUsingTerminal")]
        [HarmonyPrefix]
		public static void OnBeginUsing(ref Terminal __instance)
        { 
            TerminalBeginUsing?.Invoke((object)__instance, new() { Terminal = __instance} );
        }
    }
}
