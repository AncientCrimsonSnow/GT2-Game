using UnityEngine;

namespace Features.TileSystem
{
    public interface IInteractable
    {
        /// <summary>
        /// Interaction for direct player input
        /// </summary>
        /// <param name="interactor"></param>
        /// <returns>Whether the interaction was successful or not</returns>
        public bool TryInteract(GameObject interactor);
    }
}