

namespace TerminalApi.Events
{
	public static partial class Events
	{
		/// <summary>
		/// Runs when the terminal is fully awake.
		/// Returns the <see cref="Terminal"/>
		/// </summary>
		public static event TerminalEventHandler TerminalAwake;
		/// <summary>
		/// Runs when the terminal starts waking up.
		/// Returns the <see cref="Terminal"/>
		/// </summary>
		public static event TerminalEventHandler TerminalWaking;
		/// <summary>
		/// Runs when the terminal has fully started.
		/// Returns the <see cref="Terminal"/>
		/// </summary>
		public static event TerminalEventHandler TerminalStarted;
		/// <summary>
		/// Runs when the terminal starts.
		/// Returns the <see cref="Terminal"/>
		/// </summary>
		public static event TerminalEventHandler TerminalStarting;
		/// <summary>
		/// Runs when the player begins using the <see cref="Terminal"/>
		/// Returns the <see cref="Terminal"/>
		/// </summary>
		public static event TerminalEventHandler TerminalBeginUsing;
		/// <summary>
		/// Runs after the <see cref="Terminal.BeginUsingTerminal"/> method
		/// Returns the <see cref="Terminal"/>
		/// </summary>
		public static event TerminalEventHandler TerminalBeganUsing;
		/// <summary>
		/// Runs when the user submits a command.
		/// Returns the <see cref="Terminal"/>, the text command submitted, and the returned <see cref="TerminalNode"/>
		/// </summary>
		public static event TerminalParseSentenceEventHandler TerminalParsedSentence;
	}
}
