using Features.TileSystem.Interactables;

namespace Features.TileSystem.UnmovableTile.MineableTiles
{
    public class TreeTile : ResourceTile
    {
        public TreeTile(ITileManager tileManager) : base(tileManager)
        {
            
        }

        protected override Resource GetResource()
        {
            throw new System.NotImplementedException();
        }
    }
}