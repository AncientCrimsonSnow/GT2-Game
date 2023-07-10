using UnityEngine;
using UnityEngine.Events;

namespace DataStructures.Event
{
    public class ActionEventListener : MonoBehaviour
    {
        [SerializeField] private ActionEvent @event;
        [SerializeField] private UnityEvent response;

        private void OnEnable()
        {
            @event.RegisterListener(response.Invoke);
        }

        private void OnDisable()
        {
            @event.UnregisterListener(response.Invoke);
        }
    }
}
