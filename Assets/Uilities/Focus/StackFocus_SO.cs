using System.Collections.Generic;
using Features;
using UnityEngine;

namespace Uilities.Focus
{
    public class StackFocus_SO<T> : Focus_SO<T>
    {
        private Stack<T> _stack = new Stack<T>();
        public int count;
    
        //TODO: implement Stack: necro -> skeleton -> building -> skeleton -> necro
        
        public override void Restore()
        {
            count = 0;
            _stack = new Stack<T>();
            base.Restore();
        }

        public virtual void PushFocus(T newValue)
        {
            _stack.Push(newValue);
            SetFocus(newValue);
            count++;
        }

        public void PopFocus()
        {
            if (_stack.Count > 0)
            {
                count--;
                _stack.Pop();

                if (_stack.Count > 0)
                {
                    SetFocus(_stack.Peek());
                }
            }
            else
            {
                Restore();
            }
        }
    }
}
