using UnityEngine;

namespace Features.TileSystem
{
    public interface IInteractable
    {
        public bool OnInteract(GameObject interactor);
    }
}