using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalApi.Classes
{
    public class CommandInfo
    {
        public string Name { get; set; }
        /// <summary>
        /// The category to display info on command
        /// </summary>
        public string Category { get; set; } = "Other";
        /// <summary>
        /// The description of what the command does
        /// </summary>
        public string Description { get; set; }
    }
}
