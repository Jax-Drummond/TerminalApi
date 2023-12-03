using System;

namespace TerminalApi
{
	/// <summary>
	/// Allows information to be stored so that it can be run later.
	/// </summary>
	internal class DelayedAction
	{
		internal Action<TerminalKeyword> Action{ get; set; }
		internal TerminalKeyword Keyword { get; set; }

		internal void Run()
		{
			Action(Keyword);
		}
	}
}
