using UnityEngine;

namespace Features.TileSystem.Scripts
{
    public class BlockedTileInteractable : ITileInteractable
    {
        public bool CanInteract(GameObject interactor, out string interactionText)
        {
            interactionText = "";
            return false;
        }

        public bool TryInteract(GameObject interactor)
        {
            return false;
        }

        public bool CanCast(GameObject caster, out string interactionText)
        {
            interactionText = "";
            return false;
        }

        public bool TryCast(GameObject caster)
        {
            return false;
        }

        public bool IsMovable()
        {
            return false;
        }
    }
}
