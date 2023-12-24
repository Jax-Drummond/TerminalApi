using System;
using TerminalApi.Classes;
using TerminalApi.Interfaces;

namespace TerminalApi
{
	/// <summary>
	/// Allows information to be stored so that it can be run later.
	/// </summary>
	internal class DelayedAddTerminalKeyword : IDelayedAction
	{
		internal Action<TerminalKeyword, CommandInfo> Action{ get; set; }
		internal CommandInfo CommandInfo { get; set; } = null;
		internal TerminalKeyword Keyword { get; set; }
        public void Run()
        {
            Action(Keyword, CommandInfo);
        }
    }
}
