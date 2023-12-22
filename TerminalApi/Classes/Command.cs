using System;
using TerminalApi.Classes;

namespace TerminalApi
{
    public class Command
    {
        /// <summary>
        /// The word that triggers loading of the node
        /// </summary>
        public string CommandWord { get; set; }
        /// <summary>
        /// The category and description
        /// </summary>
        public CommandInfo CommandInfo { get; set; } = new();
        /// <summary>
        /// A verb word to go along with command. Like 'run trap', trap being the command and run being the verb
        /// </summary>
        public string VerbWord { get; set; } = null;
        /// <summary>
        /// Whether to clear the console on loading node
        /// </summary>
        public bool ClearPreviousText { get; set; } = true;
        /// <summary>
        /// A function that should return the string that is wanted to be displayed
        /// </summary>
        public Func<string> DisplayTextSupplier { get; set; }

        public TerminalNode TerminalNode { get; internal set; }

        public Command(string word, Func<string> displayTextSupplier)
        {
            CommandWord = word;
            DisplayTextSupplier = displayTextSupplier;
            CommandInfo.Name = CommandWord.Substring(0, 1).ToUpper() + CommandWord.Substring(1);
        }
        public Command() 
        {
            CommandInfo.Name = CommandWord.Substring(0, 1).ToUpper() + CommandWord.Substring(1);
        }
    }
}
