using System;

namespace TerminalApi.Events
{
	public static partial class Events
	{
		/// <summary>
		/// Triggers when Terminal is awake
		/// </summary>
		public class TerminalEventArgs : EventArgs
		{
			public Terminal Terminal { get; set; }
		}
		public delegate void TerminalAwakeEventHandler(Object sender, TerminalEventArgs e);

		/// <summary>
		/// Triggers when the terminal is waking up
		/// </summary>
		public delegate void TerminalWakingEventHandler(Object sender, TerminalEventArgs e);

		/// <summary>
		/// Triggers when the Terminal has started. Awake happens before start.
		/// </summary>
		public delegate void TerminalStartedEventHandler(Object sender, TerminalEventArgs e);

		/// <summary>
		/// Triggers when the Terminal is Starting. Waking happens before starting.
		/// </summary>
		public delegate void TerminalStartingEventHandler(Object sender, TerminalEventArgs e);

		public class TerminalParseSentenceEventArgs : EventArgs
		{
			public Terminal Terminal { get; set; }
			public TerminalKeyword ReturnedKeyword { get; set; } 
			public string SubmittedText { get; set; }
		}

		/// <summary>
		/// Triggers when the user sends a command in the terminal
		/// </summary>
		public delegate void TerminalParseSentenceEventHandler(Object sender, TerminalParseSentenceEventArgs e);
	}
}
