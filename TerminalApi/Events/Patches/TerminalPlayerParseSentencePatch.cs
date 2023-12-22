using HarmonyLib;
using System.Linq;

namespace TerminalApi.Events
{

    /// <summary>
    /// A harmony patch that runs code after the ParsePlayerSentence method on <see cref="Terminal"/>
    /// </summary>
    [HarmonyPatch(typeof(Terminal))]
    public static partial class Events
	{
        [HarmonyPatch("ParsePlayerSentence")]
        [HarmonyPostfix]
		public static void ParsePlayerSentence(ref Terminal __instance, TerminalNode __result)
        {
            Command command = TerminalApi.Commands.FirstOrDefault(c => c.TerminalNode == __result);
            if(command != null && command.DisplayTextSupplier != null)
            {
                __result.displayText = command.DisplayTextSupplier();
            }
            string submittedText = __instance.screenText.text.Substring(__instance.screenText.text.Length - __instance.textAdded);
            TerminalParsedSentence?.Invoke((object)__instance, new() { Terminal = __instance, SubmittedText = submittedText, ReturnedNode = __result });
        }
    }
}
