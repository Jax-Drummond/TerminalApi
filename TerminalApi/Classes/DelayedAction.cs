using System;
using TerminalApi.Classes;

namespace TerminalApi
{
	/// <summary>
	/// Allows information to be stored so that it can be run later.
	/// </summary>
	internal class DelayedAction
	{
		internal Action<TerminalKeyword, CommandInfo> Action{ get; set; }
		internal CommandInfo CommandInfo { get; set; } = null;
		internal TerminalKeyword Keyword { get; set; }

		internal void Run()
		{
			Action(Keyword, CommandInfo);
		}
	}
}
