using HarmonyLib;

namespace TerminalApi.Events
{

    /// <summary>
    /// A harmony patch that runs code after the <see cref="Terminal.BeginUsingTerminal"/> method on <see cref="Terminal"/>
    /// </summary>
    [HarmonyPatch(typeof(Terminal))]
    public static partial class Events
	{
        [HarmonyPatch("BeginUsingTerminal")]
        [HarmonyPostfix]
		public static void BeganUsing(ref Terminal __instance)
        { 
            TerminalBeganUsing?.Invoke((object)__instance, new() { Terminal = __instance} );
        }
    }
}
