namespace Features.TileSystem
{
    public interface IPassiveInteractable
    {
        /// <summary>
        /// Automatically triggered interaction at the end of each tick
        /// </summary>
        public void OnPassiveInteract();
    }
}