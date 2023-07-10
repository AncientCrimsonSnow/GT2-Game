using UnityEngine;

namespace DataStructures.Variables
{
    [CreateAssetMenu(fileName = "NewVector2IntVariable", menuName = "DataStructures/Variables/Vector2IntVariable")]
    public class Vector2IntVariable : AbstractVariable<Vector2Int>
    {
        public void Add(Vector2Int value)
        {
            runtimeValue += value;
            if(onValueChanged != null) onValueChanged.Raise();
        }

        public void Add(Vector2IntVariable value)
        {
            runtimeValue += value.runtimeValue;
            if(onValueChanged != null) onValueChanged.Raise();
        }
    }
}