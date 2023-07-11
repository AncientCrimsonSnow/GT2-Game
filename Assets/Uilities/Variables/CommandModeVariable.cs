using UnityEngine;

namespace Uilities.Variables
{
    [CreateAssetMenu(fileName = "NewCommandModeVariable", menuName = "DataStructures/Variables/CommandModeVariable")]
    public class CommandModeVariable : AbstractVariable<CommandMode>
    {
        
    }
    
    public enum CommandMode
    {
        Excavate,
        TransportLine
    }
}
