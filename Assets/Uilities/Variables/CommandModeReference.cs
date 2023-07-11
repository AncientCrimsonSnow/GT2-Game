using System;

namespace Uilities.Variables
{
    [Serializable]
    public class CommandModeReference : AbstractReference<CommandMode>
    {
        public CommandModeReference(CommandMode value) : base(value)
        {
        }
    }
}
