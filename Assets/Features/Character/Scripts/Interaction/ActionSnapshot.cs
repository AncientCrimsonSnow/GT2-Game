using System;
using Features.ReplaySystem.Record;

namespace Features.Character.Scripts.Interaction
{
    public class ActionSnapshot : IInputSnapshot
    {
        private readonly Action<float> _action;
        
        public ActionSnapshot(Action<float> action)
        {
            _action = action;
        }
    
        public void Tick(float tickDurationInSeconds)
        {
            _action.Invoke(tickDurationInSeconds);
        }
    }
}
