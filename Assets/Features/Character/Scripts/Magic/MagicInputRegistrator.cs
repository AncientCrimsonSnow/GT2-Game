using UnityEngine;

namespace Features.Character.Scripts.Magic
{
    public class MagicInputRegistrator : MonoBehaviour
    {
        [Header("Topic")]
        [SerializeField] private MagicInputFocus magicInputFocus;
        [SerializeField] private BaseMagicInput magicInputBehaviour;
        [SerializeField] private bool setAsRestore;

        [Header("RegistrationTime")] 
        [SerializeField] private bool awake;
        [SerializeField] private bool start;

        private void Awake()
        {
            if (!awake) return;
            
            magicInputFocus.SetFocus(magicInputBehaviour);
            if (setAsRestore)
            {
                magicInputFocus.SetCurrentAsRestore();
            }
        }

        private void Start()
        {
            if (!start) return;
            
            magicInputFocus.SetFocus(magicInputBehaviour);
            if (setAsRestore)
            {
                magicInputFocus.SetCurrentAsRestore();
            }
        }
    }
}
