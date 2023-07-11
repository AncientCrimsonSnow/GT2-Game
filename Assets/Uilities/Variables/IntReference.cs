using System;

namespace Uilities.Variables
{
    [Serializable]
    public class IntReference : AbstractReference<int>
    {
        public IntReference(int value) : base(value)
        {
        }
    }
}
