using System;

namespace TerminalApi.Events
{
	public static partial class Events
	{
		public class TerminalEventArgs : EventArgs
		{
			public Terminal Terminal { get; set; }
		}

		public delegate void TerminalEventHandler(Object sender, TerminalEventArgs e);

		public class TerminalParseSentenceEventArgs : TerminalEventArgs
		{
			public TerminalNode ReturnedNode { get; set; } 
			public string SubmittedText { get; set; }
		}

		public delegate void TerminalParseSentenceEventHandler(Object sender, TerminalParseSentenceEventArgs e);

		public class TerminalTextChangedEventArgs : TerminalEventArgs 
		{ 
			public string NewText;
			public string CurrentInputText;
		}

		public delegate void TerminalTextChangedEventHandler(Object sender, TerminalTextChangedEventArgs e);
	}
}
