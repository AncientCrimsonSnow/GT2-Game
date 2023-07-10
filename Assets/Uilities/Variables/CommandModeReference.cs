using System;

namespace DataStructures.Variables
{
    [Serializable]
    public class CommandModeReference : AbstractReference<CommandMode>
    {
        public CommandModeReference(CommandMode value) : base(value)
        {
        }
    }
}
