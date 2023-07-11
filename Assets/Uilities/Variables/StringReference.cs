using System;

namespace Uilities.Variables
{
    [Serializable]
    public class StringReference : AbstractReference<string>
    {
        public StringReference(string value) : base(value)
        {
        }
    }
}
