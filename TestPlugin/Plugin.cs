using BepInEx;
using static TerminalApi.Events.Events;
using static TerminalApi.TerminalApi;

namespace TestPlugin
{
	[BepInPlugin("atomic.testplugin", "Test Plugin", "1.0.0")]
	[BepInDependency("atomic.terminalapi")]
	public class Plugin : BaseUnityPlugin
	{
		private void Awake()
		{
			Logger.LogInfo("Plugin Test Plugin is loaded!");

			TerminalAwake +=  TerminalIsAwake;
			TerminalWaking += TerminalIsWaking;
			TerminalStarting += TerminalIsStarting;
			TerminalStarted += TerminalIsStarted;
			TerminalParsedSentence += TextSubmitted;

			// Will display 'World' when 'hello' is typed into the terminal
			AddCommand("hello", "World\n");

			// Will display 'Sorry but you cannot run kill' when 'run kill' is typed into the terminal
			// Will also display the same thing as above if you just type 'kill' into the terminal 
			// because the default verb will be 'run'
			AddCommand("kill", "Sorry but you cannot run kill\n", "run");

		}

		private void TerminalIsAwake(object sender, TerminalEventArgs e)
		{
			Logger.LogMessage("Terminal is awake");
		}

		private void TerminalIsWaking(object sender, TerminalEventArgs e)
		{
			Logger.LogMessage("Terminal is waking");
		}

		private void TerminalIsStarting(object sender, TerminalEventArgs e)
		{
			Logger.LogMessage("Terminal is starting");
		}

		private void TerminalIsStarted(object sender, TerminalEventArgs e)
		{
			Logger.LogMessage("Terminal is started");
		}

        private void TextSubmitted(object sender, TerminalParseSentenceEventArgs e)
        {
            Logger.LogMessage($"Text submitted: {e.SubmittedText} Node Returned: {e.ReturnedNode}");
        }

    }
}