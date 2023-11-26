using System;

namespace TerminalApi.Events
{
	public class TerminalAwakeEventArgs : EventArgs
	{
		public Terminal Terminal { get; set; }
	}

	public delegate void TerminalAwakeEventHandler(Object sender, TerminalAwakeEventArgs e);
}
