using UnityEngine;

namespace Features.TileSystem.Scripts
{
    public interface IInteractable
    {
        public bool CanInteract(GameObject interactor, out string interactionText);
        public bool TryInteract(GameObject interactor);

        public bool CanCast(GameObject caster, out string interactionText);
        public bool TryCast(GameObject caster);
    }
}