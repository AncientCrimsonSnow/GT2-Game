using UnityEngine;

namespace Features.Character.Scripts.Magic
{
    public class MagicInputRegistrator : MonoBehaviour
    {
        [Header("Topic")]
        [SerializeField] private CastInputFocus castInputFocus;
        [SerializeField] private BaseCastInput castInputBehaviour;

        [Header("RegistrationTime")] 
        [SerializeField] private bool awake;
        [SerializeField] private bool start;

        private void Awake()
        {
            if (!awake) return;
            
            castInputFocus.PushFocus(castInputBehaviour);
        }

        private void Start()
        {
            if (!start) return;
            
            castInputFocus.PushFocus(castInputBehaviour);
        }
    }
}
