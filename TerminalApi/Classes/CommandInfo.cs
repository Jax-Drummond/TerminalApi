﻿using System;

namespace TerminalApi.Classes
{
    public class CommandInfo
    { 
        /// <summary>
        /// The category to display info on command
        /// </summary>
        public string Category { get; set; } = "Other";
        /// <summary>
        /// The description of what the command does
        /// </summary>
        public string Description { get; set; } = "";

        // For callbacks

        /// <summary>
        /// The node that will trigger your callback function
        /// </summary>
        public TerminalNode TriggerNode { get; set; }

        /// <summary>
        /// A function that should return the string that is wanted to be displayed
        /// </summary>
        public Func<string> DisplayTextSupplier { get; set; }

    }
}
