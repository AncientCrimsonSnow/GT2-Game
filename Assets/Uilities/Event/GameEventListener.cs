using UnityEngine;
using UnityEngine.Events;

namespace Uilities.Event
{
    public class GameEventListener : MonoBehaviour
    {
        [SerializeField] private GameEvent @event;
        [SerializeField] private UnityEvent response;

        private void OnEnable()
        {
            @event.RegisterListener(this);
        }

        private void OnDisable()
        {
            @event.UnregisterListener(this);
        }

        public void OnEventRaised()
        {
            response.Invoke();
        }
    }
}
