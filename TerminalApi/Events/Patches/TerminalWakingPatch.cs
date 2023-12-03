using HarmonyLib;

namespace TerminalApi.Events
{

    /// <summary>
    /// A harmony patch that runs code before the Awake method on <see cref="Terminal"/>
    /// </summary>
    [HarmonyPatch(typeof(Terminal))]
    public static partial class Events
	{
        [HarmonyPatch("Awake")]
        [HarmonyPrefix]
		public static void Waking(ref Terminal __instance)
        { 
            TerminalWaking?.Invoke((object)__instance, new() { Terminal = __instance } );
        }
    }
}
