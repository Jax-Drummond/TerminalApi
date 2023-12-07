using HarmonyLib;

namespace TerminalApi.Events
{

    /// <summary>
    /// A harmony patch that runs code after the <see cref="Terminal.TextChanged(string)"/> method on <see cref="Terminal"/>
    /// </summary>
    [HarmonyPatch(typeof(Terminal))]
    public static partial class Events
	{
        [HarmonyPatch("TextChanged")]
        [HarmonyPostfix]
		public static void OnTextChanged(ref Terminal __instance, string newText)
        {
            string currentInputText= "";
            if (newText.Trim().Length >= __instance.textAdded)
            {
                currentInputText = newText.Substring(newText.Length - __instance.textAdded);
            }
            TerminalTextChanged?.Invoke((object)__instance, new() { Terminal = __instance,  NewText = newText, CurrentInputText = currentInputText } );
        }
    }
}
