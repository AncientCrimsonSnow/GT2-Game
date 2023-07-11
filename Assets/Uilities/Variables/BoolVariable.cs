using UnityEngine;

namespace Uilities.Variables
{
    [CreateAssetMenu(fileName = "NewBoolVariable", menuName = "DataStructures/Variables/BoolVariable")]
    public class BoolVariable : AbstractVariable<bool>
    {
        public void Toggle()
        {
            Set(!runtimeValue);
        }
        
        public void SetTrue()
        {
            Set(true);
        }

        public void SetFalse()
        {
            Set(false);
        }

        public bool Not()
        {
            return !Get();
        }
    }
}
