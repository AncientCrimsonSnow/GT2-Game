using Unity.VisualScripting;
using UnityEngine;

namespace Uilities.Focus
{
    public abstract class Focus_SO<T> : ScriptableObject
    {
        protected T Focus;

        private void OnEnable()
        {
            Restore();
        }

        public virtual void SetFocus(T newFocus)
        {
            Focus = newFocus;
        }
        
        public virtual void Restore()
        {
            //The default value of a reference type is null
            Focus = default;
        }

        public T GetFocus()
        {
            return Focus;
        }
        
        public bool ContainsFocus()
        {
            return !Focus.IsUnityNull();
        }
    }
}