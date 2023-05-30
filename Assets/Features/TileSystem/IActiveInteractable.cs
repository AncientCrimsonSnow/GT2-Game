using UnityEngine;

namespace Features.TileSystem
{
    public interface IActiveInteractable
    {
        /// <summary>
        /// Interaction for direct player input
        /// </summary>
        /// <param name="interactor"></param>
        /// <returns>Whether the interaction was successful or not</returns>
        public bool OnActiveInteract(GameObject interactor);
    }
}