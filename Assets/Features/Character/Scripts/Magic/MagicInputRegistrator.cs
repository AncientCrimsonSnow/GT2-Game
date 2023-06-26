using UnityEngine;

namespace Features.Character.Scripts.Magic
{
    public class MagicInputRegistrator : MonoBehaviour
    {
        [Header("Topic")]
        [SerializeField] private CastInputFocus castInputFocus;
        [SerializeField] private BaseCastInput castInputBehaviour;
        [SerializeField] private bool setAsRestore;

        [Header("RegistrationTime")] 
        [SerializeField] private bool awake;
        [SerializeField] private bool start;

        private void Awake()
        {
            if (!awake) return;
            
            castInputFocus.SetFocus(castInputBehaviour);
            if (setAsRestore)
            {
                castInputFocus.SetCurrentAsRestore();
            }
        }

        private void Start()
        {
            if (!start) return;
            
            castInputFocus.SetFocus(castInputBehaviour);
            if (setAsRestore)
            {
                castInputFocus.SetCurrentAsRestore();
            }
        }
    }
}
