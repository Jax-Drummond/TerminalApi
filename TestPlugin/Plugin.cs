using BepInEx;
using TerminalApi;
using TerminalApi.Classes;
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
			TerminalBeginUsing += OnBeginUsing;
			TerminalBeganUsing += BeganUsing;
			TerminalExited += OnTerminalExit;
            TerminalTextChanged += OnTerminalTextChanged;

			// Will display 'World' when 'hello' is typed into the terminal
			AddCommand("hello", "World\n");

			// Will display 'Sorry but you cannot run kill' when 'run kill' is typed into the terminal
			// Will also display the same thing as above if you just type 'kill' into the terminal 
			// because the default verb will be 'run'
			AddCommand("kill", "Sorry but you cannot run kill\n", "run");

			// All the code below is essentially the same as the line of code above
            TerminalNode triggerNode = CreateTerminalNode($"Frank is not available right now.\n", true);
            TerminalKeyword verbKeyword = CreateTerminalKeyword("get", true);
            TerminalKeyword nounKeyword = CreateTerminalKeyword("frank");

            verbKeyword = verbKeyword.AddCompatibleNoun(nounKeyword, triggerNode);
            nounKeyword.defaultVerb = verbKeyword;

            AddTerminalKeyword(verbKeyword);

			// The second parameter passed in is a CommandInfo, if you want to have a callback.
            AddTerminalKeyword(nounKeyword, new() { 
				TriggerNode = triggerNode,
				DisplayTextSupplier = () =>
				{
					Logger.LogWarning("Put code here, and it will run when trigger node is loaded");
					return "This text will display";
				},
				Category = "Other",
				Description = "This is just a test command."
				// The above would look like '>FRANK\nThis is just a test command.' in Other
			});

			// Adds a new command/terminal keyword that is 'pop' and a callback function that will run when the node of the keyword is loaded
			AddCommand("pop", new CommandInfo()
			{
				DisplayTextSupplier = () =>
				{
					Logger.LogWarning("Wowow, this ran.");
					return "popped\n\n";
				},
				Category = "Other"
			});

			// Or

			AddCommand("push", new CommandInfo()
			{
				DisplayTextSupplier = CommandFunction,
				Category = "Misc" // Does not work for categories that do not exist, yet ;)
			});
        }

		private string CommandFunction()
		{
			Logger.LogWarning("Code put here will run when terminal command is sent.");
			return "Wait, you cannot push\n\n";
		}

        private void OnTerminalTextChanged(object sender, TerminalTextChangedEventArgs e)
        {
			string userInput = GetTerminalInput();
			Logger.LogMessage(userInput);
			// Or
			Logger.LogMessage(e.CurrentInputText);

			// If user types in fuck it will changed to frick before they can even submit
			if(userInput == "fuck")
			{
				SetTerminalInput("frick");
			}
			
        }

        private void OnTerminalExit(object sender, TerminalEventArgs e)
        {
            Logger.LogMessage("Terminal Exited");
        }

        private void TerminalIsAwake(object sender, TerminalEventArgs e)
		{
			Logger.LogMessage("Terminal is awake");

			// Adds 'Hello' as a new line to the help node
			NodeAppendLine("help", "\nHello");
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

		private void OnBeginUsing(object sender, TerminalEventArgs e)
		{
            Logger.LogMessage("Player has just started using the terminal");
        }

        private void BeganUsing(object sender, TerminalEventArgs e)
        {
            Logger.LogMessage("Player is using terminal");
        }

    }
}