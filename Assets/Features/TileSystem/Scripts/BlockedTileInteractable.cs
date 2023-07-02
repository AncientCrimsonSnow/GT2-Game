using UnityEngine;

namespace Features.TileSystem.Scripts
{
    public class BlockedTileInteractable : ITileInteractable
    {
        public bool TryInteract(GameObject interactor)
        {
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
