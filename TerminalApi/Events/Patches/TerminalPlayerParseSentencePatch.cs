using HarmonyLib;
using System.Linq;
using TerminalApi.Classes;

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
            CommandInfo commandInfo = TerminalApi.CommandInfos.FirstOrDefault(cI => cI.TriggerNode == __result);

            // Calls callback function, if there is one
            if (commandInfo != null)
            {
                __result.displayText = commandInfo?.DisplayTextSupplier();
            }
           
            string submittedText = __instance.screenText.text.Substring(__instance.screenText.text.Length - __instance.textAdded);
            TerminalParsedSentence?.Invoke((object)__instance, new() { Terminal = __instance, SubmittedText = submittedText, ReturnedNode = __result });
        }
    }
}
