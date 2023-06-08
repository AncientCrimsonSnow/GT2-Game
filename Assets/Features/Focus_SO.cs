using UnityEngine;

namespace Features
{
    public abstract class Focus_SO<T> : ScriptableObject
    {
        protected T Focus;

        public virtual void SetFocus(T newFocus)
        {
            Focus = newFocus;
        }

        public T GetFocus()
        {
            return Focus;
        }
    }
}